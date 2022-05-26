using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vouchers.Models;

namespace Vouchers.Data.Repos
{
    public interface IMerchantRepo
    {
        IEnumerable<Merchant> GetAllMerchants();
    }
}
