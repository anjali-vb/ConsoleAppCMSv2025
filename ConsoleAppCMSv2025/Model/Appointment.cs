using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCMSv2025.Model
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string PeriodName { get; set; }
        public string ConsultationStatus { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public int UserId { get; set; }
        public bool IsActive { get; set; }
        public string TimeSlot { get; set; }
        public int TokenNumber { get; internal set; }
        public string? PatientName { get; internal set; }
        public string? MMRNo { get; internal set; }
    }
}
