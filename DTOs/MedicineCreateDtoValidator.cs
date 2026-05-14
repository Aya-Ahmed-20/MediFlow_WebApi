using FluentValidation;

namespace MediFlowApi.DTOs
{
    public class MedicineCreateDtoValidator : AbstractValidator<MedicineCreateDto>
    {
        public MedicineCreateDtoValidator()
        { 
            RuleFor(x => x.Name).NotEmpty().MinimumLength(3).WithMessage("Name is too short");
            RuleFor(x => x.Price).GreaterThan(0).PrecisionScale(precision: 18, scale: 2, ignoreTrailingZeros: false);

        }
    }
}
