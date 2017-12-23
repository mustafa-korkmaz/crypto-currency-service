using AutoMapper;
using MoneyMarket.Dto;
using Setting = MoneyMarket.DataAccess.Models.Setting;

namespace MoneyMarket.Business
{
    public static class MappingConfigurator
    {
        public static MapperConfiguration Config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<DataAccess.Models.ApplicationUser, ApplicationUser>();
            cfg.CreateMap<ApplicationUser, DataAccess.Models.ApplicationUser>();

            cfg.CreateMap<DataAccess.Models.RequestLog, RequestLog>();
            cfg.CreateMap<RequestLog, DataAccess.Models.RequestLog>();

            cfg.CreateMap<DataAccess.Models.Setting, Dto.Setting>();
            cfg.CreateMap<Dto.Setting, DataAccess.Models.Setting>();

        });

        public static IMapper Mapper = Config.CreateMapper();
    }
}
