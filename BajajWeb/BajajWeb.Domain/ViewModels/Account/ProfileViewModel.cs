using System;
using System.ComponentModel.DataAnnotations;

namespace BajajWeb.Domain.ViewModels.Account
{
    public class ProfileViewModel
    {
        public int id { get; set; }

        [Required(ErrorMessage = "Укажите имя")]
        [MaxLength(30, ErrorMessage = "Имя должно иметь длину меньше 30 символов")]
        [MinLength(3, ErrorMessage = "Имя должно иметь длину больше 3 символов")]
        public string name { get; set; }

        [Required(ErrorMessage = "Укажите логин")]
        [MaxLength(30, ErrorMessage = "Логин должнен иметь длину меньше 30 символов")]
        [MinLength(4, ErrorMessage = "Логин должнен иметь длину больше 4 символов")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Укажите фамлию")]
        [MaxLength(30, ErrorMessage = "Фамилия должна иметь длину меньше 30 символов")]
        [MinLength(3, ErrorMessage = "Фамилия должна иметь длину больше 3 символов")]
        public string surname { get; set; }

        [Required(ErrorMessage = "Укажите почту")]
        [MaxLength(30, ErrorMessage = "Почта должна иметь длину меньше 30 символов")]
        [MinLength(6, ErrorMessage = "Почта должна иметь длину больше 6 символов")]
        public string mail { get; set; }

        [Required(ErrorMessage = "Укажите дату рождения")]
        public DateTime date_of_birth { get; set; }

        [Required(ErrorMessage = "Укажите номер телефона")]
        public string phone_number { get; set; }
    }
}
