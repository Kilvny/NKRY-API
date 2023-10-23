using AutoMapper;
using NKRY_API.DataAccess.EFCore;
using NKRY_API.Domain.Contracts;
using NKRY_API.Domain.Entities;
using NKRY_API.Models;

namespace NKRY_API.Profiles
{
    public class InvoicesProfile : Profile
    {
        private readonly IUnitOfWork _unitOfWork;

        public InvoicesProfile(IUnitOfWork unitOfWork, ApplicationContext a)
        {
            _unitOfWork = unitOfWork;
        }
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

            CreateMap<Invoice, Invoice>()
                .ForMember(
               dest => dest.Order,
               opt => opt.MapFrom(src => _unitOfWork.Order.GetById((Guid)src.OrderId) ));
                
        }
    }
}
