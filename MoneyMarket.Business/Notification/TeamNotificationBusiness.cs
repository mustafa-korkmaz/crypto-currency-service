using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoneyMarket.Business.Command;
using MoneyMarket.Business.CryptoCurrency;
using MoneyMarket.Business.Exception;
using MoneyMarket.Business.Setting;
using MoneyMarket.Business.Slack.Integration;
using MoneyMarket.Common;
using MoneyMarket.Common.Helper;
using MoneyMarket.Common.Response;
using MoneyMarket.DataAccess;
using MoneyMarket.Dto;

namespace MoneyMarket.Business.Notification
{
    public class TeamNotificationBusiness : ICrudOperationBusiness
    {
        private readonly UnitOfWork _uow = new UnitOfWork();
        private readonly IRepository<DataAccess.Models.TeamNotification> _repository;
        private readonly SlackApiClient _client = SlackApiClient.Instance;

        public TeamNotificationBusiness()
        {
            _repository = _uow.Repository<DataAccess.Models.TeamNotification>();
        }

        /// <summary>
        /// adds or updates teamNotification
        /// </summary>
        /// <param name="dto"></param>
        public void Add(DtoBase dto)
        {
            var teamNotificationDto = (Dto.TeamNotification)dto;

            var entity = GetTeamNotificationByKeyAndType(teamNotificationDto);

            if (entity == null)
            {
                entity = MappingConfigurator.Mapper.Map<DataAccess.Models.TeamNotification>(dto);

                _repository.Insert(entity);

                _uow.Save();
                dto.Id = entity.Id;

                return;
            }

            //data exists, update record.
            entity.TimeInterval = teamNotificationDto.TimeInterval;
            entity.Key = teamNotificationDto.Key;
            entity.LastExecutedAt = teamNotificationDto.LastExecutedAt;

            _repository.Update(entity);

            _uow.Save();
        }

        public void Edit(DtoBase dto)
        {
            var teamNotificationDto = (Dto.TeamNotification)dto;

            var entity = _repository.GetById(teamNotificationDto.Id);

            entity.LastExecutedAt = teamNotificationDto.LastExecutedAt;

            _repository.Update(entity);

            _uow.Save();
        }

        public void Delete(int id)
        {
            var entity = _repository.GetById(id);

            _repository.Delete(entity);

            _uow.Save();
        }

        public BusinessResponse<T> Get<T>(int id) where T : DtoBase
        {
            throw new System.NotImplementedException();
        }

        public string CurrentUserId { get; set; }

        public void Delete(Dto.TeamNotification teamNotification)
        {
            var entity = GetTeamNotificationByKeyAndType(teamNotification);

            if (entity != null)
            {
                _repository.Delete(entity);
                _uow.Save();
            }
        }

        [AppException]
        public void SendSlackTeamNotifications()
        {
            var teamNotifications = GetTeamNotifications();

            var priceTrackerAlarmNotifications = teamNotifications.Where(p => p.NotificationType == NotificationType.PriceTracker);

            var assetReminderNotifications = teamNotifications.Where(p => p.NotificationType == NotificationType.AssetReminder);

            SendPriceTrackerAlarmNotifications(priceTrackerAlarmNotifications);

            SendAssetReminderNotifications(assetReminderNotifications);
        }

        /// <summary>
        /// returns notification if exists by teamId, currency and key search filter
        /// </summary>
        /// <returns></returns>
        private DataAccess.Models.TeamNotification GetTeamNotificationByKeyAndType(Dto.TeamNotification teamNotification)
        {
            var key = teamNotification.Key;

            if (teamNotification.NotificationType == NotificationType.PriceTracker)
            {
                //for alarms we need to check existance for splitted value of key
                var keyArray = teamNotification.Key.Split(':');

                key = keyArray[0] + ":" + keyArray[1] + ":";

                return _repository.AsQueryable()
                    .FirstOrDefault(p => p.Key.Contains(key)
                                         && p.NotificationType == teamNotification.NotificationType
                                         && p.TeamId == teamNotification.TeamId);
            }

            return _repository.AsQueryable()
                .FirstOrDefault(p => p.Key == key
                                     && p.NotificationType == teamNotification.NotificationType
                                     && p.TeamId == teamNotification.TeamId);
        }

        private IEnumerable<Dto.TeamNotification> GetTeamNotifications()
        {
            var teamNotifications = _repository.GetAsQueryable(p => p.Team.IsActive)
                .Select(p => new Dto.TeamNotification
                {
                    Id = p.Id,
                    Key = p.Key,
                    LastExecutedAt = p.LastExecutedAt,
                    NotificationType = p.NotificationType,
                    TimeInterval = p.TimeInterval,
                    TeamId = p.TeamId,
                    Team = new Dto.Team
                    {
                        Id = p.Team.Id,
                        MainCurrency = p.Team.MainCurrency,
                        BotAccessToken = p.Team.BotAccessToken,
                        Language = p.Team.Language,
                        SlackId = p.Team.SlackId,
                        Provider = p.Team.Provider,
                        Channel = p.Team.Channel
                    }
                })
                .ToList();

            return teamNotifications;
        }

