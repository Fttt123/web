using BajajWeb.Domain.Entity;
using System.Collections.Generic;

namespace BajajWeb.Domain.ViewModels.Models
{
    public class ModelViewModel
    {
        public IEnumerable<FinalModel> FinalModels { get; set; }

        public string search { get; set; } = null;
        public string minPrice { get; set; } = null;
        public string maxPrice { get; set; } = null;
    }
}
