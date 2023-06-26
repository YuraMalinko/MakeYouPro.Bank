using AutoMapper;
using MakeYouPro.Bourse.Rates.Bll.ModelsBll;
namespace MakeYouPro.Bourse.Rates.Dal.Models;

public class MapperBll
{
    private readonly MapperConfiguration _configuration;
    private static MapperBll _instanceMapperBll;
    private MapperBll()
    {
        _configuration = new MapperConfiguration(
            cfg =>
            { 
                cfg.CreateMap<ARSBll, ARSDto>();
                cfg.CreateMap<BGNBll, BGNDto>();
                cfg.CreateMap<CNYBll, CNYDto>();
                cfg.CreateMap<EURBll, EURDto>();
                cfg.CreateMap<JPYBll, JPYDto>();
                cfg.CreateMap<RSDBll, RSDDto>();
                cfg.CreateMap<RUBBll, RUBDto>();
                cfg.CreateMap<USDBll, USDDto>();
                cfg.CreateMap<ARSDto, ARSBll>();
                cfg.CreateMap<BGNDto, BGNBll>();
                cfg.CreateMap<CNYDto, CNYBll>();
                cfg.CreateMap<EURDto, EURBll>();
                cfg.CreateMap<JPYDto, JPYBll>();
                cfg.CreateMap<RSDDto, RSDBll>();
                cfg.CreateMap<RUBDto, RUBBll>();
                cfg.CreateMap<USDDto, USDBll>();
            });
    }
    public static MapperBll getInstance()
    {
        if (_instanceMapperBll is null)
        {
            _instanceMapperBll = new MapperBll();
        }
        return _instanceMapperBll;
    }

    public ARSBll MapARSDtoToARSBll(ARSDto arsDto)
    {
        return _configuration.CreateMapper().Map<ARSBll>(arsDto);
    }

    public BGNBll MapBGNDtoToBGNBll(BGNDto bgnDto)
    {
        return _configuration.CreateMapper().Map<BGNBll>(bgnDto);
    }
    public CNYBll MapCNYDtoToCNYBll(CNYDto cnyDto)
    {
        return _configuration.CreateMapper().Map<CNYBll>(cnyDto);
    }
    public EURBll MapEURDtoToEURBll(EURDto eurDto)
    {
        return _configuration.CreateMapper().Map<EURBll>(eurDto);
    }
    public JPYBll MapJPYDtoToJPYBll(JPYDto jpyDto)
    {
        return _configuration.CreateMapper().Map<JPYBll>(jpyDto);
    }
    public RSDBll MapRSDDtoToRSDBll(RSDDto rsdDto)
    {
        return _configuration.CreateMapper().Map<RSDBll>(rsdDto);
    }

    public RUBBll MapRUBDtoToRUBBll(RUBDto rubDto)
    {
        return _configuration.CreateMapper().Map<RUBBll>(rubDto);
    }
    public USDBll MapUSDDtoToUSDBll(USDDto usdDto)
    {
        return _configuration.CreateMapper().Map<USDBll>(usdDto);
    }

    public ARSDto MapARSBllToARSDto(ARSBll arsBll)
    {
        return _configuration.CreateMapper().Map<ARSDto>(arsBll);
    }
    public BGNDto MapBGNBllToBGNDto(BGNBll bgnBll)
    {
        return _configuration.CreateMapper().Map<BGNDto>(bgnBll);
    }
    public CNYDto MapCNYBllToCNYDto(CNYBll cnyBll)
    {
        return _configuration.CreateMapper().Map<CNYDto>(cnyBll);
    }
    public EURDto MapEURBllToEURDto(EURBll eurBll)
    {
        return _configuration.CreateMapper().Map<EURDto>(eurBll);
    }
    public JPYDto MapJPYBllToJPYDto(JPYBll jpyBll)
    {
        return _configuration.CreateMapper().Map<JPYDto>(jpyBll);
    }
    public RSDDto MapRSDBllToRSDDto(RSDBll rsdBll)
    {
        return _configuration.CreateMapper().Map<RSDDto>(rsdBll);
    }
    public RUBDto MapRUBBllToRUBDto(RUBBll rubBll)
    {
        return _configuration.CreateMapper().Map<RUBDto>(rubBll);
    }
    public USDDto MapUSDBllToUSDDto(USDBll usdBll)
    {
        return _configuration.CreateMapper().Map<USDDto>(usdBll);
    }
}
