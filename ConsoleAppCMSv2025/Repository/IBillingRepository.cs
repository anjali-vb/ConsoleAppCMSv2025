using ConsoleAppCMSv2025.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCMSv2025.Repository
{
    public interface IBillingRepository
    {
        Task AddBillingAsync(Billing billing);
    }
}
