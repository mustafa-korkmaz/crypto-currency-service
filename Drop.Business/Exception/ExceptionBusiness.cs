using MoneyMarket.Common.Helper;
using MoneyMarket.DataAccess;
using MoneyMarket.DataAccess.Models;

namespace MoneyMarket.Business.Exception
{
    public class ExceptionBusiness
    {
        private readonly UnitOfWork _uow = new UnitOfWork();
        private readonly IRepository<CryptoCurrencyException> _exceptionRepository;

        public ExceptionBusiness()
        {
            _exceptionRepository = _uow.Repository<CryptoCurrencyException>();
        }

        public void SaveExceptionAsync(System.Exception exception)
        {
            var exc = new CryptoCurrencyException
            {
                Message = exception.Message,
                StackTrace = exception.StackTrace,
                CreatedAt = Statics.GetTurkeyCurrentDateTime()
            };

            _exceptionRepository.Insert(exc);
            _uow.SaveAsync();
        }
    }
}