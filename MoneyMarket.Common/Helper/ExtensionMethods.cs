using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace MoneyMarket.Common.Helper
{
    public static class ExtensionMethods
    {
        public static int WordCount(this string str)
        {
            return str.Split(new char[] { ' ', '.', '?' },
                             StringSplitOptions.RemoveEmptyEntries).Length;
        }

        public static string ToText(this Enum myEnum)
        {
            return myEnum.ToString("G");
        }

        public static string JoinToString(this List<string> items, string delimeter)
        {
            return String.Join(delimeter, items.ToArray());
        }

        public static string ToCamelCase(this string value)
        {
            //
            // Uppercase only the first letter in the string when  this extension is called on.
            //
            value = value.ToLower();
            if (value.Length > 0)
            {
                char[] array = value.ToCharArray();
                array[0] = char.ToUpper(array[0], new CultureInfo("en-GB"));
                return new string(array);
            }
            return value;
        }

        public static void SetTableCaptions(this DataTable dt, List<string> captionList)
        {
            for (int i = 0; i < captionList.Count; i++)
            {
                dt.Columns[i].Caption = captionList[i];
            }
        }

        public static void SetTableCaptions(this DataTable dt, int columnIndex, string caption)
        {
            dt.Columns[columnIndex].Caption = caption;
        }

        public static string ToDropDateFormat(this DateTime d)
        {
            return d.ToString("dd.MM.yyyy");
        }

        public static string ToDropDateTimeFormat(this DateTime d)
        {
            return d.ToString("dd.MM.yyyy HH:mm");
        }

        public static string ToDropDateTimeInSecondsFormat(this DateTime d)
        {
            return d.ToString("dd.MM.yyyy HH:mm:ss");
        }

        public static DateTime ToDropDateFormat(this string s)
        {
            return s == null ? DateTime.Today : DateTime.ParseExact(s, "dd.MM.yyyy", null);
        }

        public static DateTime ToDropDateTimeFormat(this string s)
        {
            return s == null ? DateTime.Today : DateTime.ParseExact(s, "dd.MM.yyyy HH:mm", null);
        }

        public static DateTime ToDropDateCriteriaFormat(this string s)
        {
            return s == null ? DateTime.Today : DateTime.ParseExact(s, "yyyyMMdd", null);
        }

        public static string ToDropDateCriteriaFormat(this DateTime d)
        {
            return d.ToString("yyyyMMdd");
        }

        public static string ToMoneyMarketMoneyFormat(this decimal val)
        {
            //return $"{val:#,0.00}"; // "1.234.256,58"
            return val.ToString("n2");
        }

        public static string ToDropCounterFormat(this decimal val)
        {
            return $"{val:#,0}"; // "1.234.256"
        }

        public static string ToMoneyMarketCryptoCurrencyFormat(this decimal val)
        {
            var specifier = "#,0.00####;(#,0.00####)";
            return val.ToString(specifier);
        }

        public static decimal ToMoneyMarketDecimalFormat(this string val)
        {
            var style = NumberStyles.Number | NumberStyles.AllowCurrencySymbol;
            //"6,032.51";
            CultureInfo provider = new CultureInfo("en-GB");
            return decimal.Parse(val, style, provider);
        }

        public static bool ToMoneyMarketDecimalTryParseFormat(this string val, out decimal d)
        {
            var style = NumberStyles.Number | NumberStyles.AllowCurrencySymbol;
            //"6,032.51";
            CultureInfo provider = new CultureInfo("en-GB");
            return decimal.TryParse(val, style, provider, out d);
        }

        public static string ToDocumentName(this string name)
        {
            name = name.ToLower();
            name = name.Replace("ı", "i");
            name = name.Replace("ö", "o");
            name = name.Replace("ü", "u");
            name = name.Replace("ç", "c");
            name = name.Replace("ş", "s");
            name = name.Replace("ğ", "g");

            string retValue = string.Empty;
            string id_long_name = name.Trim();
            string[] id_array = id_long_name.Split(' ');
            int length = id_array.Length;
            for (int i = 0; i < length; i++)
            {
                retValue += id_array[i];
                if (i < length - 1)
                    retValue += "_";
            }
            return retValue;
        }


        public static string SetDataFormat(this string value, ColumnDataFormat format)
        {
            string retValue = string.Empty;
            if (format != ColumnDataFormat.Default)
            {
                switch (format)
                {
                    case ColumnDataFormat.Date:
                        if (string.IsNullOrEmpty(value))
                            return "";
                        retValue = DateTime.Parse(value).ToDropDateFormat();
                        break;
                    case ColumnDataFormat.Money:
                        value = string.IsNullOrEmpty(value) ? "0" : value;
                        retValue = Decimal.Parse(value).ToMoneyMarketMoneyFormat();
                        break;
                    default:
                        break;
                }
            }
            else
                retValue = value;

            return retValue;
        }

        /// <summary>
        /// converts string values as "4999" to decimal 49.99
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal ToTwoDigitDecimal(this string value)
        {
            var style = NumberStyles.Number | NumberStyles.AllowCurrencySymbol;
            var provider = new CultureInfo("en-GB");

            /*4995/100=49.95*/
            return decimal.Parse(value, style, provider) / 100;
        }
    }
}