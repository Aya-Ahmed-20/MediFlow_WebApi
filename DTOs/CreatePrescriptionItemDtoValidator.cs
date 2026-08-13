using FluentValidation;

namespace MediFlowApi.DTOs
{
    public class CreatePrescriptionItemDtoValidator: AbstractValidator<CreatePrescriptionItemDto>
    {
        public CreatePrescriptionItemDtoValidator()
        {
            RuleFor(x=>x.MedicineId).NotEmpty().GreaterThan(0);
            RuleFor(x=>x.Dose ).NotEmpty().MaximumLength(500);
            RuleFor(x=>x.Instructions).MaximumLength(500);
        }
    }
}
