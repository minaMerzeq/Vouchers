using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vouchers.Data;
using Vouchers.Dtos;
using Vouchers.Models;

namespace Vouchers.Profiles
{
    public class VouchersProfile : Profile
    {
        public VouchersProfile()
        {
            // source --> target
            CreateMap<Voucher, VoucherReadDto>()
                .ForMember(dest => dest.MerchantName, opt => opt.MapFrom(src => src.Merchant.Name))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => Enum.GetName(typeof(VoucherType), src.Type)))
                .ForMember(dest => dest.Criteria, opt => opt.MapFrom(src => Enum.GetName(typeof(VoucherCriteria), src.Criteria)));

            CreateMap<VoucherCreateDto, Voucher>();
        }
    }
}
