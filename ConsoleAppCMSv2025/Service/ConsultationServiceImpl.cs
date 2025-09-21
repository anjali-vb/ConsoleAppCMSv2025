using ConsoleAppCMSv2025.Model;
using ConsoleAppCMSv2025.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCMSv2025.Service
{
    public class ConsultationServiceImpl : IConsultationService
    {
        private readonly IConsultationRepository _consultationRepository;

        public ConsultationServiceImpl(IConsultationRepository consultationRepository)
        {
            _consultationRepository = consultationRepository;
        }

        public async Task AddConsultationAsync(Consultation consultation)
        {
            await _consultationRepository.AddConsultationAsync(consultation);
        }
    }
}