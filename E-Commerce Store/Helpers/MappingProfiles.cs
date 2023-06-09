﻿using AutoMapper;
using Core.Entities;
using E_Commerce_Store.Dtos;

namespace E_Commerce_Store.Helpers
{
    public class MappingProfiles: Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(d=>d.ProductBrand, x=>x.MapFrom(s=>s.ProductBrand.Name))
                .ForMember(d => d.ProductType, x => x.MapFrom(s => s.ProductType.Name))
                .ForMember(d=>d.PictureUrl, x=>x.MapFrom<ProductUrlResolver>());
        }
    }
}
