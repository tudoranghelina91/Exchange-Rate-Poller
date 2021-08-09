using AutoMapper;
using ExchangeRatePoller.DataAccess.Features.BnrExchangeRate.Dto;
using ExchangeRatePoller.Domain.Features.BnrExchangeRate.Models;

namespace ExchangeRatePoller.DataAccess
{
    public class ExchangeRateMappingProfile : Profile
    {
        public ExchangeRateMappingProfile()
        {
            CreateMap<Domain.Features.BnrExchangeRate.Dto.Cube, Cube>();
            CreateMap<Cube, CubeDto>();

            CreateMap<Domain.Features.BnrExchangeRate.Dto.Rate, Rate>()
                .ForPath(dest => dest.Currency.Code, opt => opt.MapFrom(src => src.Currency))
                .ForPath(dest => dest.Currency.Multiplier, opt => opt.MapFrom(src => src.Multiplier));

            CreateMap<Rate, RateDto>();

            CreateMap<Currency, CurrencyDto>();
        }
    }
}
