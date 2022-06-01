using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vouchers.Data.Repos;
using Vouchers.Dtos;

namespace Vouchers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MerchantsController : ControllerBase
    {
        private readonly IMerchantRepo _repo;
        private readonly IMapper _mapper;

        public MerchantsController(IMerchantRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<MerchantReadDto>> GetAllMerchants()
        {
            var merchants = _repo.GetAllMerchants();

            return Ok(_mapper.Map<IEnumerable<MerchantReadDto>>(merchants));
        }
    }
}
