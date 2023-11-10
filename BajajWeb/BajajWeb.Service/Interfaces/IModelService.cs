using BajajWeb.Domain.Entity;
using BajajWeb.Domain.Response;
using BajajWeb.Domain.ViewModels.Models;

namespace BajajWeb.Service.Interfaces
{
    public interface IModelService
    {
        IBaseResponse<ModelViewModel> GetAllModels();

        IBaseResponse<FinalModel> GetModel(int id);

        IBaseResponse<FinalModel> Edit(int id, FinalModel model);
    }
}
