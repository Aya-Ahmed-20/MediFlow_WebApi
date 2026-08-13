using FluentValidation;

namespace MediFlowApi.DTOs
{
    public class CreatePrescriptionDtoValidator:AbstractValidator<CreatePrescriptionDto>
    {
        public CreatePrescriptionDtoValidator()
        { 
            RuleFor(x=>x.ConsultationId).GreaterThan(0);
            RuleFor(x => x.DurationInDays).NotEmpty().InclusiveBetween(1, 365);
            RuleFor(x=>x.PrescriptionItems).NotEmpty().ForEach(x=>x. SetValidator(new CreatePrescriptionItemDtoValidator()));

        }
    }
}
