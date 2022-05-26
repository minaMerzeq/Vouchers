using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vouchers.Models;

namespace Vouchers.Data.Repos
{
    public class VoucherRepo : IVoucherRepo
    {
        private readonly AppDbContext _context;

        public VoucherRepo(AppDbContext context)
        {
            _context = context;
        }

        public void CreateVoucher(Voucher voucher)
        {
            CheckArgumentValidation(voucher);

            _context.Vouchers.Add(voucher);
        }

        public void DeleteVoucher(int id)
        {
            var voucher = _context.Vouchers.FirstOrDefault(v => v.Id == id);
            if (voucher == null)
            {
                return;
            }

            _context.Vouchers.Remove(voucher);
        }

        public IEnumerable<Voucher> GetAllVouchers()
        {
            return _context.Vouchers.Include(v => v.Merchant).ToList();
        }

        public Voucher GetVoucherById(int id)
        {
            return _context.Vouchers.Include(v => v.Merchant).FirstOrDefault(v => v.Id == id);
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() >= 0;
        }

        public void UpdateVoucher(Voucher voucher)
        {
            CheckArgumentValidation(voucher);

            _context.Update(voucher);
        }

        private void CheckArgumentValidation(Voucher voucher)
        {
            if (voucher == null)
            {
                throw new ArgumentNullException(nameof(voucher));
            }
        }
    }
}
