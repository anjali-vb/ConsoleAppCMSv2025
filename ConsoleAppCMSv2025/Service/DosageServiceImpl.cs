using ConsoleAppCMSv2025.Model;
using ConsoleAppCMSv2025.Repository;
using System.Collections.Generic;

namespace ConsoleAppCMSv2025.Service
{
    internal class DosageServiceImpl : IDosageService
    {
        private readonly IDosageRepository _dosageRepository;

        public DosageServiceImpl(IDosageRepository dosageRepository)
        {
            _dosageRepository = dosageRepository;
        }

        public List<Dosage> GetAllDosages()
        {
            return _dosageRepository.GetAllDosages();
        }

        public Dosage GetDosageById(int dosageId)
        {
            return _dosageRepository.GetDosageById(dosageId);
        }

        public int AddDosage(Dosage dosage)
        {
            return _dosageRepository.AddDosage(dosage);
        }

        public bool UpdateDosage(Dosage dosage)
        {
            return _dosageRepository.UpdateDosage(dosage);
        }

        public bool DeleteDosage(int dosageId)
        {
            return _dosageRepository.DeleteDosage(dosageId);
        }
    }
}
