using AutoMapper;
using ProductApi.Core.Dtos.Endpoints.CreateProduct;
using ProductApi.Core.Dtos.Endpoints.GetProductByCode;
using ProductApi.Core.Dtos.Endpoints.GetProducts;
using ProductApi.Core.Dtos.Endpoints.UpdateProduct;
using ProductApi.Core.Entities;

namespace ProductApi.Application
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateProductRequest, Product>();
            CreateMap<Product, CreateProductResponse>();

            CreateMap<Product, GetProductByCodeResponse>();
            CreateMap<Product, GetProductsResponse>();

            CreateMap<UpdateProductRequest, Product>();
        }
    }
}