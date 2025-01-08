using AutoMapper;

namespace Automapper.Mapping;

public static class MapperFactory
{
    public static IMapper CreateMapper()
    {
        var config = new MapperConfiguration(cfg => { cfg.AddProfile<MyProfile>(); });
#if DEBUG
        config.AssertConfigurationIsValid();
#endif
        return config.CreateMapper();
    }
}