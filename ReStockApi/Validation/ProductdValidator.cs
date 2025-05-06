using FluentValidation;
using ReStockApi.Models;

namespace ReStockApi.Validation
{
    public class ProductdValidator : AbstractValidator<Product>
    {
        public ProductdValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .WithMessage("Name is required");
            RuleFor(x => x.ItemNo)
                .NotNull()
                .NotEmpty()
                .WithMessage("ItemNo is required");
            RuleFor(x => x.Brand)
                .NotNull()
                .NotEmpty()
                .WithMessage("Brand is required");
            RuleFor(x => x.RetailPrice)
                .NotNull()
                .InclusiveBetween(69, 1200)
                .WithMessage("RetailPrice must be between 69 and 1200");
        }
    }
}
