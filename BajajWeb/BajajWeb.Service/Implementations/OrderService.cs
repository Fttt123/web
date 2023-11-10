using BajajWeb.DAL.Interfaces;
using BajajWeb.Domain.Enum;
using BajajWeb.Domain.Response;
using BajajWeb.Domain.Entity;
using System;
using BajajWeb.Domain.ViewModels.Order;
using System.Linq;
using BajajWeb.Service.Interfaces;
using BajajWeb.Domain.ViewModels.Account;

namespace BajajWeb.Service.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IBaseRepository<Users> _userRepository;
        private readonly IBaseRepository<Orders> _orderRepository;
        private readonly IBaseRepository<FinalModel> _modelRepository;
        private readonly IBaseRepository<Cart> _cartRepository;
        public OrderService(IBaseRepository<Users> userRepository, IBaseRepository<Orders> orderRepository, IBaseRepository<FinalModel> modelRepository, IBaseRepository<Cart> cartRepository)
        {
            _userRepository = userRepository;
            _orderRepository = orderRepository;
            _modelRepository = modelRepository;
            _cartRepository = cartRepository;
        }
        public IBaseResponse<Orders> Create(OrderViewModel model)
        {
            try
            {
                var cart = _cartRepository.GetAll().SingleOrDefault(p => p.id == model.id_motorcycle);
                var user = _userRepository.GetAll().SingleOrDefault(x => x.id == cart.user_id);
                if (user == null)
                {
                    return new BaseResponse<Orders>()
                    {
                        Description = "Пользователь не найден",
                        StatusCode = StatusCode.NotFound
                    };
                }
                var finalModel = _modelRepository.GetAll().SingleOrDefault(p => p.id == cart.motorcycle_id);
                int count = cart.count;
                if(count > finalModel.motorcycles.Where(p=>p.id_status == 1).Count())
                {
                    return new BaseResponse<Orders>()
                    {
                        Description = "У нас нет такого количества мотоциклов данной модели",
                        StatusCode = StatusCode.InternalServerError
                    };
                }
                Orders order = null;
                Motorcycles motoid = null;
                for (int i = 0; i < count; i++)
                {
                    motoid = finalModel.motorcycles.FirstOrDefault();
                    order = new Orders()
                    {
                        id_motorcycle = motoid.id,
                        id_order_status = 1,
                        id_user_buyer = user.id,
                        date = DateTime.Now,
                        id_payment_method = model.id_payment_method,
                        final_price = finalModel.retail_price
                    };
                    _orderRepository.Register(order);
                    _modelRepository.Update(finalModel);
                }
                cart.status_id = 3;
                cart.count = 0;
                _cartRepository.Update(cart);
                return new BaseResponse<Orders>()
                {
                    Data = order,
                    Description = count+" "+ motoid.id_model,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Orders>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
        public IBaseResponse<bool> Delete(int id)
        {
            try
            {
                var order = _orderRepository.GetAll().SingleOrDefault(x => x.id == id);
                if (order == null)
                {
                    return new BaseResponse<bool>()
                    {
                        StatusCode = StatusCode.NotFound,
                        Description = "Заказ не найден"
                    };
                }
                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.OK,
                    Description = "Заказ удален"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
