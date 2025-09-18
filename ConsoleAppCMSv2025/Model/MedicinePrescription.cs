using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCMSv2025.Model
{
    public class MedicinePrescription
    {
        public int MedicinePrescriptionId { get; set; }
        public int MedicineId { get; set; }
        public int DosageId { get; set; }
        public string Quantity { get; set; }
        public string Duration { get; set; }
        public int AppointmentId { get; set; }
        public bool IsActive { get; set; }
    }
}