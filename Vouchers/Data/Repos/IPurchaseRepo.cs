using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vouchers.Models;

namespace Vouchers.Data.Repos
{
    public interface IPurchaseRepo
    {
        UserPoints GetUserPoints(string userId);
        void AddUserPoints(UserPoints userPoints);
        IEnumerable<UserVouchers> GetUserVouchers(string userId);
        void CreatePurchase(UserVouchers userVouchers);
    }
}
