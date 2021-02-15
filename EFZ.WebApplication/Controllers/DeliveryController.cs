using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFZ.Domain.BusinessLogic.Interface;
using EFZ.Resources;
using EFZ.WebApplication.Models.Delivery;
using EFZ.WebApplication.Models.Order;
using EFZ.WebApplication.Models.SelectModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;

namespace EFZ.WebApplication.Controllers
{
    public class DeliveryController : BaseController
    {
        private readonly IOrderBlProvider _orderBlProvider;
        private readonly ISettingsBlProvider _settingsBlProvider;
        private readonly IDeliveryBlProvider _deliveryBlProvider;

        public DeliveryController(IOrderBlProvider orderBlProvider,ISettingsBlProvider settingsBlProvider, IDeliveryBlProvider deliveryBlProvider,IConfiguration cfg, IUserBlProvider userBlProvider) : base(cfg, userBlProvider)
        {
            _orderBlProvider = orderBlProvider;
            _settingsBlProvider = settingsBlProvider;
            _deliveryBlProvider = deliveryBlProvider;
            IndexModel = new DeliveryIndexModel();
        }

        public IActionResult Index()
        {
            var model = (DeliveryIndexModel)IndexModel;
            model.Deliveries = _deliveryBlProvider.GetCollection().Select(DeliveryVm.MapToViewModel).ToList();
         
            return View(model);
        }

        [HttpPost]
        public IActionResult NewDelivery(DeliveryVm model)
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
                    var entityModel = DeliveryVm.MapToEntityModel(model);

                    _deliveryBlProvider.AddItem(entityModel);

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
                    if (!list.Any(t => t.Equals(modelStateValue.AttemptedValue)))
                        list.Add(modelStateValue.AttemptedValue);

                }

                return Json(list);
            }
            list.Add(response);
            return Json(list);

        }

        [HttpGet]
        public IActionResult NewDelivery()
        {
            ViewBag.Customers =
            _settingsBlProvider.GetCustomers()
                .Select(t => new SelectModel() { label = t.Name, value = t.Id }).ToList();
            var deliveryVm = new DeliveryVm();


            return PartialView("_NewDelivery", deliveryVm);
        }

        [HttpGet]
        public IActionResult DeliveryDetail(long deliveryId)
        {
            var deliveryVm = DeliveryVm.MapToViewModel(_deliveryBlProvider.GetSingle(deliveryId));


            return PartialView("_OrderDetail", deliveryVm);
        }

        [HttpPost]
        public IActionResult GetDeliveryByOrder(long orderId)
        {
            var list = _deliveryBlProvider.GetCollection(orderId).Select(t => new SelectModel() { label = $"{t.DeliveryNumber}", value = t.Id }).ToList();

            return Json(list);
        }

        [HttpPost]
        public IActionResult GetDeliveryData(long deliveryId)
        {
            var delivery = _deliveryBlProvider.GetSingle(deliveryId);
            var obj = new
            {
                orderNumber = delivery.OrderNumber,
                deliveryNumber = delivery.DeliveryNumber
            };
            return Json(obj);
        }
    }
}