using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCMSv2025.Model
{
    public class DoctorSpecialization
    {
        public int DoctorSpecializationId { get; set; }
        public int DoctorId { get; set; }
        public int SpecializationId { get; set; }
    }
}