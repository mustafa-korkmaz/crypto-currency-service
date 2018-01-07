using System;
using System.Collections.Generic;
using System.Linq;
using MoneyMarket.Common;
using MoneyMarket.Common.Response;
using MoneyMarket.DataAccess;
using MoneyMarket.Dto;

namespace MoneyMarket.Business.Notification
{
    public class TeamNotificationBusiness : ICrudOperationBusiness
    {
        private readonly UnitOfWork _uow = new UnitOfWork();
        private readonly IRepository<DataAccess.Models.TeamNotification> _repository;

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
            throw new System.NotImplementedException();
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

        public void SendSlackTeamNotifications()
        {
            var teamNotifications = GetTeamNotifications();

            var priceTrackerAlarmNotifications = teamNotifications.Where(p => p.NotificationType == NotificationType.PriceTracker);

            var assetReminderNotifications = teamNotifications.Where(p => p.NotificationType == NotificationType.AssetReminder);

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
                key = teamNotification.Key.Split(':')[0] + ":";

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

        private void SendPriceTrackerAlarmNotifications(IEnumerable<Dto.TeamNotification> priceTrackerAlarmNotifications)
        {
            //:bell: slack alarm emoji
            //BtcTurk Btc: 17,022.33323 Usd
        }

        private void SendAssetReminderNotifications(IEnumerable<Dto.TeamNotification> assetReminderNotifications)
        {
            //:pushpin: slack pin emoji
            //mute: 100.00 Btc
            //Assets total: 1,718,393.08 Usd

            var utcNow = DateTime.UtcNow;

            //get notifications needs to be pushed by time interval in minutes.
            var filteredNotifications = assetReminderNotifications
                .Where(p => p.LastExecutedAt.AddMinutes(p.TimeInterval) < utcNow);
        }
    }
}
