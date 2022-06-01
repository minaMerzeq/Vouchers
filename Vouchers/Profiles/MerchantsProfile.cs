using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vouchers.Dtos;
using Vouchers.Models;

namespace Vouchers.Profiles
{
    public class MerchantsProfile : Profile
    {
        public MerchantsProfile()
        {
            // source --> target
            CreateMap<Merchant, MerchantReadDto>();
        }
    }
}
