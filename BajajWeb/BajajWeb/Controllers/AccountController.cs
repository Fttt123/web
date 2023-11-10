using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BajajWeb.Domain.ViewModels.Account;
using BajajWeb.Service.Interfaces;
using DNTCaptcha.Core;

namespace BajajWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IDNTCaptchaValidatorService _validatorService;
        public AccountController(IAccountService accountService, IDNTCaptchaValidatorService validatorService)
        {
            _accountService = accountService;
            ViewBag.ShowHeader = true;
            _validatorService = validatorService;
        }
        [HttpGet]
        public IActionResult Register() => View();
        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = _accountService.Register(model);
                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(response.Data));
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", response.Description);
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Login()
        {
            var model = new LoginViewModel();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!_validatorService.HasRequestValidCaptchaEntry() && model.NumMistakes >=3)
                {
                    this.ModelState.AddModelError("capcha","Ошибка ввода капчи");
                    return View(model);
                }
                var response = _accountService.Login(model);
                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(response.Data));
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", response.Description);
            }
            return View(model);
        }
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = _accountService.ChangePassword(model);
                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    return Json(new { description = response.Description });
                }
            }
            var modelError = ModelState.Values.SelectMany(v => v.Errors);
            return StatusCode(StatusCodes.Status500InternalServerError, new { modelError.First().ErrorMessage });
        }
        [HttpPost]
        public IActionResult Save(ProfileViewModel model)
        {
            ModelState.Remove("id");
            ModelState.Remove("Login");
            if (ModelState.IsValid)
            {
                var response = _accountService.Save(model);
                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    return Json(new { description = response.Description });
                }
            }
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        public IActionResult Detail()
        {
            var userName = User.Identity.Name;
            var response = _accountService.GetUser(userName);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }
            return View();
        }
    }
}
