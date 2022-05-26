using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vouchers.Data;

namespace Vouchers.Dtos
{
    public class VoucherReadDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int CostPoints { get; set; }

        public string Image { get; set; }

        public VoucherType Type { get; set; }

        public VoucherCriteria Criteria { get; set; }

        public int AvailableNumberOfTimes { get; set; }

        public string MerchantName { get; set; }
    }
}
