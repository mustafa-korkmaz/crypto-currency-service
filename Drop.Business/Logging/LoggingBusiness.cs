using MoneyMarket.DataAccess;
using MoneyMarket.DataAccess.Models;

namespace MoneyMarket.Business.Logging
{
    public class LoggingBusiness
    {
        private readonly UnitOfWork _uow = new UnitOfWork();

        public void LogRequestAsync(Dto.RequestLog requestLog)
        {
            var requestEntity = MappingConfigurator.Mapper.Map<RequestLog>(requestLog);

            _uow.Repository<RequestLog>().Insert(requestEntity);
            _uow.SaveAsync();
        }
    }
}