using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;
using EFZ.Resources;
using EFZ.WebApplication.Models.Order;
using EFZ.WebApplication.Models.Settings;

namespace EFZ.WebApplication.Models.Delivery
{
    public class DeliveryVm : BaseVm
    {
        [Display(ResourceType = typeof(Labels), Name = "valOrder")]
        public long OrderId { get; set; }
        [Required(ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "valDeliveryNumerRequired")]
        [Display(ResourceType = typeof(Labels), Name = "valDeliveryNumber")]
        public string DeliveryNumber { get; set; }
        [Required(ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "valOrderNumerRequired")]
        [StringLength(1000, ErrorMessage = "{0} musí byt dlouhé minimalně {2} znaků.", MinimumLength = 10)]
        [Display(ResourceType = typeof(Labels), Name = "valOrderNumber")]
        public string OrderNumber { get; set; }
        [Required(ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "valDeliveryDateRequired")]
        [Display(ResourceType = typeof(Labels), Name = "valDeliveryDate")]
        public DateTime DeliveryDate { get; set; } = DateTime.Now;

        [Display(ResourceType = typeof(Labels), Name = "valDeliveryAll")]
        public bool DeliveryAll { get; set; } = true;
        public OrderVm Order { get; set; }
        public IList<OrderDetailVm> OrderDetails { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "valCustomer")]
        public long CustomerId { get; set; }
        public static Entities.Entities.Delivery MapToEntityModel(DeliveryVm source)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<DeliveryVm, Entities.Entities.Delivery>()
                .ForMember(x => x.OrderItems, y => y.MapFrom(z => z.OrderDetails.Select(OrderDetailVm.MapToEntityModel)))
                .ForMember(x => x.Order, y => y.MapFrom(z => OrderVm.MapToEntityModel(z.Order)))
            );
            var mapper = config.CreateMapper();
            var destination = mapper.Map<Entities.Entities.Delivery>(source);
            return destination;
        }

        public static DeliveryVm MapToViewModel(Entities.Entities.Delivery source)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Entities.Entities.Delivery, DeliveryVm>()
                .ForMember(x => x.OrderDetails, y => y.MapFrom(z => z.OrderItems.Select(OrderDetailVm.MapToViewModel)))
                .ForMember(x => x.Order, y => y.MapFrom(z => OrderVm.MapToViewModel(z.Order)))
            );

            var mapper = config.CreateMapper();
            var destination = mapper.Map<DeliveryVm>(source);
            return destination;
        }

    }
}