using BajajWeb.DAL.Interfaces;
using BajajWeb.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BajajWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly IModelService _modelService;

        public HomeController(IModelService modelService) 
        {
            _modelService = modelService;
            ViewBag.ShowHeader = true;
        }

        public IActionResult Index()
        {
            var response = _modelService.GetAllModels();
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(response.Data.FinalModels.GroupBy(p => p.priority).SelectMany(g => g).Take(3));
            }
            if (response.StatusCode == Domain.Enum.StatusCode.NotFound)
            {
                return RedirectToAction("NotFoundResult");
            }
            return RedirectToAction("Error");
        }

        public IActionResult aboutUs()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}