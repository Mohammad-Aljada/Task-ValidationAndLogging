using FluentValidation;

namespace Task_ValidationAndLogging.Dtos
{
    public class CreateProductDtoValidation : AbstractValidator<CreateProductDtos>
    {
        public CreateProductDtoValidation() {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Error Name is requied and Name length between 5-30")
                .MinimumLength(5).MaximumLength(30);
            RuleFor(x => x.Description).NotEmpty().WithMessage("Error Description is requied and Description length at leasr 10").MinimumLength(10);
            RuleFor(x=>x.Price).NotEmpty().InclusiveBetween(20, 3000).WithMessage("Error Price is requied and Price length between 20-3000");
        }
    }
}
