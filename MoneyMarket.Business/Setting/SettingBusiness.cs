using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MoneyMarket.Business.Caching.Provider;
using MoneyMarket.Common;
using MoneyMarket.Common.Helper;
using MoneyMarket.DataAccess;
using MoneyMarket.Dto;

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

        public decimal GetUsdValue()
        {
            return GetUsdValueSetting().Value
                .ToMoneyMarketDecimalFormat(); //decimal.Parse(GetUsdValueSetting().Value, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// cachable result for usd value setting.
        /// </summary>
        /// <returns>tl value of usd</returns>
        [CacheableResult(ExpireInMinutes = 30)]
        private Dto.Setting GetUsdValueSetting()
        {
            var usdSetting = _repository.GetById(DatabaseKey.Setting.UsdSellRate);

            return new Dto.Setting
            {
                Value = usdSetting.Value,
                Key = usdSetting.Key
            };
        }

    }
}

