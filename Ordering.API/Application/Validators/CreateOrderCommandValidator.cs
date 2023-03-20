namespace Ordering.API.Application.Validators
{
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator(OrderNumberUniquenessChecker checker)
        {
            RuleFor(command => command.Number).NotEmpty();
            RuleFor(command => command.Date).NotEmpty();
            RuleFor(command => command.ProviderId).NotEmpty();

            RuleFor(command => command.OrderItems)
                .Must(oi => oi.Any())
                .WithMessage("No order items found");

            RuleFor(command => command.OrderItems)
                .Must(oi => !oi.Any(i => i.Quantity <= 0))
                .WithMessage("Order item quantity should be greater than zero");

            RuleFor(command => command)
                .Must((c) => checker.IsUniqueForProvider(c.Number!, c.ProviderId))
                .WithMessage("The order number must be unique for the provider");

            RuleFor(command => command)
                .Must((c) => !c.OrderItems.Any(oi => oi.Name == c.Number))
                .WithMessage("The name of the order item must not be equal to the order number");
        }
    }
}