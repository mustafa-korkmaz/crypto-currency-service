using System;
using System.Linq;
using MoneyMarket.Business.Setting;
using MoneyMarket.Common;

namespace MoneyMarket.Business.Common
{
    public static class CommonBusiness
    {
        public static DateTime GetSlackTeamExpirationDate(AccountType accountType)
        {
            var settingBusiness = new SettingBusiness();

            var settings = settingBusiness.All().First(p => p.Id == DatabaseKey.Setting.TrialAccountExpirationDays);

            return DateTime.UtcNow.AddDays(int.Parse(settings.Value));
        }

    }
}

