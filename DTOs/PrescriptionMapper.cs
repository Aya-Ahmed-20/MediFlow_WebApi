using MediFlowApi.Models;

namespace MediFlowApi.DTOs
{
    public class PrescriptionMapper
    {
        // 1. Map Create DTO -> Entity 
        public Prescription ToEntity(CreatePrescriptionDto dto)
        {
            if (dto == null) return null!;

            return new Prescription
            {
                ConsultationId = dto.ConsultationId,
                DurationInDays = dto.DurationInDays,
                PrescriptionItems = dto.PrescriptionItems?.Select(item => ToPrescriptionItem(item)).ToList() ?? new List<PrescriptionItem>()
            };
        }

        public PrescriptionItem ToPrescriptionItem(CreatePrescriptionItemDto dto)
        {
            if (dto == null) return null!;

            return new PrescriptionItem
            {
                Dose = dto.Dose,
                Instructions = dto.Instructions,
                MedicineId = dto.MedicineId,
            };
        }

        // 2. Map Entity -> Response DTO 
        public PrescriptionResponseDto ToResponseDto(Prescription entity)
        {
            if (entity == null) return null!;

            return new PrescriptionResponseDto
            {
                Id = entity.Id,
                ConsultationId = entity.ConsultationId,
                DurationInDays = entity.DurationInDays,
                PrescriptionItems = entity.PrescriptionItems?.Select(item => new PrescriptionItemDto
                {
                    MedicineId = item.MedicineId,
                    Dose = item.Dose,
                    Instructions = item.Instructions
                }).ToList() ?? new List<PrescriptionItemDto>()
            };
        }
    }
}