using System.Collections.Generic;
using System.Linq;
using MoneyMarket.Business.Caching.Provider;
using MoneyMarket.DataAccess;

namespace MoneyMarket.Business.Slack
{
    public class ScopeBusiness
    {
        private readonly UnitOfWork _uow = new UnitOfWork();
        private readonly IRepository<DataAccess.Models.Scope> _repository;

        public ScopeBusiness()
        {
            _repository = _uow.Repository<DataAccess.Models.Scope>();
        }

        /// <summary>
        /// cachable result for scopes.
        /// max datetimeOffset cached
        /// </summary>
        /// <returns>scopes DTO object list</returns>
        [CacheableResult]
        public IEnumerable<Dto.Scope> All()
        {
            var settings = _repository.AsQueryable()
                .Select(p => new Dto.Scope
                {
                    Id = p.Id,
                    Name = p.Name
                })
                .ToList();

            return settings;
        }
    }
}

