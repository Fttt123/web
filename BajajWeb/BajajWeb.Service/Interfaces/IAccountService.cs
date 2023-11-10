using BajajWeb.Domain.Response;
using BajajWeb.Domain.Entity;
using BajajWeb.Domain.ViewModels.Account;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BajajWeb.Service.Interfaces
{
    public interface IAccountService
    {
        BaseResponse<ClaimsIdentity> Register(RegisterViewModel model);

        BaseResponse<ClaimsIdentity> Login(LoginViewModel model);

        BaseResponse<ProfileViewModel> GetUser(string login);

        BaseResponse<Users> Save(ProfileViewModel model);

        BaseResponse<bool> ChangePassword(ChangePasswordViewModel model);
    }
}
