using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml.Serialization;
using MoneyMarket.Common.Response;

namespace MoneyMarket.Common.Helper
{
    public static class Statics
    {

        public static string GetApiUrl()
        {
            AppSettingsReader configurationAppSettings = new AppSettingsReader();
            return (string)configurationAppSettings.GetValue(ConfigKeys.ApiUrl, typeof(string));
        }

        public static string GetSlackApiUrl()
        {
            AppSettingsReader configurationAppSettings = new AppSettingsReader();
            return (string)configurationAppSettings.GetValue(ConfigKeys.SlackApiUrl, typeof(string));
        }

        /// <summary>
        /// return from web.config [key] value
        /// </summary>
        /// <param name="key">config app key</param>
        /// <returns></returns>
        public static string GetConfigKey(string key)
        {
            AppSettingsReader configurationAppSettings = new AppSettingsReader();
            return (string)configurationAppSettings.GetValue(key, typeof(string));
        }

        public static string ConnectionUrlToElasticServer
        {
            get
            {
                string defaultValue = "http://localhost:9200/";
                return ConfigurationManager.AppSettings["ConnectionUrlToElasticServer"] ?? defaultValue;
            }
        }

        public static int ElasticSearchTimeOutInSeconds
        {
            get
            {
                int defaultValue;

                if (!int.TryParse(ConfigurationManager.AppSettings["ElasticSearchTimeOutInSeconds"], out defaultValue))
                {
                    defaultValue = 5;
                }

                return defaultValue;
            }
        }

        public static string GetStatusText(Status status)
        {
            switch (status)
            {
                case Status.Active:
                    return "Aktif";
                case Status.Passive:
                    return "Pasif";
                case Status.Suspended:
                    return "Donduruldu";
                case Status.Deleted:
                    return "Silindi";
                case Status.Tracking:
                    return "Takip ediliyor";
                case Status.Untracking:
                    return "Takip edilmiyor";
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }
        }

        public static Language GetLanguage(string langText)
        {
            switch (langText.ToLower())
            {
                case "en":
                    return Language.English;
                case "tr":
                    return Language.Turkish;

            }

            return Language.Unknown;
        }

        public static MainCurrency GetMainCurrency(string currencyText)
        {
            switch (currencyText.ToLower())
            {
                case "tl":
                    return MainCurrency.Try;
                case "usd":
                case "$":
                    return MainCurrency.Usd;

            }

            return MainCurrency.Unknown;
        }

        public static MainCurrency GetMainCurrency(int mainCurrency)
        {
            return (MainCurrency)mainCurrency;
        }

        public static AlarmType GetAlarmType(int alarmType)
        {
            return (AlarmType)alarmType;
        }

        public static Currency GetCurrency(string currencyText)
        {
            /*
              Unknown = 0,
                Btc,
                Eth,
                Etc,
                Stellar,
                Ripple,
                Nxt,
                Lisk,
                Nem,
                Eos
             */

            switch (currencyText.ToLower())
            {
                case "eth":
                    return Currency.Eth;
                case "btc":
                    return Currency.Btc;
                case "etc":
                    return Currency.Etc;
                case "stellar":
                case "xlm":
                    return Currency.Stellar;
                case "ripple":
                case "xrp":
                    return Currency.Ripple;
                case "nxt":
                    return Currency.Nxt;
                case "lisk":
                    return Currency.Lisk;
                case "nem":
                case "xem":
                    return Currency.Nem;
                case "eos":
                    return Currency.Eos;
                case "trx":
                case "tron":
                    return Currency.Tron;
                case "ıgnis":
                case "ignis":
                    return Currency.Ignis;
                case "poly":
                    return Currency.Poly;
                case "ltc":
                    return Currency.Ltc;
            }

            return Currency.Unknown;
        }

        public static Currency GetCurrency(int currency)
        {
            return (Currency)currency;
        }

        public static Provider GetProvider(string providerText)
        {
            switch (providerText.ToLower())
            {

                case "bitstamp":
                    return Provider.BitStamp;
                case "btcturk":
                    return Provider.BtcTurk;
                case "coinmarketcap":
                    return Provider.CoinMarketCap;
                case "bittrex":
                    return Provider.Bittrex;
                case "hitbtc":
                    return Provider.HitBtc;
                case "binance":
                    return Provider.Binance;
            }

            return Provider.Unknown;
        }

        public static Provider GetProvider(int provider)
        {
            return (Provider)provider;
        }


