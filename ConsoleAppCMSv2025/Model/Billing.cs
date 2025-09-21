using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCMSv2025.Model

{
    public class Billing
    {
        public int BillId { get; set; }
        public int AppointmentId { get; set; }
        public decimal ConsultationFee { get; set; }
        public DateTime BillDate { get; set; }
        public bool IsPaid { get; set; }
    }
}