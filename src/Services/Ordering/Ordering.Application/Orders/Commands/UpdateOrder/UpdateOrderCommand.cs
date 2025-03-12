namespace Ordering.Application.Orders.Commands.UpdateOrder;

public record UpdateOrderCommand(OrderDto OrderDto) : ICommand<UpdateOrderResult>;
public record UpdateOrderResult(bool IsSuccess);

public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderCommandValidator()
    {
        RuleFor(o => o.OrderDto.Id).NotEmpty().WithMessage("Id is required");
        RuleFor(o => o.OrderDto.OrderName).NotEmpty().WithMessage("Name is required");
        RuleFor(o => o.OrderDto.CustomerId).NotEmpty().WithMessage("CustomerId is required");

    }
}
