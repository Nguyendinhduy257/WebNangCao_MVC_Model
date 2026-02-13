using FluentValidation;
using WebNangCao_MVC_Model.Models;

namespace WebNangCao_MVC_Model.Validators
{
    // --- XÓA class AuthValidators bao bên ngoài đi ---

    // Class độc lập 1: LoginValidator
    public class LoginValidator : AbstractValidator<LoginViewModel>
    {
        public LoginValidator()
        {
            RuleFor(x => x.UsernameOrEmail)
                .NotEmpty().WithMessage("Vui lòng nhập email hoặc tên đăng nhập");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Vui lòng nhập mật khẩu");
        }
    }

    // Class độc lập 2: RegisterValidator
    public class RegisterValidator : AbstractValidator<RegisterViewModel>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Vui lòng nhập họ và tên");

            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Vui lòng nhập tên đăng nhập")
                .MinimumLength(3).WithMessage("Tên đăng nhập phải có ít nhất 3 ký tự");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Vui lòng nhập email")
                .EmailAddress().WithMessage("Địa chỉ email không hợp lệ");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Vui lòng nhập mật khẩu")
                .MinimumLength(6).WithMessage("Mật khẩu phải có ít nhất 6 ký tự");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage("Mật khẩu xác nhận không khớp");
        }
    }
}