using BajajWeb.Domain.ViewModels.Models;
using BajajWeb.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BajajWeb.Controllers
{
    public class ModelController : Controller
    {
        private readonly IModelService _modelService;
        public ModelController(IModelService modelService)
        {
            _modelService = modelService;
            ViewBag.ShowHeader = true;
        }
        [HttpGet]
        public IActionResult GetModels()
        {
            var response = _modelService.GetAllModels();
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                response.Data.FinalModels = response.Data.FinalModels.GroupBy(p => p.priority).SelectMany(g => g);
                return View(response.Data);
            }
            if (response.StatusCode == Domain.Enum.StatusCode.NotFound)
            {
                return RedirectToAction("NotFoundResult");
            }
            return RedirectToAction("Error");
        }
        [HttpPost]
        public IActionResult GetModels(ModelViewModel response, Dictionary<string, string> filters)
        {
            if (ModelState.IsValid)
            {
                var allModels = _modelService.GetAllModels().Data.FinalModels;
                response.FinalModels = allModels;
                response.FinalModels = response.FinalModels.GroupBy(p => p.priority).SelectMany(g => g);
                string search = response.search;
                string minPrice = response.minPrice;
                string maxPrice = response.maxPrice;
                var models = response.FinalModels;
                if (!string.IsNullOrEmpty(search))
                {
                    models = models.Where(p => p.name.Contains(search) || p.mark.Contains(search) || p.description.Contains(search));
                }
                if (!string.IsNullOrEmpty(minPrice) && int.TryParse(minPrice, out var parsedMinPrice))
                {
                    models = models.Where(m => Convert.ToInt32(m.retail_price) >= parsedMinPrice);
                }
                if (!string.IsNullOrEmpty(maxPrice) && int.TryParse(maxPrice, out var parsedMaxPrice))
                {
                    models = models.Where(m => Convert.ToInt32(m.retail_price) <= parsedMaxPrice);
                }
                foreach (var filter in filters)
                {
                    if (filter.Value != null && filter.Key != "search" && filter.Key != "minPrice" && filter.Key != "maxPrice" && filter.Key != "__RequestVerificationToken")
                        models = models.Where(m => m.characteristics.Any(c => c.characteristic_name == filter.Key && c.value == filter.Value));
                }
                response.FinalModels = models;
                return View(response);
            }
            return View(response);
        }
        [HttpGet]
        public IActionResult GetModel(int id)
        {
            ViewBag.ShowHeader = false;
            var response = _modelService.GetModel(id);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }
            if (response.StatusCode == Domain.Enum.StatusCode.NotFound)
            {
                return RedirectToAction("NotFoundResult");
            }
            return RedirectToAction("Error");
        }
    }
}