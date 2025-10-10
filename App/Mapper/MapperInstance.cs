using AutoMapper;

namespace App.Mapper
{
    public class MapperInstance
    {
        public static MapperConfiguration Configuration => new MapperConfiguration(cfg => {
            cfg.AddProfile<MapperProfile>();
        }, new LoggerFactory());

        public static IMapper GetInstance()
        {
            return Configuration.CreateMapper();
        }
    }
}
