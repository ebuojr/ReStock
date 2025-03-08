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
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.Country).NotEmpty().WithMessage("Country is required");
            RuleFor(x => x.Address).NotEmpty().WithMessage("Address is required");
        }
    }
}
