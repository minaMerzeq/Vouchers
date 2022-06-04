using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Vouchers.Data.Repos;
using Vouchers.Dtos;
using Vouchers.Models;

namespace Vouchers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PurchaseController : ControllerBase
    {
        private readonly IPurchaseRepo _repo;
        private readonly IMapper _mapper;

        public PurchaseController(IPurchaseRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet("UserAllPoints")]
        public ActionResult<AllPointsReadDto> GetAllPoints()
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            var userPoints = _repo.GetUserPoints(userId);
            var spentPoints = _repo.GetUserVouchers(userId).Select(uv => uv.Voucher).Sum(v => v.CostPoints);
            var allPoints = new AllPointsReadDto { AvailablePoints = userPoints.Points, SpentPoints = spentPoints };

            return Ok(allPoints);
        }

        [HttpGet("UserVouchers")]
        public ActionResult<IEnumerable<VoucherReadDto>> GetUserVouchers()
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            var userVouchers = _repo.GetUserVouchers(userId).Select(uv => uv.Voucher);

            return Ok(_mapper.Map<IEnumerable<VoucherReadDto>>(userVouchers));
        }

        [HttpPost]
        public ActionResult CreatePurchase(int voucherId)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            var userVouchers = new UserVouchers { UserId = userId, VoucherId = voucherId };
            _repo.CreatePurchase(userVouchers);

            return Ok(new { success = true, message = "successful purchase..." });
        }
    }
}
