namespace Ordering.Application.Orders.Commands.DeleteOrder;

public record DeleteOrderCommand(Guid OrderId): ICommand<DeleteOrderResult>;
public record DeleteOrderResult(bool IsSuccess);

public class DeleteOrderCommmandValidator: AbstractValidator<DeleteOrderCommand>
{
    public DeleteOrderCommmandValidator()
    {
        RuleFor(o => o.OrderId).NotEmpty().WithMessage("OrderId is required");
    }
}