        private async void SendPriceTrackerAlarmNotifications(IEnumerable<Dto.TeamNotification> priceTrackerAlarmNotifications)
        {
            //:bell: slack alarm emoji
            //BtcTurk Btc: 17,022.33323 Usd

            var usdSellRate = new SettingBusiness().GetUsdValue();

            //get all crypto currencies
            var cryptoCurrencies = GetCryptoCurrencies();

            foreach (var teamNotification in priceTrackerAlarmNotifications)
            {
                var keyArray = teamNotification.Key.Split(':');
                var provider = Statics.GetProvider(int.Parse(keyArray[0]));
                var currency = Statics.GetCurrency(int.Parse(keyArray[1]));
                var limitAmount = keyArray[2].ToMoneyMarketDecimalFormat();
                var mainCurrency = Statics.GetMainCurrency(int.Parse(keyArray[3]));

                IEnumerable<Dto.CryptoCurrency> alarms;

                if (mainCurrency == MainCurrency.Try)
                {
                    alarms = cryptoCurrencies.Where(p => (p.UsdValue / usdSellRate) >= limitAmount &&
                                                         p.Currency == currency);
                }
                else
                {
                    alarms = cryptoCurrencies.Where(p => p.UsdValue >= limitAmount && p.Currency == currency);
                }

                if (provider != Provider.Unknown)
                {
                    alarms = alarms.Where(p => p.Provider == provider);
                }

                if (!alarms.Any())
                {
                    //do nothing
                    continue;
                }

                var successMessage = SlackMessageGenerator.GetCryptoCurrencyAlarmMessage(alarms, mainCurrency, usdSellRate);

                await PostMessage(GetSlackExecutionSuccessMessage(successMessage, teamNotification.Team));

                Delete(teamNotification.Id);
            }
        }

        private async void SendAssetReminderNotifications(IEnumerable<Dto.TeamNotification> assetReminderNotifications)
        {
            //:pushpin: slack pin emoji
            //mute: 100.00 Btc
            //Assets total: 1,718,393.08 Usd

            var command = new CommandBusiness().Get(DatabaseKey.Command.GetBalance);

            var utcNow = DateTime.UtcNow;

            //get notifications needs to be pushed by time interval in minutes.
            var filteredNotifications = assetReminderNotifications
                .Where(p => p.LastExecutedAt.AddMinutes(p.TimeInterval) < utcNow);

            foreach (var teamNotification in filteredNotifications)
            {
                var currency = Statics.GetCurrency(int.Parse(teamNotification.Key));

                var balances = GetTeamCryptoCurrencyBalances(currency, teamNotification.TeamId);

                if (!balances.Any())
                {
                    //do nothing
                    continue;
                }

                //get crypto currencies by team.provider
                var cryptoCurrencies = GetCryptoCurrencies(teamNotification.Team.Provider);

                var successText = command.Responses.First(p => p.Language == teamNotification.Team.Language && p.Depth == 0).SuccessText;

                var successMessage = SlackMessageGenerator.GetCryptoCurrencyBalanceMessage(balances, teamNotification.Team.MainCurrency, cryptoCurrencies, successText);

                var pushpinEmoji = ":pushpin:{lf}";

                successMessage = pushpinEmoji + successMessage;

                await PostMessage(GetSlackExecutionSuccessMessage(successMessage, teamNotification.Team));

                teamNotification.LastExecutedAt = DateTime.UtcNow;

                Edit(teamNotification);
            }
        }

        private IEnumerable<Dto.TeamCryptoCurrencyBalance> GetTeamCryptoCurrencyBalances(Currency currency, int teamId)
        {
            var teamCryptoCurrencyBalanceBusiness = new TeamCryptoCurrencyBalanceBusiness();

            return teamCryptoCurrencyBalanceBusiness.GetTeamCryptoCurrencyBalances(teamId, currency);
        }

        private IEnumerable<Dto.CryptoCurrency> GetCryptoCurrencies(Provider provider)
        {
            var crpytoCurrencyBusiness = new CryptoCurrencyBusiness();

            return crpytoCurrencyBusiness.GetCryptoCurrenciesByProvider(provider);
        }

        /// <summary>
        /// returns all crypto currencies
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Dto.CryptoCurrency> GetCryptoCurrencies()
        {
            var crpytoCurrencyBusiness = new CryptoCurrencyBusiness();

            return crpytoCurrencyBusiness.All();
        }

        private async Task<SlackResponse> PostMessage(SlackMessage message)
        {
            var resp = await _client.InvokeApi<SlackMessage, SlackResponse>("chat.postMessage", message);

            return resp.ResponseData;
        }

        /// <summary>
        /// returns success message with given text and team
        /// </summary>
        /// <returns></returns>
        private SlackMessage GetSlackExecutionSuccessMessage(string text, Dto.Team team)
        {
            return new SlackMessage
            {
                text = text,
                token = team.BotAccessToken,
                channel = team.Channel
            };
        }

    }
}
