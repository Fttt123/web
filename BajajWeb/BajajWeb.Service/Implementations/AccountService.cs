using BajajWeb.DAL.Interfaces;
using BajajWeb.Domain.Enum;
using BajajWeb.Domain.Response;
using BajajWeb.Domain.Entity;
using BajajWeb.Domain.Helpers;
using BajajWeb.Domain.ViewModels.Account;
using BajajWeb.Service.Interfaces;
using System;
using System.Security.Claims;
using System.Linq;
using System.Collections.Generic;

namespace BajajWeb.Service.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IBaseRepository<Users> _userRepository;
        public AccountService(IBaseRepository<Users> userRepository)
        {
            _userRepository = userRepository;
        }
        public BaseResponse<bool> ChangePassword(ChangePasswordViewModel model)
        {
            try
            {
                var user = _userRepository.GetAll().FirstOrDefault(x => x.Login == model.Login);
                if (user == null)
                {
                    return new BaseResponse<bool>()
                    {
                        StatusCode = StatusCode.NotFound,
                        Description = "Пользователь не найден"
                    };
                }
                string check = checkPassword(model.NewPassword);
                if (check != null )
                {
                    return new BaseResponse<bool>()
                    {
                        StatusCode = StatusCode.NotFound,
                        Description = check
                    };
                }
                user.password = HashPasswordHelper.HashPassowrd(model.NewPassword);
                _userRepository.Update(user);
                return new BaseResponse<bool>()
                {
                    Data = true,
                    StatusCode = StatusCode.OK,
                    Description = "Пароль обновлен"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
        private string checkPassword(string passwor)
        {
            if (!passwor.AsEnumerable().Any(ch => char.IsDigit(ch)))
            {
                return "Пароль должен содежать цифры";
            }
            if (!passwor.AsEnumerable().Any(ch => char.IsLetter(ch)))
            {
                return "Пароль должен содержать буквы";
            }
            return null;
        }
        public BaseResponse<ProfileViewModel> GetUser(string login)
        {
            try
            {
                var user = _userRepository.GetAll().Select(p => new ProfileViewModel()
                {
                    id = p.id,
                    name = p.name,
                    Login = p.Login,
                    surname = p.surname,
                    mail = p.mail,
                    date_of_birth = p.date_of_birth,
                    phone_number = p.phone_number
                }).FirstOrDefault(p => p.Login == login);
                if (user == null)
                {
                    return new BaseResponse<ProfileViewModel>()
                    {
                        StatusCode = StatusCode.NotFound,
                        Description = "Пользователь не найден"
                    };
                }
                return new BaseResponse<ProfileViewModel>()
                {
                    StatusCode = StatusCode.OK,
                    Data = user
                };
            }
            catch
            {
                return new BaseResponse<ProfileViewModel>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = "Что-то пошло не так"
                };
            }
        }
        public BaseResponse<ClaimsIdentity> Login(LoginViewModel model)
        {
            try
            {
                var user = _userRepository.GetAll().FirstOrDefault(x => x.Login == model.Login && x.id_role == 2);
                if (user == null)
                {
                    model.NumMistakes++;
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Description = "Пользователь не найден"
                    };
                }
                if (user.password != HashPasswordHelper.HashPassowrd(model.password))
                {
                    model.NumMistakes++;
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Description = "Неверный пароль или логин"
                    };
                }
                var result = Authenticate(user);
                return new BaseResponse<ClaimsIdentity>()
                {
                    Data = result,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
        public BaseResponse<ClaimsIdentity> Register(RegisterViewModel model)
        {
            try
            {
                if (_userRepository.GetAll().Any(x => x.Login == model.Login))
                {
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Description = "Пользователь с таким логином уже есть",
                    };
                }
                int dateResult = DateTime.Compare(DateTime.Parse(model.date_of_birth), DateTime.Today);
                if (dateResult >= 0 || DateTime.Today.Year - DateTime.Parse(model.date_of_birth).Year == 200)
                {
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Description = "Дата рождения указана не верно",
                    };
                }
                string check = checkPassword(model.password);
                if (check != null)
                {
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Description = check
                    };
                }
                var user = new Users()
                {
                    name = model.name,
                    id_role = 2,
                    password = HashPasswordHelper.HashPassowrd(model.password),
                    id_job_title = 1,
                    Login = model.Login,
                    surname = model.surname,
                    mail = model.mail,
                    date_of_birth = DateTime.Parse(model.date_of_birth),
                    phone_number = model.phone_number
                };
                _userRepository.Register(user);
                var result = Authenticate(user);
                return new BaseResponse<ClaimsIdentity>()
                {
                    Data = result,
                    Description = "Объект добавился",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
        public BaseResponse<Users> Save(ProfileViewModel model)
        {
            try
            {
                var profile = _userRepository.GetAll().FirstOrDefault(x => x.id == model.id);
                profile.surname = model.surname;
                profile.phone_number = model.phone_number;
                profile.date_of_birth = model.date_of_birth;
                profile.mail = model.mail;
                profile.name = model.name;
                _userRepository.Update(profile);
                return new BaseResponse<Users>()
                {
                    Data = profile,
                    Description = "Данные обновлены",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Users>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"Внутренняя ошибка: {ex.Message}"
                };
            }
        }
        private ClaimsIdentity Authenticate(Users user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login)
            };
            return new ClaimsIdentity(claims, "ApplicationCookie",
                ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        }
    }
}
