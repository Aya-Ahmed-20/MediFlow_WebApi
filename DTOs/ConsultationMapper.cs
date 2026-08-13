using MediFlowApi.Data;
using MediFlowApi.Models;

namespace MediFlowApi.DTOs
{
    public static class ConsultationMapper
    {
        public static  Consultation ToEntity(CreateConsultationDto dto)
        {
            return new Consultation
            {
                DoctorId=dto.DoctorId,
                PatientId=dto.PatientId,
                Diagnoses=dto.Diagnoses,
                Symptoms = dto.Symptoms,
                ConsultationDate=DateTime.UtcNow
            };
        }
        public static  ReadConsultationDto ToDto(Consultation entity)
        {
            return new ReadConsultationDto
            {
                ConsultationDate = entity.ConsultationDate,
                Diagnoses = entity.Diagnoses,
                Symptoms = entity.Symptoms,
                DoctorName = entity.Doctor.Name,
                PatientName = entity.Patient.Name
            };
        }
    }
}
