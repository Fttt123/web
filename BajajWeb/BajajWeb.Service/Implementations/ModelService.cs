using BajajWeb.DAL.Interfaces;
using BajajWeb.Domain.Entity;
using BajajWeb.Domain.Enum;
using BajajWeb.Domain.Response;
using BajajWeb.Service.Interfaces;
using System;
using System.Linq;
using BajajWeb.Domain.ViewModels.Models;

namespace BajajWeb.Service.Implementations
{
    public class ModelService : IModelService
    {
        private readonly IBaseRepository<FinalModel> modelRepository;
        public ModelService(IBaseRepository<FinalModel> modelRepository)
        {
            this.modelRepository = modelRepository;
        }
        public IBaseResponse<FinalModel> GetModel(int id)
        {
            var baseResponse = new BaseResponse<FinalModel>();
            try
            {
                var model = modelRepository.GetAll().FirstOrDefault(p => p.id == id);
                baseResponse.StatusCode = StatusCode.OK;
                return GetModel(model, baseResponse);
            }
            catch (Exception ex)
            {
                return ExceptionForOneModel(ex);
            }
        }
        public IBaseResponse<ModelViewModel> GetAllModels()
        {
            var baseResponse = new BaseResponse<ModelViewModel>();
            try
            {
                var models = modelRepository.GetAll().ToList();
                if(models.Count == 0) 
                {
                    baseResponse.Description = "Найдено 0 элементов";
                    baseResponse.StatusCode = StatusCode.NotFound;
                    return baseResponse;
                }
                ModelViewModel model = new ModelViewModel();
                model.FinalModels = models;
                baseResponse.Data = model;
                baseResponse.StatusCode = StatusCode.OK;
                return baseResponse;
            }
            catch(Exception ex)
            {
                return new BaseResponse<ModelViewModel>()
                {
                    Description = $"[GetAllModels] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
        private IBaseResponse<FinalModel> ExceptionForOneModel(Exception ex)
        {
            return new BaseResponse<FinalModel>()
            {
                Description = $"[GetModel] : {ex.Message}",
                StatusCode = StatusCode.InternalServerError
            };
        }
        private IBaseResponse<FinalModel> GetModel(FinalModel model, BaseResponse<FinalModel> baseResponse)
        {
            if (model == null)
            {
                baseResponse.Description = "Model not found";
                baseResponse.StatusCode = StatusCode.NotFound;
                return baseResponse;
            }
            baseResponse.Data = model;
            return baseResponse;
        }
        public IBaseResponse<FinalModel> Edit(int id, FinalModel model)
        {
            var baseResponse = new BaseResponse<FinalModel>();
            try
            {
                var editedModel = modelRepository.GetAll().FirstOrDefault(p => p.id == id);
                if (editedModel == null)
                {
                    baseResponse.Description = "Model not found";
                    baseResponse.StatusCode = StatusCode.NotFound;
                    return baseResponse;
                }
                modelRepository.Update(editedModel);
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<FinalModel>()
                {
                    Description = $"[Edit] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
