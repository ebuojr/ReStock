using FluentValidation;
using ReStockApi.Models;

namespace ReStockApi.Validation
{
    public class InventoryThresholdValidator : AbstractValidator<InventoryThreshold>
    {
        public InventoryThresholdValidator() 
        {
            RuleFor(x => x.StoreNo)
                .NotNull()
                .NotEmpty()
                .WithMessage("StoreNo is required");
            RuleFor(x => x.ItemNo)
                .NotNull()
                .NotEmpty()
                .WithMessage("ItemNo is required");
            RuleFor(x => x.MinimumQuantity)
                .NotNull()
                .InclusiveBetween(0, 30)
                .WithMessage("Threshold must be between 0 and 30");
            RuleFor(x => x.TargetQuantity)
                .NotNull()
                .InclusiveBetween(0, 30)
                .WithMessage("Threshold must be between 0 and 30");
            RuleFor(x => x.ReorderQuantity)
                .NotNull()
                .InclusiveBetween(0, 30)
                .WithMessage("Threshold must be between 0 and 30");
        }
    }
}
