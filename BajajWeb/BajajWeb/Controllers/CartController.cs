using BajajWeb.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BajajWeb.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }
        public IActionResult Detail()
        {
            var response = _cartService.GetItems(User.Identity.Name, 1);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(response.Data.ToList());
            }
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Comparison()
        {
            var response = _cartService.GetItems(User.Identity.Name, 2);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(response.Data.ToList());
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult AddCart(int id)
        {
            _cartService.CreatCart(User.Identity.Name, id, 1);
            return RedirectToAction("Detail");
        }
        [HttpGet]
        public IActionResult AddToComparison(int id)
        {
            _cartService.CreatCart(User.Identity.Name, id, 2);
            return RedirectToAction("Comparison");
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            _cartService.Delete(id, 2);
            return RedirectToAction("Detail");
        }
        [HttpGet]
        public IActionResult DeleteFromComparison(int id)
        {
            _cartService.Delete(id, 1);
            return RedirectToAction("Comparison");
        }
    }
}
