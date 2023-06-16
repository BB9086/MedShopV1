using AutoMapper;
using MedShopWebAPi.Dtos;
using MedShop.Dtos;
using MedShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedShopWebAPi.Profiles
{
    public class MedShopProfile : Profile
    {
        public MedShopProfile()
        {
            //Source to target ...for reading
            //CreateMap<Order, OrderReadDto>();


            //        CreateMap<Customer, OrderReadDto>()
            //.ForMember(dest => dest.OrderId, opt => opt.Ignore())
            //.ForMember(dest => dest.StoreId, opt => opt.Ignore())
            //.ForMember(dest => dest.OrderStatus, opt => opt.Ignore())
            //.ForMember(dest => dest.ShippingStatus, opt => opt.Ignore())
            //.ForMember(dest => dest.PaymentStatus, opt => opt.Ignore())
            //.ForMember(dest => dest.OrderTotal, opt => opt.Ignore())
            //.ForMember(dest => dest.ShippingMethod, opt => opt.Ignore())
            //.ForMember(dest => dest.CreatedOn, opt => opt.Ignore())
            //.ForMember(dest => dest.ShippingDate, opt => opt.Ignore());

            //        CreateMap<Order, OrderReadDto>()
            //    .ForMember(dest => dest.LoginUserId, opt => opt.Ignore())
            //    .ForMember(dest => dest.FirstName, opt => opt.Ignore())
            //    .ForMember(dest => dest.LastName, opt => opt.Ignore())
            //    .ForMember(dest => dest.Address, opt => opt.Ignore())
            //    .ForMember(dest => dest.ZipCode, opt => opt.Ignore())
            //    .ForMember(dest => dest.City, opt => opt.Ignore())
            //    .ForMember(dest => dest.Country, opt => opt.Ignore())
            //    .ForMember(dest => dest.TelephoneNumber, opt => opt.Ignore())
            //    .ForMember(dest => dest.Email, opt => opt.Ignore())
            //    .ForMember(dest => dest.Orders, opt => opt.Ignore());


            CreateMap<Order, OrderReadDto>();
            CreateMap<Order, OrderUpdateDto>();
            CreateMap<OrderUpdateDto, Order>();
            //Ignore se stavlja da bi zanemarili vrednost tog propertija tj. da bi bio null
            CreateMap<Customer, OrderReadDto>()
                 .ForMember(dest => dest.Orders, opt => opt.Ignore())
                 .ForMember(dest => dest.LoginUserId,opt => opt.Ignore());


            CreateMap<OrderItem, OrderItemReadDto>();
            CreateMap<Product, OrderItemReadDto>()
                .ForMember(dest => dest.Price, opt => opt.Ignore())
                .ForMember(dest => dest.OrderItems, opt => opt.Ignore())
                .ForMember(dest => dest.ImageSource, opt => opt.Ignore());

            CreateMap<Store,StoreReadDto>();

        }
    }
}
