using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vouchers.Models;

namespace Vouchers.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<Merchant> Merchants { get; set; }
        public DbSet<UserPoints> UserPoints { get; set; }
        public DbSet<UserVouchers> UserVouchers { get; set; }
    }
}
