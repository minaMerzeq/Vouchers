using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vouchers.Data.Repos;
using Vouchers.Dtos;
using Vouchers.Models;

namespace Vouchers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VouchersController : ControllerBase
    {
        private readonly IVoucherRepo _repo;
        private readonly IMapper _mapper;

        public VouchersController(IVoucherRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<VoucherReadDto>> GetAllVouchers()
        {
            var vouchers = _repo.GetAllVouchers();

            return Ok(_mapper.Map<IEnumerable<VoucherReadDto>>(vouchers));
        }

        [HttpGet("Search")]
        [Authorize(Roles = "Admin")]
        public ActionResult<IEnumerable<VoucherReadDto>> SearchForVouchersByTitleOrDescription(string search)
        {
            var vouchers = _repo.GetAllVouchers().Where(v => v.Title.Contains(search) || v.Description.Contains(search));

            return Ok(_mapper.Map<IEnumerable<VoucherReadDto>>(vouchers));
        }

        [HttpGet("Filter")]
        [Authorize(Roles = "Admin")]
        public ActionResult<IEnumerable<VoucherReadDto>> FilterVouchersByTypeOrMerchant(int? type, int? merchantId)
        {
            var vouchers = _repo.GetAllVouchers();

            if (type != null)
            {
                vouchers = vouchers.Where(v => (int)v.Type == type);
            }

            if (merchantId != null)
            {
                vouchers = vouchers.Where(v => v.MerchantId == merchantId);
            }

            return Ok(_mapper.Map<IEnumerable<VoucherReadDto>>(vouchers));
        }

        [HttpGet("{id}", Name = "GetVoucherById")]
        public ActionResult<IEnumerable<VoucherReadDto>> GetVoucherById(int id)
        {
            var voucher = _repo.GetVoucherById(id);

            if (voucher != null)
            {
                return Ok(_mapper.Map<VoucherReadDto>(voucher));
            }

            return NotFound();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult UpdateVoucher(int id, VoucherCreateDto voucherCreateDto)
        {
            if (voucherCreateDto == null)
            {
                return BadRequest();
            }

            var voucher = _mapper.Map<Voucher>(voucherCreateDto);
            voucher.Id = id;

            _repo.UpdateVoucher(voucher);
            _repo.SaveChanges();
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteVoucher(int id)
        {
            _repo.DeleteVoucher(id);
            if (_repo.SaveChanges())
            {
                return Ok();
            }
                
            return NotFound();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<VoucherReadDto> CreateVoucher(VoucherCreateDto voucherCreateDto)
        {
            if (voucherCreateDto == null)
            {
                return BadRequest();
            }
            var voucher = _mapper.Map<Voucher>(voucherCreateDto);

            _repo.CreateVoucher(voucher);
            _repo.SaveChanges();

            var createdVoucher = _repo.GetVoucherById(voucher.Id);
            return CreatedAtRoute(nameof(GetVoucherById), new { Id = createdVoucher.Id }, _mapper.Map<VoucherReadDto>(createdVoucher));
        }
    }
}
