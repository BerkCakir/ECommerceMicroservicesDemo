using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models.ProductCatalog;

namespace Web.Validators
{
    public class ProductCreateInputValidator:AbstractValidator<ProductCreateInput>
    {
        public ProductCreateInputValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name cannot be empty");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description cannot be empty");
            RuleFor(x => x.ProductInventory.StockAmount).InclusiveBetween(1, int.MaxValue).WithMessage("Stock amount cannot be empty");
            RuleFor(x => x.Price).NotEmpty().WithMessage("Price cannot be empty").ScalePrecision(2, 12).WithMessage("Wrong format");
            RuleFor(x => x.ProductCategoryId).NotEmpty().WithMessage("Category must be selected");
        }
    }
}
