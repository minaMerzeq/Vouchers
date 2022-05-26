using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vouchers.Models;

namespace Vouchers.Data.Repos
{
    public class MerchantRepo : IMerchantRepo
    {
        private readonly AppDbContext _context;

        public MerchantRepo(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Merchant> GetAllMerchants()
        {
            return _context.Merchants.ToList();
        }
    }
}
