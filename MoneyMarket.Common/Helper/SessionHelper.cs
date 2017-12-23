using System.Web;

namespace MoneyMarket.Common.Helper
{
    public static class SessionHelper
    {
        public static T GetValue<T>(string key)
        {
            return (T)HttpContext.Current.Session[key];
        }
        public static void SetValue(string key, object value)
        {
            if (value == null)
                HttpContext.Current.Session.Remove(key);
            else
                HttpContext.Current.Session[key] = value;
        }

        public static void RemoveAll()
        {
            HttpContext.Current.Session.RemoveAll();
        }
    }
}
