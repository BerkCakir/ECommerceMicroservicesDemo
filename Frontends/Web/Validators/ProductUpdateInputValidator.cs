using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models.ProductCatalog;

namespace Web.Validators
{
    public class ProductUpdateInputValidator : AbstractValidator<ProductUpdateInput>
    {
        public ProductUpdateInputValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name cannot be empty");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description cannot be empty");
            RuleFor(x => x.ProductInventory.StockAmount).InclusiveBetween(1, int.MaxValue).WithMessage("Stock amount cannot be empty");
            RuleFor(x => x.Price).NotEmpty().WithMessage("Price cannot be empty").ScalePrecision(2, 12).WithMessage("Wrong format");
        }
    }
}
