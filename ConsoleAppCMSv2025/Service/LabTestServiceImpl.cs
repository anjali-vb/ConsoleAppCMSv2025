using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleAppCMSv2025.Model;
using ConsoleAppCMSv2025.Repository;

namespace ConsoleAppCMSv2025.Service
{
    
    internal class LabTestServiceImpl : ILabTestService
    {
        private readonly ILabTestRepository _labTestRepository;
        public LabTestServiceImpl(ILabTestRepository labTestRepository)
        {
            _labTestRepository = labTestRepository;
        }
        public int AddLabTest(LabTest labTest)
        {
            return _labTestRepository.AddLabTest(labTest);
        }
    }
}
