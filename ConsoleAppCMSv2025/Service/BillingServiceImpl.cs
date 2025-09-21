using ConsoleAppCMSv2025.Model;
using ConsoleAppCMSv2025.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCMSv2025.Service
{
    public class BillingServiceImpl : IBillingService
    {
        private readonly IBillingRepository _billingRepository;

        public BillingServiceImpl(IBillingRepository billingRepository)
        {
            _billingRepository = billingRepository;
        }

        public async Task AddBillingAsync(Billing billing)
        {
            await _billingRepository.AddBillingAsync(billing);
        }
    }
}