using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vouchers.Models
{
    public class UserPoints
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Points { get; set; }

        [Required]
        public string UserId { get; set; }

        public IdentityUser User { get; set; }
    }
}
