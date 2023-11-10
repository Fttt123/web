using System.ComponentModel.DataAnnotations;

namespace BajajWeb.Domain.ViewModels.Account
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Укажите Логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        [MinLength(6, ErrorMessage = "Пароль должен быть больше или равен 6 символов")]
        public string NewPassword { get; set; }
    }
}
