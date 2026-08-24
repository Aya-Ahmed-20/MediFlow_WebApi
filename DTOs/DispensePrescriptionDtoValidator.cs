using FluentValidation;

namespace MediFlowApi.DTOs
{
    public class DispensePrescriptionDtoValidator :AbstractValidator<DispensePrescriptionDto>
    {
          public DispensePrescriptionDtoValidator() 
        { 
            RuleFor(x=>x.PrescriptionId).GreaterThan(0);
            RuleFor(x=>x.Notes).MaximumLength(500);
        }
    }
}
