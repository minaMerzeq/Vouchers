﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Vouchers.Data;

namespace Vouchers.Dtos
{
    public class VoucherCreateDto
    {
        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public int CostPoints { get; set; }

        public string Image { get; set; }

        [Required]
        public VoucherType Type { get; set; }

        [Required]
        public VoucherCriteria Criteria { get; set; }

        [Required]
        public int AvailableNumberOfTimes { get; set; }

        [Required]
        public int MerchantId { get; set; }
    }
}