        public static string GetNotificationStatusText(NotificationStatus notificationStatus)
        {
            switch (notificationStatus)
            {
                case NotificationStatus.HasError:
                    return NotificationMessage.HasError;
                case NotificationStatus.WaitingForSendingNotification:
                    return NotificationMessage.WaitingForSendingNotification;
                case NotificationStatus.NotificationSent:
                    return NotificationMessage.NotificationSent;
                case NotificationStatus.NotificationRead:
                    return NotificationMessage.NotificationRead;
                default:
                    throw new ArgumentOutOfRangeException(nameof(notificationStatus), notificationStatus, null);
            }
        }

        public static string GetApprovalText(bool isApproved)
        {
            return isApproved ? "Onaylı" : "Onay Bekliyor";
        }

        public static DateTime GetTurkeyCurrentDateTime()
        {
            DateTime utcTime = DateTime.UtcNow;
            //TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("GTB Standard Time");
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(utcTime, tzi); // convert from utc to local
        }

        //public static BusinessResponse<string> SaveImage(string imageData, string filePath)
        //{
        //    var businessResp = new BusinessResponse<string>
        //    {
        //        ResponseCode = ResponseCode.Fail,
        //    };

        //    Regex regex = new Regex(@"base64,(.*)");
        //    Match match = regex.Match(imageData);

        //    if (!match.Success)
        //    {
        //        return businessResp;
        //    }

        //    string imageDataBase64String = match.Value.Substring(7); // trim 'base64,'

        //    regex = new Regex(@"data[:-]image(.*);"); //data-image/png;

        //    match = regex.Match(imageData);

        //    if (!match.Success)
        //    {
        //        return businessResp;
        //    }

        //    string fileExtension = "." + match.Value.Substring(11).Replace(";", ""); // trim 'data-image/' returns: .png

        //    var guid = Guid.NewGuid().ToString();

        //    string fullPath = filePath + guid + fileExtension;

        //    var sourceBytes = Convert.FromBase64String(imageDataBase64String);

        //    var targetBytes = CropImage(sourceBytes, 144, 144);

        //    using (var imageFile = new FileStream(fullPath, FileMode.Create))
        //    {
        //        imageFile.Write(targetBytes, 0, targetBytes.Length);
        //        imageFile.Flush();
        //    }

        //    businessResp.ResponseData = guid + fileExtension; // return file name.
        //    businessResp.ResponseCode = ResponseCode.Success;
        //    return businessResp;
        //}

        //public static byte[] CropImage(byte[] imageStream, int width, int height, bool needToFill = false)
        //{
        //    Image image;

        //    using (var ms = new MemoryStream(imageStream))
        //    {
        //        image = Image.FromStream(ms);
        //    }

        //    int sourceWidth = image.Width;
        //    int sourceHeight = image.Height;

        //    int sourceX = 0;
        //    int sourceY = 0;
        //    double destX = 0;
        //    double destY = 0;

        //    double nScale = 0;
        //    double nScaleW = 0;
        //    double nScaleH = 0;

        //    nScaleW = (width / (double)sourceWidth);
        //    nScaleH = (height / (double)sourceHeight);
        //    if (!needToFill)
        //    {
        //        nScale = Math.Min(nScaleH, nScaleW);
        //    }
        //    else
        //    {
        //        nScale = Math.Max(nScaleH, nScaleW);
        //        destY = (height - sourceHeight * nScale) / 2;
        //        destX = (width - sourceWidth * nScale) / 2;
        //    }

        //    if (nScale > 1)
        //        nScale = 1;

        //    int destWidth = (int)Math.Round(sourceWidth * nScale);
        //    int destHeight = (int)Math.Round(sourceHeight * nScale);

        //    Bitmap bmPhoto = null;
        //    try
        //    {
        //        bool sizesAvailable = true;

        //        if (destWidth < width)
        //        {
        //            destWidth = width;
        //            sizesAvailable = false;
        //        }

        //        if (destHeight < height)
        //        {
        //            destHeight = height;
        //            sizesAvailable = false;
        //        }

        //        if (sizesAvailable)
        //        {
        //            bmPhoto = new Bitmap(destWidth + (int)Math.Round(2 * destX), destHeight + (int)Math.Round(2 * destY));
        //        }
        //        else
        //        {
        //            bmPhoto = new Bitmap(destWidth, destHeight);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new ApplicationException(string.Format("destWidth:{0}, destX:{1}, destHeight:{2}, desxtY:{3}, Width:{4}, Height:{5}", destWidth, destX, destHeight, destY, width, height), ex);
        //    }
        //    using (Graphics grPhoto = Graphics.FromImage(bmPhoto))
        //    {
        //        grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //        grPhoto.CompositingQuality = CompositingQuality.HighQuality;
        //        grPhoto.SmoothingMode = SmoothingMode.HighQuality;

