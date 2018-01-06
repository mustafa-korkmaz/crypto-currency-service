using System.Linq;
using MoneyMarket.Business.HttpClient;
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

            var entity = GetTeamNotificationByKeyAndCurrency(teamNotificationDto);

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
            var entity = GetTeamNotificationByKeyAndCurrency(teamNotification);

            if (entity != null)
            {
                _repository.Delete(entity);
                _uow.Save();
            }
        }

        /// <summary>
        /// returns notification if exists by teamId, currency and key search filter
        /// </summary>
        /// <returns></returns>
        private DataAccess.Models.TeamNotification GetTeamNotificationByKeyAndCurrency(Dto.TeamNotification teamNotification)
        {
            return _repository.AsQueryable()
                .FirstOrDefault(p => p.Key == teamNotification.Key
                                     && p.NotificationType == teamNotification.NotificationType
                                     && p.TeamId == teamNotification.TeamId);
        }

    }
}
