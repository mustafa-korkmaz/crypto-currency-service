namespace MoneyMarket.Common
{
    public enum AuthenticationMode
    {
        Network = 0,
        Application = 1,
        Both = 2,
        None = 3,
        BothSecure = 4
    }

    public enum ColumnDataFormat
    {
        Default,
        Date,
        Money
    }

    public enum ColumnDataType
    {
        Text,
        LinkButton,
        ImageButton,
    }

    public enum DataSearchType
    {
        Text,
        Dropdown,
        DatePicker,
        DateRangePicker
    }

    public enum Status : byte
    {
        Active,
        Passive,
        Tracking,
        Untracking,
        Suspended,
        WaitingForUpdate,
        GotErrorWhileUpdating,
        RemovedFromWebSite,
        Deleted
    }

    public enum JobExecutionStatus : byte
    {
        Failed,
        Successed,
        Working
    }

    /// <summary>
    /// product modification type
    /// </summary>
    public enum ModificationType : byte
    {
        /// <summary>
        /// product price increased
        /// </summary>
        PriceIncreased = 0,
        /// <summary>
        /// product price decreased
        /// </summary>
        PriceDecreased,
        /// <summary>
        /// product quantity increased
        /// </summary>
        QuantityIncreased,
        /// <summary>
        /// product quantity decreased
        /// </summary>
        QuantityDecreased,
        /// <summary>
        ///product info updated
        /// </summary>
        PropertiesUpdated,
        /// <summary>
        /// product status info updated
        /// </summary>
        StatusUpdated,
        /// <summary>
        /// first time saving
        /// </summary>
        Initialization
    }

    public enum Key
    {
        Tag,
        Discount,
        Image,
        Video
    }

    public enum ResponseCode
    {
        Fail = -1,
        Success = 0,
        Warning = 1,
        Info = 2,
        NoEffect = 3,
        DuplicateRecord = 4,
        NoContent,
        Unauthorized
    }

    public enum WebMethodType
    {
        Null,
        Get,
        Post,
        Put,
        Info,
        NoEffect
    }

    public enum ContentType
    {
        Null,
        FormUrlencoded
    }

    public enum AccountType : byte
    {
        /// <summary>
        /// trial installation
        /// </summary>
        Trial,
        /// <summary>
        /// standart team 
        /// </summary>
        Standart,
        /// <summary>
        /// full authorized team
        /// </summary>
        Premium,
        /// <summary>
        /// account expired status
        /// </summary>
        Suspended
    }

    /// <summary>
    /// HasError: 0
    /// Deleted: 1
    /// WaitingForSendingNotification: 2
    /// NotificationSent: 3
    /// NotificationRead: 4
    /// </summary>
    public enum NotificationStatus : byte
    {
        HasError,
        Deleted,
        WaitingForSendingNotification,
        NotificationSent,
        NotificationRead,
    }

    public enum NotificationType : byte
    {
        Product,
        General
    }

    public enum SearchType
    {
        Authors = 1,
        Events = 2
    }

    public enum DeviceType : byte
    {
        Ios = 0,
        Android = 1
    }

    public enum RequestType : byte
    {
        Complaint = 0,
        Suggession = 1,
        /// <summary>
        /// wish to track request for non-tracked websites
        /// </summary>
        Wish = 2,
        Other = 3,
        None = 4
    }

    public enum Currency : byte
    {
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
        //etc..
    }

    public enum Provider : byte
    {
        Unknown = 0,
        BtcTurk,
        BitStamp,
        CoinMarketCap
    }

    public enum Language : byte
    {
        Turkish = 1,
        English = 2
    }

}