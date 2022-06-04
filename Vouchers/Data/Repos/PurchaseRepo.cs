using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vouchers.Models;

namespace Vouchers.Data.Repos
{
    public class PurchaseRepo : IPurchaseRepo
    {
        private readonly AppDbContext _context;

        public PurchaseRepo(AppDbContext context)
        {
            _context = context;
        }

        public void CreatePurchase(UserVouchers userVouchers)
        {
            _context.UserVouchers.Add(userVouchers);
            _context.SaveChanges();

            var costPoints = _context.Vouchers.FirstOrDefault(v => v.Id == userVouchers.VoucherId).CostPoints;
            var userPoints = _context.UserPoints.FirstOrDefault(up => up.UserId == userVouchers.UserId);

            userPoints.Points -= costPoints;
            _context.SaveChanges();
        }

        public UserPoints GetUserPoints(string userId)
        {
            return _context.UserPoints.FirstOrDefault(p => p.UserId == userId);
        }

        public IEnumerable<UserVouchers> GetUserVouchers(string userId)
        {
            return _context.UserVouchers.Include(uv => uv.Voucher).ThenInclude(v => v.Merchant).Where(uv => uv.UserId == userId);
        }

        void IPurchaseRepo.AddUserPoints(UserPoints userPoints)
        {
            _context.UserPoints.Add(userPoints);
            _context.SaveChanges();
        }
    }
}
