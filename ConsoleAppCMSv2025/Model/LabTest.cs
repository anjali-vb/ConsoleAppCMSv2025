using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCMSv2025.Model
{
    public class LabTest
    {
        public int LabTestId { get; set; }
        public string TestName { get; set; }
        public decimal Amount { get; set; }
        public string MinRange { get; set; }
        public string MaxRange { get; set; }
        public string SampleRequired { get; set; }
        public int LabTestCategoryId { get; set; }
        public bool IsActive { get; set; }
    }
}