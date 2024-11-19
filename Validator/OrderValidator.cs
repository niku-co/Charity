using FluentValidation;
using NikuAPI.Entities;

namespace NikuAPI.Validator;

public class OrderValidator : AbstractValidator<Order>
{
    public OrderValidator()
    {
        RuleFor(o => o.Reserved).Equal(false).WithMessage("سفارش رزرو شده است!");
        RuleFor(o => o.PayBill).Equal(true).WithMessage("سفارش پرداخت نشده است!");
        RuleFor(o => o.Cancel).Equal(false).WithMessage("سفارش لغو شده است!");
        RuleFor(o => o.DeliveryTime).Null().WithMessage("سفارش تحویل داده شده است!");
    }
}