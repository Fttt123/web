using BajajWeb.DAL.Interfaces;
using BajajWeb.Domain.Entity;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BajajWeb.DAL.Repositories
{
    public class ModelRepository : IBaseRepository<FinalModel>
    {
        private readonly ApplicationDbContext _context;
        private List<FinalModel> AllModels = new List<FinalModel>();
        public ModelRepository(ApplicationDbContext context)
        {
            _context = context;
            GetData();
        }
        private void GetData()
        {
            List<FinalCharacteristics> allFinalCharacteristics = new List<FinalCharacteristics>();
            List<All_characteristics> all_Characteristics = _context.All_characteristics.ToList();
            List<Category_characteristics> category_characteristics = _context.Category_characteristics.ToList();
            List<List_of_units> list_of_units = _context.List_of_units.ToList();
            List<Marks> marks = _context.Marks.ToList();
            List<Years_of_release> year = _context.Years_of_release.ToList();
            List<Motorcycles> motorcycles = _context.Motorcycles.ToList();
            List<Photos> photos = _context.Photos.ToList();
            int i;
            foreach (var chracteristic in _context.Motorcycle_characteristics)
            {
                i = all_Characteristics.Single(p => p.id == chracteristic.id_characteristic).id_category_characteristics;
                FinalCharacteristics characteristics = new FinalCharacteristics()
                {
                    characteristic_name = all_Characteristics.Single(p => p.id == chracteristic.id_characteristic).name,
                    category_name = category_characteristics.Single(p => p.id == i).name,
                    id_motorcycle_model = chracteristic.id_motorcycle_model,
                    value = chracteristic.value,
                    unit = ""
                };
                try
                {
                    characteristics.unit = list_of_units.Single(p => p.id == chracteristic.id_unit).name;
                }
                catch { }
                allFinalCharacteristics.Add(characteristics);
            }
            allFinalCharacteristics.GroupBy(p => p.category_name);
            foreach (var model in _context.Models)
            {
                FinalModel finalModel = new FinalModel()
                {
                    id = model.id,
                    name = model.name,
                    mark = marks.Single(p => p.id == model.id_mark).name,
                    years_of_release = year.Single(p => p.id == model.id_years_of_release).year,
                    description = model.description,
                    retail_price = model.retail_price,
                    priority = model.priority,
                    motorcycles = motorcycles.Where(p => p.id_model == model.id && p.id_status != 2).ToList(),
                    photos = photos.Where(p => p.id_model == model.id).ToList(),
                    characteristics = allFinalCharacteristics.Where(p => p.id_motorcycle_model == model.id).ToList()
                };
                AllModels.Add(finalModel);
            }
        }
        public FinalModel Update(FinalModel entity)
        {
            var moto = entity.motorcycles.FirstOrDefault();
            moto.id_status = 2;
            var model = _context.Models.Single(p => p.id == entity.id);
            model.total_count -= 1;
            _context.SaveChanges();
            return entity;
        }
        public List<FinalModel> GetAll()
        {
            return AllModels;
        }
        public void Register(FinalModel entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
