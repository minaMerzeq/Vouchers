using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vouchers.Data
{
    public enum VoucherType
    {
        Discount,
        Gift,
    }

    public enum VoucherCriteria
    {
        Basic,
        Silver,
        Gold,
        Platinum,
    }
}
