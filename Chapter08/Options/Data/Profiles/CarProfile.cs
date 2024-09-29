using AutoMapper;
using Cars.Data.DTOs;
using Cars.Data.Entities;

namespace Cars.Data.Profiles
{
    public class CarProfile : Profile
    {
        public CarProfile()
        {
            CreateMap<CarDto, Car>()
                .ForMember(car => car.id, opt => opt.MapFrom(carDto => carDto.Id))
                .ForMember(car => car.name, opt => opt.MapFrom(carDto => carDto.Name))
                .ForMember(car => car.mpg, opt => opt.MapFrom(carDto => carDto.Mpg))
                .ForMember(car => car.cylinders, opt => opt.MapFrom(carDto => carDto.Cylinders))
                .ForMember(car => car.displacement, opt => opt.MapFrom(carDto => carDto.Displacement))
                .ForMember(car => car.horsepower, opt => opt.MapFrom(carDto => carDto.Horsepower))
                .ForMember(car => car.weight, opt => opt.MapFrom(carDto => carDto.Weight))
                .ForMember(car => car.acceleration, opt => opt.MapFrom(carDto => carDto.Acceleration))
                .ForMember(car => car.model_year, opt => opt.MapFrom(carDto => carDto.ModelYear))
                .ForMember(car => car.origin, opt => opt.MapFrom(carDto => carDto.Origin))
                .ReverseMap();

            CreateMap<List<CarFlat>, CarDto>()
                .ForPath(dest => dest.Id, opt => opt.MapFrom(src => src.First().id))
                .ForPath(dest => dest.Name, opt => opt.MapFrom(src => src.First().name))
                .ForMember(dest => dest.Options, opt => opt.MapFrom(src => src));

            CreateMap<OptionsDto, CarFlat>()
                .ForMember(dest => dest.option_id, opt => opt.MapFrom(src => src.OptionId))
                .ForMember(dest => dest.option_name, opt => opt.MapFrom(src => src.OptionName))
                .ForMember(dest => dest.option_price, opt => opt.MapFrom(src => src.OptionPrice))
                .ReverseMap();
        }
    }
}
