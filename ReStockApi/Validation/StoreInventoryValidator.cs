using FluentValidation;
using ReStockApi.Models;

namespace ReStockApi.Validation
{
    public class StoreInventoryValidator : AbstractValidator<StoreInventory>
    {
        public StoreInventoryValidator()
        {
            RuleFor(x => x.StoreNo)
                .NotNull()
                .NotEmpty()
                .WithMessage("StoreNo is required");
            RuleFor(x => x.ItemNo)
                .NotNull()
                .NotEmpty()
                .WithMessage("ItemNo is required");
            RuleFor(x => x.Quantity)
                .NotNull()
                .InclusiveBetween(0, 100)
                .WithMessage("Quantity must be between 0 and 100");
        }
    }
}
