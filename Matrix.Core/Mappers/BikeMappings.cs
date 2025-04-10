using AutoMapper;
using Matrix.Core.Models;
using Matrix.Entities.Models;

namespace Matrix.Core.Mappers;

public class BikeMappings : Profile
{
    public BikeMappings()
    {
        CreateMap<Bike, BikeModel>();
    }
}