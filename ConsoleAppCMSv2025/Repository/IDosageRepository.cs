using ConsoleAppCMSv2025.Model;
using System.Collections.Generic;

namespace ConsoleAppCMSv2025.Repository
{
    internal interface IDosageRepository
    {
        List<Dosage> GetAllDosages();
        Dosage GetDosageById(int dosageId);
        int AddDosage(Dosage dosage);
        bool UpdateDosage(Dosage dosage);
        bool DeleteDosage(int dosageId);
    }
}
