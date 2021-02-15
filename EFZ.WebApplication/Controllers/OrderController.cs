using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFZ.Core.Entities.Dao;
using EFZ.Domain.BusinessLogic.Interface;
using EFZ.Entities.Entities;
using EFZ.Resources;
using EFZ.WebApplication.Models.Invoice;
using EFZ.WebApplication.Models.Order;
using EFZ.WebApplication.Models.SelectModel;
using EFZ.WebApplication.Models.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;

namespace EFZ.WebApplication.Controllers
{
    public class OrderController : BaseController
    {
        private readonly ISettingsBlProvider _settingsBlProvider;
        private readonly IOrderBlProvider _orderBlProvider;

        public OrderController(ISettingsBlProvider settingsBlProvider, IOrderBlProvider orderBlProvider, IConfiguration cfg, IUserBlProvider userBlProvider) : base(cfg, userBlProvider)
        {
            _settingsBlProvider = settingsBlProvider;
            _orderBlProvider = orderBlProvider;
            IndexModel = new OrderIndexModel();
        }
        public IActionResult Index()
        {
            var model = (OrderIndexModel)IndexModel;
            model.Orders = _orderBlProvider.GetCollection().Select(OrderVm.MapToViewModel).ToList();
         
            return View(model);
        }

        [HttpPost]
        public IActionResult NewOrder(OrderVm model)
        {
            var list = new List<string>();
            string response;
            if (ModelState.IsValid)
            {
                try
                {
                    if (model.OrderDetails != null)
                    {
                        if (model.OrderDetails.Any(t => string.IsNullOrEmpty(t.ProductName)))
                        {
                            list.Add(ErrorMessages.valProductNameRequired);
                            return Json(list);
                        }
                    }
                    var entityModel = OrderVm.MapToEntityModel(model);

                    _orderBlProvider.AddItem(entityModel);

                    response = "ok";
                }
                catch (Exception)
                {
                    response = "failure";
                }
            }
            else
            {
                
                foreach (var modelStateValue in ModelState.Values)
                {
                    if (modelStateValue.ValidationState != ModelValidationState.Invalid) continue;
                    if(!list.Any(t=>t.Equals(modelStateValue.AttemptedValue)))
                        list.Add(modelStateValue.AttemptedValue);

                }

                return Json(list);
            }
            list.Add(response);
            return Json(list);

        }

        [HttpGet]
        public IActionResult NewOrder()
        {
            ViewBag.Customers = 
            _settingsBlProvider.GetCustomers()
                .Select(t => new SelectModel() { label = t.Name, value = t.Id }).ToList();
            var orderVm = new OrderVm();
            

            return PartialView("_NewOrder", orderVm);
        }

        [HttpGet]
        public IActionResult OrderDetails(long orderId)
        {
            var orderVm = OrderVm.MapToViewModel(_orderBlProvider.GetSingle(orderId));


            return PartialView("_OrderDetail", orderVm);
        }

        [HttpPost]
        public IActionResult GetOrderByCustomer(long customerId)
        {
            var list = _orderBlProvider.GetCollectionByCustomerId(customerId).Select(t => new SelectModel() { label = $"{t.Customer.Name} - {t.OrderNumber}", value = t.Id }).ToList();

            return Json(list);
        }
        [HttpPost]
        public IActionResult GetAllOrderByCustomer(long customerId)
        {
            var list = _orderBlProvider.GetCollectionByCustomerId(customerId, true).Select(t => new SelectModel() { label = $"{t.Customer.Name} - {t.OrderNumber}", value = t.Id }).ToList();

            return Json(list);
        }

        [HttpPost]
        public IActionResult GetOrderNumber(long orderId)
        {
            var order = _orderBlProvider.GetSingleWithDeliveries(orderId);
            var obj = new
            {
                orderNumber = order.OrderNumber,
                isDelivered = order.Deliveries != null && order.Deliveries.Any()
            };
            return Json(obj);
        }
    }
}