using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCMSv2025.Model
{
    public class Dosage
    {
        public int DosageId { get; set; }
        public string DosageName { get; set; }
        public bool IsActive { get; set; }
    }
}