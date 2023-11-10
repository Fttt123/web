using BajajWeb.Domain.Entity;
using BajajWeb.Domain.ViewModels.Order;
using BajajWeb.Service.Implementations;
using BajajWeb.Service.Interfaces;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace BajajWeb.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IAccountService _accountService;
        private readonly IModelService _modelService;
        public OrderController(IOrderService orderService, IAccountService accountService, IModelService modelService)
        {
            _orderService = orderService;
            _accountService = accountService;
            _modelService = modelService;
        }
        [HttpGet]
        public IActionResult CreateOrder(int id)
        {
            ViewBag.ShowHeader = false;
            var orderModel = new OrderViewModel()
            {
                id_motorcycle = id,
            };
            return View(orderModel);
        }
        [HttpPost]
        public IActionResult CreateOrder(OrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = _orderService.Create(model);
                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    var user = _accountService.GetUser(User.Identity.Name);
                    ModelState.Remove("Commnet");
                    string senderEmail = "fttteha123@mail.ru";
                    string recipientEmail = user.Data.mail;
                    string subject = "Чек";
                    MailMessage message = new MailMessage();
                    message.From = new MailAddress(senderEmail);
                    message.To.Add(new MailAddress(recipientEmail));
                    message.Subject = subject;
                    string[] numbers = response.Description.Split(' ');
                    var Model = _modelService.GetModel(Convert.ToInt32(numbers[1]));
                    message.Body = CheckMaker(response.Data, Model.Data, Convert.ToInt32(numbers[0]));
                    message.IsBodyHtml = true;
                    using (SmtpClient smtpClient = new SmtpClient())
                    {
                        smtpClient.Host = "smtp.mail.ru";
                        smtpClient.Port = 587;
                        smtpClient.EnableSsl = true;
                        smtpClient.Credentials = new NetworkCredential(senderEmail, "fYVXxLT1kFtfRBdwvHrR");
                        smtpClient.Send(message);
                    }
                    return Json(new { V = "Заказ создан. Чек придёт вам на почту" });
                }
            }
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        private string CheckMaker(Orders order, FinalModel model, int count)
        {
            StringBuilder body = new StringBuilder();
            body.Append($"Чек № "+ order.id+ " заказа из магазина мотоциклов Bajaj");
            body.Append("<br>" + DateTime.Now);
            body.Append("<br>-----------------------------------");
            body.Append($"<br>Модель:{model.mark}  {model.name}, Количество: {count}, Цена: {order.final_price * count}");
            body.Append("<br>-----------------------------------");
            body.Append($"<br>Итоговая сумма: {order.final_price * count}");
            body.Append("<br>-----------------------------------");
            body.Append("<br>Спасибо за заказ в нашем магазине!");
            return body.ToString();
        }
    }
}
