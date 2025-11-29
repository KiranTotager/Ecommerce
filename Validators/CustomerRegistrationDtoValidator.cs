using ECommerce.DTOs.Auth;
using FluentValidation;

namespace ECommerce.Validators
{
    public class CustomerRegistrationDtoValidator : AbstractValidator<CustomerRegistrationDto>
    {
        public CustomerRegistrationDtoValidator()
        {
            RuleFor(Cstmr => Cstmr.UserName).MinimumLength(3).WithMessage("user name must contains atleast 3 letters").WithErrorCode(""+StatusCodes.Status400BadRequest);
            RuleFor(Cstmr => Cstmr.Address).MinimumLength(10).WithMessage("user address must contaiins atleast 10 letters").WithErrorCode("" + StatusCodes.Status400BadRequest);
            RuleFor(Cstmr => Cstmr.PhoneNumber).NotEmpty().WithMessage("user phone number can not be empty").Matches(@"^\d{10}$").WithMessage("phone number must be exactly 10 numbers").WithErrorCode("" + StatusCodes.Status400BadRequest);
            RuleFor(Cstmr=>Cstmr.EmailId).EmailAddress().WithMessage("please provide correct email id").WithErrorCode("" + StatusCodes.Status400BadRequest);
        }
    }
}
