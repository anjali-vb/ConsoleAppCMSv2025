using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleAppCMSv2025.Model;

namespace ConsoleAppCMSv2025.Service
{
    internal interface IMedicineService
    {
        List<Medicine> GetAllMedicines();
    }
}
