using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCMSv2025.Model
{
    public class Doctor
    {
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public decimal ConsultationFee { get; set; }

        public string Department { get; set; }
        public string PeriodName { get; set; }
        public string TimeSlot { get; set; }
        public int UserId { get; set; }
        public bool IsActive { get; set; }
    }
}