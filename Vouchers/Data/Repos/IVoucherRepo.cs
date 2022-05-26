using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vouchers.Models;

namespace Vouchers.Data.Repos
{
    public interface IVoucherRepo
    {
        bool SaveChanges();
        IEnumerable<Voucher> GetAllVouchers();
        Voucher GetVoucherById(int id);
        void CreateVoucher(Voucher voucher);
        void DeleteVoucher(int id);
        void UpdateVoucher(Voucher voucher);
    }
}
