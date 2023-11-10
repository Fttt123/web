using BajajWeb.DAL.Interfaces;
using BajajWeb.Domain.Enum;
using BajajWeb.Domain.Response;
using System.Collections.Generic;
using System;
using BajajWeb.Domain.Entity;
using BajajWeb.Service.Interfaces;
using System.Linq;

namespace BajajWeb.Service.Implementations
{
    public class CartService : ICartService
    {
        private readonly IBaseRepository<Users> _userRepository;
        private readonly IBaseRepository<FinalModel> _modelRepository;
        private readonly IBaseRepository<Cart> _cartRepository;
        public CartService(IBaseRepository<Users> userRepository, IBaseRepository<FinalModel> modelRepository, IBaseRepository<Cart> cartRepository)
        {
            _userRepository = userRepository;
            _modelRepository = modelRepository;
            _cartRepository = cartRepository;
        }
        public IBaseResponse<IEnumerable<Cart>> GetItems(string userName, int status)
        {
            try
            {
                var user = _userRepository.GetAll().SingleOrDefault(p => p.Login == userName);
                var carts = _cartRepository.GetAll().Where(p => p.user_id == user.id && p.count !=0 && (p.status_id == status || p.status_id == 4));
                if (user == null)
                {
                    return new BaseResponse<IEnumerable<Cart>>()
                    {
                        Description = "Пользователь не найден",
                        StatusCode = StatusCode.NotFound
                    };
                }
                var model = _modelRepository.GetAll();
                var response = from p in carts
                               join c in _modelRepository.GetAll() on p.motorcycle_id equals c.id
                               select new Cart()
                               {
                                   id = p.id,
                                   motorcycle_id = c.id,
                                   user_id = user.id,
                                   status_id = 1,
                                   count = p.count,
                                   model = model.SingleOrDefault(j=>j.id == p.motorcycle_id)
                               };
                return new BaseResponse<IEnumerable<Cart>>()
                {
                    Data = response,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<Cart>>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
        public IBaseResponse<bool> CreatCart(string userName, int id, int status)
        {
            try
            {
                var models = _modelRepository.GetAll();
                var user = _userRepository.GetAll().SingleOrDefault(x => x.Login == userName);
                var model = _modelRepository.GetAll().SingleOrDefault(p => p.id == id);
                if (user == null || model == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Description = "Не найдено",
                        StatusCode = StatusCode.NotFound
                    };
                }
                var oldCart = _cartRepository.GetAll().SingleOrDefault(p => p.user_id == user.id && p.motorcycle_id == model.id);
                if (oldCart == null)
                {
                    var cart = new Cart()
                    {
                        user_id = user.id,
                        motorcycle_id = model.id,
                        status_id = status,
                        count = 1,
                        model = model
                    };
                    _cartRepository.Register(cart);
                }
                else if (oldCart.status_id == 3)
                {
                    oldCart.status_id = status;
                    oldCart.count += 1;
                    _cartRepository.Update(oldCart);
                }
                else if (oldCart.status_id != status)
                {
                    oldCart.status_id = 4;
                    _cartRepository.Update(oldCart);
                }
                else if(oldCart.status_id != 2)
                {
                    oldCart.count += 1;
                    _cartRepository.Update(oldCart);
                }
                return new BaseResponse<bool>()
                {
                    Description = "Добавленно в корзину",
                    StatusCode = StatusCode.OK
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
        public IBaseResponse<bool> Delete(int id, int status)
        {
            try
            {
                var cart = _cartRepository.GetAll().SingleOrDefault(x => x.id == id);
                if (cart == null)
                {
                    return new BaseResponse<bool>()
                    {
                        StatusCode = StatusCode.NotFound,
                        Description = "Не найдено"
                    };
                }
                if (cart.count == 1)
                {
                    if (cart.status_id != 4)
                    {
                        cart.status_id = 3;
                    }
                    else
                    {
                        cart.status_id = status;
                    }    
                    cart.count = 0;
                }
                else
                {
                    cart.count -= 1;
                }
                _cartRepository.Update(cart);
                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.OK,
                    Description = "Удалено"
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
