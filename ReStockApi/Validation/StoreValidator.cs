using FluentValidation;
using ReStockApi.Models;

namespace ReStockApi.Validation
{
    public class StoreValidator : AbstractValidator<Store>
    {
        public StoreValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.No).NotEmpty().WithMessage("Store No is required");
            RuleFor(x => x.No).GreaterThan(5000).WithMessage("Store No must be greater than 5000");
            RuleFor(x => x.No).LessThan(6000).WithMessage("Store No must be less than 6000");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.Country).NotEmpty().WithMessage("Country is required");
            RuleFor(x => x.Address).NotEmpty().WithMessage("Address is required");
        }
    }
}
