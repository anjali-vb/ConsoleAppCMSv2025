using ConsoleAppCMSv2025.Model;
using System.Collections.Generic;

namespace ConsoleAppCMSv2025.Repository
{
    internal interface IMedicinePrescriptionRepository
    {
        int AddMedicinePrescription(MedicinePrescription prescription);
        List<MedicinePrescription> GetMedicinePrescriptionsByAppointmentId(int appointmentId);
        MedicinePrescription GetMedicinePrescriptionById(int medicinePrescriptionId);
        bool UpdateMedicinePrescription(MedicinePrescription prescription);
        bool DeleteMedicinePrescription(int medicinePrescriptionId);
    }
}
