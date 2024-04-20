﻿using AutoMapper;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Restaurants.Dtos;

public class RestaurantsProfile : Profile
{
    public RestaurantsProfile()
    {
        CreateMap<Restaurant, RestaurantDto>()
            .ForMember(a => a.City, opt => 
                opt.MapFrom(src => src.Address == null ? null : src.Address.City))
            .ForMember(a => a.PostalCode, opt => 
                opt.MapFrom(src => src.Address == null ? null : src.Address.PostalCode))
            .ForMember(a => a.Street, opt => 
                opt.MapFrom(src => src.Address == null ? null : src.Address.Street))
            .ForMember(d => d.Dishes, opt => 
                opt.MapFrom(src => src.Dishes));
    }
}