using ConsoleAppCMSv2025.Model;
using ConsoleAppCMSv2025.Repository;
using System.Collections.Generic;

namespace ConsoleAppCMSv2025.Service
{
    internal class MedicinePrescriptionServiceImpl : IMedicinePrescriptionService
    {
        private readonly IMedicinePrescriptionRepository _medicinePrescriptionRepository;

        public MedicinePrescriptionServiceImpl(IMedicinePrescriptionRepository medicinePrescriptionRepository)
        {
            _medicinePrescriptionRepository = medicinePrescriptionRepository;
        }

        public int AddMedicinePrescription(MedicinePrescription prescription)
        {
            return _medicinePrescriptionRepository.AddMedicinePrescription(prescription);
        }

        public List<MedicinePrescription> GetMedicinePrescriptionsByAppointmentId(int appointmentId)
        {
            return _medicinePrescriptionRepository.GetMedicinePrescriptionsByAppointmentId(appointmentId);
        }

        public MedicinePrescription GetMedicinePrescriptionById(int medicinePrescriptionId)
        {
            return _medicinePrescriptionRepository.GetMedicinePrescriptionById(medicinePrescriptionId);
        }

        public bool UpdateMedicinePrescription(MedicinePrescription prescription)
        {
            return _medicinePrescriptionRepository.UpdateMedicinePrescription(prescription);
        }

        public bool DeleteMedicinePrescription(int medicinePrescriptionId)
        {
            return _medicinePrescriptionRepository.DeleteMedicinePrescription(medicinePrescriptionId);
        }
    }
}