        //        Rectangle to = new Rectangle((int)Math.Round(destX), (int)Math.Round(destY), destWidth, destHeight);
        //        Rectangle from = new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight);
        //        //Console.WriteLine("From: " + from.ToString());
        //        //Console.WriteLine("To: " + to.ToString());
        //        grPhoto.DrawImage(image, to, from, GraphicsUnit.Pixel);
        //    }

        //    using (var ms = new MemoryStream())
        //    {
        //        bmPhoto.Save(ms, image.RawFormat);
        //        return ms.ToArray();
        //    }
        //}

        /// <summary>
        /// returns client ip
        /// </summary>
        /// <returns></returns>
        public static string GetIp()
        {
            string ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(ip))
            {
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            return ip;
        }

        /// <summary>
        /// returns product's web site bot tracker class assembly info via web site's domain name
        /// </summary>
        /// <param name="domainName"></param>
        /// <returns></returns>
        public static string GetWebSiteClassAssemblyInfo(string domainName)
        {
            var webSiteClassName = domainName.Replace("-", "").Split('.')[0].ToCamelCase();
            return "Drop.Business.Product.Integration." + webSiteClassName;
        }

        public static string GetSqlWhereConditionInValues<T>(IEnumerable<T> inValues)
        {
            return string.Join(", ", inValues.Select(p => p.ToString()));
        }

        public static decimal GetLatestUsdSellRate()
        {
            var currencyApiUrl = GetConfigKey(ConfigKeys.CurrencyApiUrl);

            var resp = GetWebResponse(currencyApiUrl);

            using (TextReader sr = new StringReader(resp.ResponseData))
            {
                var serializer = new XmlSerializer(typeof(TcmbDate));
                TcmbDate root = (TcmbDate)serializer.Deserialize(sr);

                var list = root.Currency; //usd.selling;

                var dollar = list.First(p => p.CurrencyCode == "USD").BanknoteSelling;

                CultureInfo provider = new CultureInfo("en-GB");
                return decimal.Parse(dollar, provider);
            }
        }

        /// <summary>
        /// default http client
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static HttpClientResponse<string> GetWebResponse(string url)
        {
            var resp = new HttpClientResponse<string>();

            WebClient client = new WebClient();

            client.Encoding = Encoding.UTF8;
            client.Headers.Add("User-Agent: Doviz/5.2.0");

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            try
            {
                string html = client.DownloadString(url);
                if (!string.IsNullOrEmpty(html))
                {
                    resp.ResponseData = html;
                    resp.HttpStatusCode = HttpStatusCode.OK;
                }
                else
                {
                    resp.HttpStatusCode = HttpStatusCode.BadRequest;
                    resp.ResponseMessage = "ContentNoFound";
                }
            }
            catch (WebException exc)
            {
                var excResp = exc.Response as HttpWebResponse;

                if (excResp == null)
                {
                    resp.HttpStatusCode = HttpStatusCode.InternalServerError;
                    return resp;
                }

                resp.HttpStatusCode = excResp.StatusCode;
            }

            return resp;
        }
    }

    [XmlRoot(ElementName = "Currency")]
    public class TcmbCurrency
    {
        [XmlElement(ElementName = "Unit")]
        public string Unit { get; set; }
        [XmlElement(ElementName = "Isim")]
        public string Isim { get; set; }
        [XmlElement(ElementName = "CurrencyName")]
        public string CurrencyName { get; set; }
        [XmlElement(ElementName = "ForexBuying")]
        public string ForexBuying { get; set; }
        [XmlElement(ElementName = "ForexSelling")]
        public string ForexSelling { get; set; }
        [XmlElement(ElementName = "BanknoteBuying")]
        public string BanknoteBuying { get; set; }
        [XmlElement(ElementName = "BanknoteSelling")]
        public string BanknoteSelling { get; set; }
        [XmlElement(ElementName = "CrossRateUSD")]
        public string CrossRateUSD { get; set; }
        [XmlElement(ElementName = "CrossRateOther")]
        public string CrossRateOther { get; set; }
        [XmlAttribute(AttributeName = "CrossOrder")]
        public string CrossOrder { get; set; }
        [XmlAttribute(AttributeName = "Kod")]
        public string Kod { get; set; }
        [XmlAttribute(AttributeName = "CurrencyCode")]
        public string CurrencyCode { get; set; }
    }

    [XmlRoot(ElementName = "Tarih_Date")]
    public class TcmbDate
    {
        [XmlElement(ElementName = "Currency")]
        public List<TcmbCurrency> Currency { get; set; }
        [XmlAttribute(AttributeName = "Tarih")]
        public string Tarih { get; set; }
        [XmlAttribute(AttributeName = "Date")]
        public string Date { get; set; }
        [XmlAttribute(AttributeName = "Bulten_No")]
        public string Bulten_No { get; set; }
    }
}