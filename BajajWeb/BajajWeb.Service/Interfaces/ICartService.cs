using BajajWeb.Domain.Response;
using BajajWeb.Domain.Entity;
using BajajWeb.Domain.ViewModels.Order;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BajajWeb.Service.Interfaces
{
    public interface ICartService
    {
        IBaseResponse<IEnumerable<Cart>> GetItems(string userName, int status);

        IBaseResponse<bool> CreatCart(string userName, int id, int status);

        IBaseResponse<bool> Delete(int id,int status);
    }
}
