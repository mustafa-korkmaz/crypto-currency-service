namespace MoneyMarket.Common
{
    public class ExportTypeText
    {
        public const string Category = "category";
        public const string Product = "product";
    }

    public static class MoneyMarketConstant
    {
        public const int ApiTokenValidDays = 30;
        public const string ApiKeyValue = "T.E.A.M"; // taylan.erdi.aliko.mute
        public const string JobApiKeyValue = "T.B.A.M"; // taylan.burak.aliko.mute
        public static string ApiKeyHash = Helper.ApiKey.Instance.GetHashValue(ApiKeyValue);
        public static string JobApiKeyHash = Helper.ApiKey.Instance.GetHashValue(JobApiKeyValue);
        public const string EncyrptingPassword = "T.E.A.M";
        public const string IosCertificatePath = "content/certificates/";
        public const int UntrackableQuantityValue = -1;
        public const decimal UntrackablePriceValue = -1;
        public const string NotificationOwner = "raqun";
    }

    public static class ErrorMessage
    {
        public const string IntegrationKeyError = "Application Integration key error.";
        public const string ApplicationExceptionMessage = "Uygulamada beklenmedik bir hata meydana geldi.\nLütfen daha sonra tekrar deneyiniz.";
        public const string RecordNotFound = "Listelenecek kayıt bulunamadı.";
        public const string LeaguesNotFoundWithRelatedCategory = "Bu kategoriye ait lig kaydı bulunamadı.";
        public const string ApiKeyNotFound = "Api key not found.";
        public const string JobApiKeyNotFound = "Job Api key not found.";
        public const string ChannelTypeNotFound = "Channel type key not found.";
        public const string ApiKeyIncorrect = "Given api key is incorrect.";
        public const string ChannelTypeIncorrect = "Given channel type is incorrect.";
        public const string UserNotFound = "Kullanıcı adı ya da şifre hatalı.";
        public const string UserEmailNotConfirmed = "Üyeliğiniz henüz aktifleştirilmemiş. Lütfen kayıt olduğunuz mail adresinize gönderilen doğrulama maili ile üyeliğinizi aktifleştiriniz.";
        public const string UserNotActive = "Kullanıcı durumu aktif değil.";
        public const string UserEmailNotExists = "Mail adres alanı zounludur.";
        public const string PhoneNumberNotExists = "Telefon alanı zounludur.";
        public const string DeviceKeyNotExists = "DeviceKey alanı zounludur.";
        public const string EmailOrUserNameNotFound = "Girilen değer ile eşleşen kullanıcı adı ya da email bulunamadı.";
        public const string RequestInvalid = "Invalid request.";
        public const string RequestNotFound = "Request body not found";
        public const string ClaimsNotFound = "İşlemi gerçekleştirme yetkiniz bulunmamaktadır.";
        public const string AuthorNotFound = "Yazar bulunamadı ya da kullanıcının yazar rolü mevcut değil.";
        public const string SearchTypeNotFound = "Arama kriteri tipi bulunamadı.";
        public const string AuthorNotSaved = "Yazar kaydetme işlemi başarısız oldu.";
        public const string CouponCannotBeDeleted = "Kupon silme işlemi başarısız oldu.";
        public const string CouponStatusCannotBeChanged = "Kupon onay değişiklik işlemi başarısız oldu.";
        public const string LeagueNotSaved = "Lig kaydetme işlemi başarısız oldu.";
        public const string EventNotSaved = "Maç bilgileri kaydetme işlemi başarısız oldu.";
        public const string EventNotUpdated = "Maç bilgileri güncelleme işlemi başarısız oldu.";
        public const string OddsCannotDeleted = "Maça ait eski bahisleri güncelleme işlemi başarısız oldu.";
        public const string OddsCannotCreated = "Maça ait yeni bahisleri kaydetme işlemi başarısız oldu.";
        public const string EventExists = "Bültende bu kod ile tanımlı farklı bir maç bulunmaktadır.";
        public const string AuthorImageNotSaved = "Yazar resmi kaydetme işlemi başarısız oldu.";
        public const string UserExists = "Kayıt olmak istenilen kullanıcı adı ya da email daha önce kullanılmış.";
        public const string LeagueExists = "Tanımlamaya çalıştığınız lig {0} ({1}) ile çakışıyor. Spor kategori ve lig kodunu benzersiz giriniz.";
        public const string EventExistsWithRelatedLeague = "Silmeye çalıştığınız lig, bir karşılaşma içerisinde tanımlı olduğundan işlem başarısız oldu.";
        public const string EmailSendingFailure = "Email gönderimi başarısız oldu. Lütfen daha sonra tekrar deneyiniz.";
        public const string WrongVerificationLink = "Link geçerliliğini yitirmiş.";
        public const string UserOrEmailNotFound = "Kullanıcı adı ya da email adresi bulunamadı.";
        public const string EventNotStarted = "Sonuç girmek için maç sonunu bekleyiniz.";
        public const string BetTypesNotFound = "Bahis tipi bulunamadı.";
        public const string AuthorListNotFound = "Yazar listesi bulunamadı.";
        public const string PredictionsNotSaved = "Tahmin kaydetme işlemi başarısız oldu.";
        public const string PredictionsNotFound = "Maça ait tahmin bulunamadı.";
        public const string DuplicatedProduct = "Bu ürün daha önce kaydedilmiş.";
        public const string CommentNotSaved = "Yorum kaydetme işlemi başarısız oldu.";
        public const string CommentAndPredictionsNotSaved = "Tahmin ve  yorum kaydetme işlemi başarısız oldu.";
        public const string OddsNotFound = "Bahis bulunamadı.";
        public const string ChannelTypeNotAppropriate = "Sadece mobil bir device key izni bulunmaktadır.";
        public const string NotAuthorized = "İşlemi gerçekleştirme yetkiniz bulunmamaktadır.";
        public const string RecordNotSaved = "Veri kaydetme işlemi başarısız oldu."; // generic not saved error message
        public const string RecordNotUpdated = "Veri güncelleme işlemi başarısız oldu."; // generic not saved error message
        public const string WrongSecurityCode = "Güvenlik kodu hatalı."; // generic not saved error message
        public const string EventAdditionalInfoNotFound = "Maç limit, handikap bilgileri bulunamadı. Bu bilgilerin olması gerektiğini düşünyorsanız yöneticiniz ile görüşün.";
        public const string UserDeviceNotFound = "Kayıtlı mobil device bulunamadı, bildirim gönderilemedi.";
        public const string MobileUserCouponCannotBeDeleted = "Mobil kullanıcıların oluşturduğu kuponları silme yetkiniz bulunmamaktadır.";
        public const string CouponCannotBeCreatedWithGivenCriteria = "Girilen kriterlere uygun bir kupon oluşturulamadı.";
        public const string CouponCreationHasError = "Kupon oluşturma işlemi sırasında hata alındı.\nLütfen daha sonra tekrar deneyiniz.";
        public const string NotIntegratedFirm = "Takip etmek istediğiniz ürüne ait web sitesi henüz sistemimize entegre edilmemiştir.";
        public const string UrlCannotBeParsed = "Takip etmek istediğiniz ürün ile ilgili beklenmedik bir sorun oluştu.\nGirmiş olduğunuz linkin geçerli bir ürüne ait olduğundan emin olunuz.";
        public const string CurrentPasswordWrong = "Mevcut şifreniz doğrulanamadı";
    }

    public static class NotificationMessage
    {
        public const string PriceIncreased = "Takip ettiğin ürünün fiyatı yükseldi.";
        public const string PriceDecreased = "Takip ettiğin ürünün fiyatı düştü.";
        public const string QuantityIncreased = "Takip ettiğin ürünün stok sayısı arttı.";
        public const string QuantityDecreased = "Takip ettiğin ürünün stok sayısı azaldı.";
        public const string DesiredPriceOccured = "Takip ettiğin ürün tam istediğin fiyat!";
        public const string DesiredQuantityOccured = "Takip ettiğin ürün tam istediğin stok sayısında!";
        public const string HasError = "Hatalı";
        public const string WaitingForSendingNotification = "Bildirim kuyruğuna eklendi";
        public const string NotificationSent = "Yeni"; //Gönderildi
        public const string NotificationRead = "Okundu";
        public const string UserInfoChanged = "Kullanıcı bilgilerin değişti.";
        public const string PasswordChanged = "Parolan değişti.";
    }

    public static class ApiResponseMessage
    {
        public const string IntegrationNotFound = "Integration not found";
        public const string ExportTypeNotFound = "Export type not found";
    }

    public static class HttpClientResponseMessage
    {
        public const string ContentNoFound = "Veri bulanamadi.";
        public const string ExportTypeNotFound = "Export type not found";
    }

    public static class SlackBotMessage
    {
        public const string Welcome =
            "Selam!\nLütfen beni _#general_ kanalınıza davet edin ya da _direct message_ yazarak hemen kullanmaya başlayın."
            + "\nHi there!\nPlease invite me your _#general_ channel or send a _direct message_ to start using.\nType `l set en` for English";
        public const string ExportTypeNotFound = "Export type not found";
    }


    public static class BusinessResponseMessage
    {
        public const string BetProgramUpToDate = "Mevcut bülten daha önceden güncellenmiş.";
        public const string WeeksNotFound = "Bülten hafta kodlari bulunamadi.";
        public const string EventsNotFound = "{0} hafta kodlu bülten maclari bulunamadi.";
        public const string EventsParseError = "{0} hafta kodlu bülten maclari parse hatasi.";
        public const string EventsSaveError = "{0} hafta kodlu bülten maclari kaydetme hatasi.";
        public const string BetProgramsNotFound = "Bülten bulunamadı.";
    }

    public static class DataAccessResponseMessage
    {
        public const string RecordNotSaved = "Kayit islemi basarisiz!";
    }

    public static class ContentTpye
    {
        public const string FormUrlencoded = "application/x-www-form-urlencoded";
    }

    public static class WebMethodTypeText
    {
        public const string Get = "Get";
        public const string Post = "Post";
        public const string Put = "Put";
    }

    public static class Mail
    {
        public const string VerificationSubject = "Hesap aktivasyon işlemi";
        public const string ReminderSubject = "Kullanıcı parola resetleme isteği";
        public const string VerificationContent = "Hesabınızı aktifleştirmek için <a href=\"{0}\" taget=\"_blank\"> buraya </a> tıklayınız.<br/>";
        public const string ReminderContent = "Merhaba {0}, <br/>Parolanızı sıfırlamak için <a href=\"{1}\" taget=\"_blank\"> buraya </a> tıklayınız.<br/>";
        public const string Footer = "<br/>Raqun destek ekibi. <br/> <a href=\"http://raqun.co/\" taget=\"_blank\">www.raqun.co</a><br/>";
    }

    public static class ApplicationRole
    {
        public const string Owner = "Owner";
        public const string Admin = "Admin";
        public const string StandartUser = "StandartUser";
        public const string AdminOrStandartUser = "Admin,StandartUser";
        public const string None = "None";
    }

    public static class ApplicationUserClaimType
    {
        public const string Role = "role";
        public const string AccountType = "account";
        public const string AccountExpiresAt = "accExpires";
    }

    public static class ApplicationUserClaimValue
    {
        public const string PremiumAccount = "Premium";
        public const string StandartAccount = "Standart";
        public const string TrialAccount = "Trial";
    }

    public static class DefaultAuthenticationTypes
    {
        public const string ApplicationCookie = "ApplicationCookie";
        public const string ExternalCookie = "ExternalCookie";
    }

    public static class ApiRequest
    {
        public const string ApplicationCookie = "ApplicationCookie";
        public const string ExternalCookie = "ExternalCookie";
    }

    public static class ChannelType
    {
        public const string Ios = "0";
        public const string Android = "1";
        public const string WebApp = "2";
        public const string LandingPage = "3";
    }

    public static class DataGridTableNames
    {
        public const string BetProgramList = "Bülten Listesi";
        public const string EventList = "Maç Listesi";
        public const string MessageList = "Mesaj Listesi";
        public const string OddsList = "Bahis Listesi";
        public const string OddsTypeList = "Bahis Tipi Listesi";
    }

    public static class DataGridTableIds
    {
        public const string BetProgramList = "bet-program-list";
        public const string EventList = "event-list";
        public const string OddsList = "odds-list";
        public const string OddsTypeList = "odds-type-list";
    }

    public static class ControllerName
    {
        public const string BetProgram = "BetProgram";
        public const string Event = "Event";
        public const string Author = "Author";
        public const string Coupon = "Coupon";
        public const string Tip = "Tip";
        public const string Report = "Report";
        public const string Common = "Common";
    }

    public static class ActionName
    {
        public const string FillBetProgramList = "FillBetProgramList";
        public const string FillEventList = "FillEventList";
        public const string FillCurrentBetProgramEventList = "FillCurrentBetProgramEventList";
        public const string FillOddsList = "FillOddsList";
    }

    #region image paths

    public static class ImagePath
    {
        public const string Category = "content/media/category/";
        public const string User = "content/media/user/";
        public const string UserDefault = "content/media/user/avatar.png";
    }

    #endregion image path

    #region UserMessageTypes

    public static class UserMessageType
    {
        public const string None = "none";
        public const string ServerError = "server-error";
        public const string Error = "error";
        public const string Warning = "warning";
        public const string Success = "success";
        public const string Info = "info";
    }

    #endregion  UserMessageTypes

    #region api urls

    public static class ApiUrl
    {
        public const string Register = "Account/Register";
        public const string GetToken = "Token";
        public const string Login = "Account/Login";
        public const string Logout = "Account/Logout";
        public const string SlackOAuth = "oauth.access";
        public const string SlackGrant = "https://slack.com/oauth/authorize?client_id={0}&scope={1}&redirect_uri={2}";
        public const string SlackRedirectUri = "https://ccs.raqun.co/grantsuccess";
    }

    #endregion api urls

    #region Session

    public static class SessionVariables
    {
        public const string Breadcrumbs = "Breadcrumbs";
        public const string ApplcationUser = "ApplcationUser";
        public const string LeaguesDictionary = "LeaguesDictionary";
        public const string CaptchaCode = "CaptchaCode";
    }

    #endregion Session

    #region RequestType

    public static class RequestTypeText
    {
        public const string Complaint = "Şikayet";
        public const string Suggession = "Öneri";
        public const string Wish = "İstek";
        public const string Other = "Diğer";
    }

    #endregion RequestType


    #region cookies

    public static class Cookies
    {
        public const string AccessToken = "AccessToken";
    }

    #endregion cookies

    #region cofig keys

    public static class ConfigKeys
    {
        public const string ApiUrl = "ApiUrl";
        public const string SlackApiUrl = "SlackApiUrl";
        public const string SlackToken = "SlackToken";
        public const string SlackClientId = "SlackClientId";
        public const string SlackScope = "SlackScope";
        public const string SlackTokenAkhisar = "SlackTokenAkhisar";
        public const string XlmBalance = "XlmBalance";
        public const string EthDiff = "EthDiff";
        public const string SlackClientSecret = "SlackClientSecret";
        public const string MailFrom = "MailFrom";
        public const string MailPass = "MailPass";
        public const string MailServer = "MailServer";
        public const string MailPort = "MailPort";
        public const string MailUseSsl = "MailUseSSL";
        public const string MailDisplayName = "MailDisplayName";
        public const string SendNotifications = "SendNotifications";
        public const string IosCerFile = "IosCerFile";
        public const string IosCerFilePwd = "IosCerFilePwd";
        public const string AndroidSenderId = "AndroidSenderId";
        public const string AndroidAuthToken = "AndroidAuthToken";
        public const string MaxCouponGeneratorPredictionLimit = "MaxCouponGeneratorPredictionLimit";
        public const string UseProxy = "UseProxy";
        public const string ProxyUrl = "ProxyUrl";
        public const string MaxBulkInsertCount = "MaxBulkInsertCount";
        public const string MaxProductRefreshCount = "MaxProductRefreshCount";
    }

    #endregion cofig keys

    #region request headers

    public static class RequestHeader
    {
        public const string Authorization = "Authorization";
        public const string ApiKey = "ApiKey";
        public const string JobApiKey = "JobApiKey";
        public const string ChannelType = "ChannelType";
    }

    #endregion request headers

    #region database Ids

    /// <summary>
    /// includes db related special keys or ids
    /// </summary>
    public static class DatabaseKey
    {
        public static class ApplicationRoleId
        {
            public const string Admin = "ede64fc4-7b30-45f7-b92d-e71f3d0027b6";
            public const string Owner = "911f5c44-c2d1-43aa-9247-ec71fbc17956";
        }

        public static class Setting
        {
            public const int UsdSellRate = 1;
            public const int TrialAccountMaxUserLimit = 2;
            public const int StandartAccountMaxUserLimit = 3;
            public const int PremiumAccountMaxUserLimit = 4;
            public const int TrialAccountExpirationDays = 5;
            public const int StandartAccountExpirationDays = 6;
            public const int PremiumAccountExpirationDays = 7;
        }

        public static class Scope
        {
            public const int SetAlarm = 1;
            public const int SetSettings = 2;
            public const int ListCurrency = 3;
            public const int ListArbitrage = 4;
        }

        public static class CryptoCurrency
        {
            public const int Xlm = 6;
        }

        /// <summary>
        /// job names in db
        /// </summary>
        public static class JobName
        {
            public const string ProductPropertyRefresher = "ProductPropertyRefresher";
        }

        /// <summary>
        /// Category ids in db
        /// </summary>
        public static class CategoryId
        {
            /// <summary>
            /// use this id for unknown categories
            /// </summary>
            public const int Diger = 7;

            public const int SanalPara = 4;
            public const int Oyun = 5;

            #region KadinAyakkabi

            public const int KadinAyakkabi = 8;

            public const int KadinAyakkabi_SporAyakkabi = 114;
            public const int KadinAyakkabi_Terlik = 115;
            public const int KadinAyakkabi_Casual = 116;
            public const int KadinAyakkabi_Sandalet = 117;
            public const int KadinAyakkabi_Babet = 118;
            public const int KadinAyakkabi_Bot = 119;
            public const int KadinAyakkabi_Topuklu = 120;
            public const int KadinAyakkabi_Espadril = 121;

            #endregion KadinAyakkabi

            #region ErkekAyakkabi

            public const int ErkekAyakkabi = 9;

            public const int ErkekAyakkabi_SporAyakkabi = 122;
            public const int ErkekAyakkabi_Terlik = 123;
            public const int ErkekAyakkabi_Casual = 124;
            public const int ErkekAyakkabi_Classic = 125;
            public const int ErkekAyakkabi_Loafer = 126;
            public const int ErkekAyakkabi_Bot = 127;

            #endregion ErkekAyakkabi

            #region  ErkekGiyim

            public const int ErkekGiyim = 10;

            public const int ErkekGiyim_CeketYelek = 11;
            public const int ErkekGiyim_SortCapri = 19;
            public const int ErkekGiyim_Gomlek = 20;
            public const int ErkekGiyim_HirkaKazak = 21;
            public const int ErkekGiyim_MontKaban = 23;
            public const int ErkekGiyim_Pantolon = 24;
            public const int ErkekGiyim_T_shirtSweatshirt = 25;
            public const int ErkekGiyim_Jean = 26;
            public const int ErkekGiyim_TakimElbise = 27;
            public const int ErkekGiyim_IcGiyimPijama = 28;

            #endregion  ErkekGiyim

            #region CocukGiyim

            public const int CocukGiyim = 12;

            public const int CocukGiyim_SortCapri = 49;
            public const int CocukGiyim_Gomlek = 50;
            public const int CocukGiyim_HırkaKazak = 51;
            public const int CocukGiyim_CeketYelek = 52;
            public const int CocukGiyim_MontKaban = 53;
            public const int CocukGiyim_Pantolon = 54;
            public const int CocukGiyim_TshirtSweatshirt = 55;
            public const int CocukGiyim_Jean = 56;
            public const int CocukGiyim_IcGiyimPijama = 58;
            public const int CocukGiyim_Etek = 59;
            public const int CocukGiyim_Atlet = 60;
            public const int CocukGiyim_Yagmurluk = 61;
            public const int CocukGiyim_BereSapka = 62;

            #endregion CocukGiyim

            #region BebekGiyim

            public const int BebekGiyim = 13;
            // children
            public const int BebekGiyim_Gomlek = 63;
            public const int BebekGiyim_CeketYelek = 64;
            public const int BebekGiyim_Pantolon = 65;
            public const int BebekGiyim_TshirtSweatshirt = 66;
            public const int BebekGiyim_Jean = 67;
            public const int BebekGiyim_IcGiyimPijama = 68;
            public const int BebekGiyim_Atlet = 69;
            public const int BebekGiyim_BodyTulum = 70;
            public const int BebekGiyim_Yagmurluk = 71;

            #endregion BebekGiyim

            #region ErkekAksesuar

            public const int ErkekAksesuar = 14;
            //ErkekAksesuar children
            public const int ErkekAksesuar_Cuzdan = 72;
            public const int ErkekAksesuar_Corap = 73;
            public const int ErkekAksesuar_Bileklik = 74;
            public const int ErkekAksesuar_Saat = 75;
            public const int ErkekAksesuar_Kravat = 76;
            public const int ErkekAksesuar_Parfum = 77;
            public const int ErkekAksesuar_Çanta = 78;
            public const int ErkekAksesuar_BereSapka = 79;
            public const int ErkekAksesuar_GunesGozluğu = 80;
            public const int ErkekAksesuar_AtkiEldivenSal = 81;
            public const int ErkekAksesuar_Kemer = 82;
            public const int ErkekAksesuar_PapyonKusak = 83;

            #endregion ErkekAksesuar

            #region KadinAksesuar

            public const int KadinAksesuar = 15;
            // children
            public const int KadinAksesuar_Cuzdan = 98;
            public const int KadinAksesuar_corap = 99;
            public const int KadinAksesuar_Bileklik = 100;
            public const int KadinAksesuar_Saat = 101;
            public const int KadinAksesuar_Parfum = 103;
            public const int KadinAksesuar_canta = 104;
            public const int KadinAksesuar_BereSapka = 105;
            public const int KadinAksesuar_GunesGozlugu = 106;
            public const int KadinAksesuar_AtkiEldivenSal = 107;

            #endregion KadinAksesuar

            #region KadinGiyim

            public const int KadinGiyim = 16;
            // children
            public const int KadinGiyim_SortCapri = 31;
            public const int KadinGiyim_Gömlek = 35;
            public const int KadinGiyim_HirkaKazak = 36;
            public const int KadinGiyim_CeketYelek = 37;
            public const int KadinGiyim_MontKaban = 38;
            public const int KadinGiyim_Pantolon = 39;
            public const int KadinGiyim_TshirtSweatshirt = 40;
            public const int KadinGiyim_Jean = 41;
            public const int KadinGiyim_Elbise = 42;
            public const int KadinGiyim_Bluz = 43;
            public const int KadinGiyim_Tunik = 44;
            public const int KadinGiyim_HamileGiyim = 45;
            public const int KadinGiyim_İcGiyimPijama = 46;
            public const int KadinGiyim_Etek = 47;

            #endregion KadinGiyim

            #region EvYasam

            public const int EvYasam = 17;
            //EvYasam children
            public const int EvYasam_Havlu = 110;

            #endregion EvYasam

            #region CocukAyakkabi

            public const int CocukAyakkabi = 18;
            // children
            public const int CocukAyakkabi_SporAyakkabi = 111;
            public const int CocukAyakkabi_Terlik = 112;
            public const int CocukAyakkabi_Bot = 113;

            #endregion CocukAyakkabi

        }

        /// <summary>
        /// Brand ids in db
        /// </summary>
        public static class BrandId
        {
            /// <summary>
            /// use this id for unknown brands
            /// </summary>
            public const int Unknown = 7;
        }

    }

    #endregion database Ids
}