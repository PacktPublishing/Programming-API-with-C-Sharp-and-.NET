using Cars.Data.DTOs;
using FluentValidation;

namespace Cars.Validators
{
    public class CarDtoValidator : AbstractValidator<CarDto>
    {
        public CarDtoValidator()
        {
            //  RuleFor(x => x.Is_Deleted).Equal("0").WithMessage("Car must not be deleted");
            //RuleFor(x => x.Is_Deleted).NotEmpty().Equal("0").WithMessage("Car must not be deleted");
            //RuleFor(car => car.Is_Deleted).Must(isDeleted => isDeleted == "0").WithMessage("Car must have value zero");
            //RuleFor(x => x.Is_Deleted).Cascade(CascadeMode.Stop).NotEmpty().Equal("0").WithMessage("Car must not be deleted");
            RuleFor(car => car.Is_Deleted).Must(isDeleted => isDeleted == "0").WithSeverity(Severity.Warning).WithMessage("Car must have value zero");
        }
    }
}
