using System.Collections.Generic;
using System.Linq;
using MoneyMarket.Business.Caching.Provider;
using MoneyMarket.DataAccess;

namespace MoneyMarket.Business.Setting
{
    public class SettingBusiness
    {
        private readonly UnitOfWork _uow = new UnitOfWork();
        private readonly IRepository<DataAccess.Models.Setting> _repository;

        public SettingBusiness()
        {
            _repository = _uow.Repository<DataAccess.Models.Setting>();
        }

        /// <summary>
        /// cachable result for settings.
        /// max datetimeOffset cached
        /// </summary>
        /// <returns>setting DTO object list</returns>
        [CacheableResult]
        public IEnumerable<Dto.Setting> All()
        {
            var settings = _repository.AsQueryable()
                .Select(p => new Dto.Setting
                {
                    Id = p.Id,
                    Key = p.Key,
                    Value = p.Value
                })
                .ToList();

            return settings;
        }
    }
}

