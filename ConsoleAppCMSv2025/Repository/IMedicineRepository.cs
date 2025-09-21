using ConsoleAppCMSv2025.Model;
using System.Collections.Generic;

namespace ConsoleAppCMSv2025.Repository
{
    internal interface IMedicineRepository
    {
        List<Medicine> GetAllMedicines();
        Medicine GetMedicineById(int medicineId);
        int AddMedicine(Medicine medicine);
        bool UpdateMedicine(Medicine medicine);
        bool DeleteMedicine(int medicineId);
    }
}
