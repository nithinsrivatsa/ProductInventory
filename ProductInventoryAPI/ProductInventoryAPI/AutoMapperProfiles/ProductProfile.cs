// <copyright file="ProductProfile.cs" company="Carl Zeiss">
//   Copyright 2025 Carl Zeiss. All rights reserved
// </copyright>

namespace ProductInventoryAPI.AutoMapperProfiles
{
    using AutoMapper;
    using ProductInventoryAPI.Dtos;
    using ProductInventoryAPI.Repositories;

    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductDto, Product>().ReverseMap();
            CreateMap<ProductUpsertDto, Product>().ReverseMap();
        }
    }
}
