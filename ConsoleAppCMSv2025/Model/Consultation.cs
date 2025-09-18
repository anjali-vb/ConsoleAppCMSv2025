using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCMSv2025.Model
{
    public class Consultation
    {
        public int ConsultationId { get; set; }
        public string Symptoms { get; set; }
        public string Diagnosis { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedDate { get; set; }
        public int AppointmentId { get; set; }
        public bool IsActive { get; set; }
    }
}