using System.Threading;

namespace MoneyMarket.Common.Helper
{
    public static class LinqExtensionHelper
    {
        public static string qGroupBegin(this string source)
        {
            source = source.TrimEnd();
            source = string.Concat(source, " ( ");
            source = source.Replace("( OR", "(").Replace("( AND", "");
            return source;
        }

        public static string qGroupEnd(this string source)
        {
            source = source.TrimEnd();
            source = string.Concat(source, " ) ");
            source = source.Replace("OR )", ")").Replace("AND )", ")");
            return source;
        }

        public static string qAnd(this string source)
        {
            return string.Concat(source, " AND ");
        }

        public static string qOr(this string source)
        {
            return string.Concat(source, " OR ");
        }

        public static string qLike(this string source)
        {
            return string.Concat("*", source.Trim('*'), "*");
        }

        public static string qEqals(this string source)
        {
            return string.Concat(source);
        }

        public static string qStartWith(this string source)
        {
            return string.Concat(source.TrimEnd('*'), "*");
        }

        public static string qEndWith(this string source)
        {
            return string.Concat("*", source.TrimStart('*'));
        }

        public static string qWhere(this string source, string key, string value)
        {
            source = source.TrimEnd();
            source = source.Replace(")", ") AND "); //default AND atıyorum
            source = source.Replace("AND  AND", "AND");
            source = source.Replace("OR  OR", "OR");
            return string.Concat(source, $" {key}:{value} ");
        }

        public static string qDateRange(this string source, string key, string startDatetime, string endDatetime)
        {
            return string.Concat(source, $" {key}:[{startDatetime} TO {endDatetime}] ");
        }

        /// <summary>
        /// To Turkish search exp. for wildcard searches
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>lowercase string for wildcard searches</returns>
        public static string ToTurkishSearchExp(this string source)
        {
            string result;

            if (source.EndsWith("\"") && source.StartsWith("\""))
            {
                // tırnaklı aramalarda içerde wildcard (örn: *, ?) kullanılamıyor. 
                result = source.ToLower(Thread.CurrentThread.CurrentCulture);
            }
            else
            {
                result = source.Replace("ı", "?").Replace("I", "?").ToLower(Thread.CurrentThread.CurrentCulture);
            }

            return result;
        }

        /// <summary>
        /// Lowercases the word using threads culture for exact term searches.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>lowercase string for exact term searches</returns>
        public static string ToLowerCase(this string source)
        {
            return source.ToLower(Thread.CurrentThread.CurrentCulture);
        }
    }
}
