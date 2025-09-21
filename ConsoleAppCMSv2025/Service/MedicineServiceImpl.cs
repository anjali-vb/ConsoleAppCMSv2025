using ConsoleAppCMSv2025.Model;
using ConsoleAppCMSv2025.Repository;
using System.Collections.Generic;

namespace ConsoleAppCMSv2025.Service
{
    internal class MedicineServiceImpl : IMedicineService
    {
        private readonly IMedicineRepository _medicineRepository;

        public MedicineServiceImpl(IMedicineRepository medicineRepository)
        {
            _medicineRepository = medicineRepository;
        }

        public List<Medicine> GetAllMedicines()
        {
            return _medicineRepository.GetAllMedicines();
        }
    }
}
