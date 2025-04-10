using AutoMapper;
using Matrix.Api.Responses;
using Matrix.Core.Models;

namespace Matrix.Api.Mappers;

public class BikeApiMapper : Profile
{
    public BikeApiMapper()
    {
        CreateMap<BikeModel, BikeResponse>();
    }
}