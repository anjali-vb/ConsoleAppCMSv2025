using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCMSv2025.Model
{
    public class LabPrescription
    {
        public int LabTestPrescriptionId { get; set; }
        public int LabTestId { get; set; }
        public string LabTestValue { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Remarks { get; set; }
        public int AppointmentId { get; set; }
        public bool IsActive { get; set; }
    }
}