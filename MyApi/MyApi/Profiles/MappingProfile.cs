using System.Collections.Generic;
using AutoMapper;
using Dto;
using Entities;
using Services;

namespace MyApi.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, CreateAddressDto>();
            CreateMap<CreateAddressDto, Product>();
            CreateMap<Product, UpdateProductDto>();
            CreateMap<UpdateProductDto, Product>();
            CreateMap<Product, ProductResponseDto>();

            CreateMap<Order, CreateOrderDto>();
            CreateMap<CreateOrderDto, Order>();
            CreateMap<Order, OrderResponseDto>();
            CreateMap<OrderResponseDto, Order>();
        }
    }
}