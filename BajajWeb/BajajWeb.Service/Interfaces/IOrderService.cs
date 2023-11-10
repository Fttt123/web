using BajajWeb.Domain.Response;
using BajajWeb.Domain.Entity;
using BajajWeb.Domain.ViewModels.Order;

namespace BajajWeb.Service.Interfaces
{
    public interface IOrderService
    {
        IBaseResponse<Orders> Create(OrderViewModel model);

        IBaseResponse<bool> Delete(int id);
    }
}
