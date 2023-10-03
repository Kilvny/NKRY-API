using AutoMapper;
using NKRY_API.Domain.Entities;
using NKRY_API.Models;

namespace NKRY_API.Profiles
{
    public class InvoicesProfile : Profile
    {
        public InvoicesProfile()
        {
            CreateMap<Invoice, TaxInvoiceDto>()
                .ForMember(
                dest => dest.CustomerName,
                opt => opt.MapFrom(src => src.BillTo))
                .ForMember(
                dest => dest.Items,
                opt => opt.MapFrom(src => src.Order.Items))
                .ForMember(
                dest => dest.Description,
                opt => opt.MapFrom(src => src.Order.Description))
                .ForMember(
                dest => dest.Quantity,
                opt => opt.MapFrom(src => src.Order.Quantity))
                .ForMember(
                dest => dest.Price,
                opt => opt.MapFrom(src => src.Order.Price));

                
        }
    }
}
