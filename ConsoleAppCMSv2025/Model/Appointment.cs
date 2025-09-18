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
        public string AppointmentTimeSlot { get; set; }
        public int TokenNumber { get; set; }
        public string ConsultationStatus { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public int UserId { get; set; }
        public bool IsActive { get; set; }
        public string TimeSlot { get; internal set; }
    }
}