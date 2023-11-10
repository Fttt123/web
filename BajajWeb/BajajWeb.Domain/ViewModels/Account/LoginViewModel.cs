using System.ComponentModel.DataAnnotations;

namespace BajajWeb.Domain.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Введите Логин")]
        [MaxLength(30, ErrorMessage = "Имя должно иметь длину меньше 30 символов")]
        [MinLength(6, ErrorMessage = "Имя должно иметь длину больше 6 символов")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string password { get; set; }

        public int NumMistakes { get; set; }
    }
}
