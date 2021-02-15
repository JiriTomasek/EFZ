using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;
using EFZ.Entities.Entities;
using EFZ.Resources;
using EFZ.WebApplication.Models.Settings;

namespace EFZ.WebApplication.Models.Order
{
    public class OrderVm : BaseVm
    {
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "valCustomerRequired")]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "valCustomerRequired")]
        [Display(ResourceType = typeof(Labels), Name = "valCustomer")]
        public long CustomerId { get; set; }
        [Required(ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "valOrderNumerRequired")]
        [StringLength(1000,ErrorMessage = "{0} musí byt dlouhé minimalně {2} znaků.", MinimumLength = 10)]
        [Display(ResourceType = typeof(Labels), Name = "valOrderNumber")]
        public string OrderNumber { get; set; }
        
        [Required(ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "valOrderDateRequired")]
        [Display(ResourceType = typeof(Labels), Name = "valOrderDate")]
        public DateTime OrderDate { get; set; } = DateTime.Now;
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "valTotalPriceRequired")]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "valTotalPriceRequired")]
        [Display(ResourceType = typeof(Labels), Name = "valTotalPrice")]
        public double TotalPrice { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "valTotalNet")]
        public long TotalNet { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "valTotalTax")]
        public long TotalTax { get; set; }

        public CustomerVm Customer { get; set; }

        public IList<OrderDetailVm> OrderDetails { get; set; }
        public static Entities.Entities.Order MapToEntityModel(OrderVm source)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<OrderVm, Entities.Entities.Order>()
                .ForMember(x => x.OrderItems, y => y.MapFrom(z => z.OrderDetails.Select(OrderDetailVm.MapToEntityModel)))
                .ForMember(x => x.Customer, y => y.MapFrom(z => CustomerVm.MapToEntityModel(z.Customer)))
            );
            var mapper = config.CreateMapper();
            var destination = mapper.Map<Entities.Entities.Order>(source);
            return destination;
        }

        public static OrderVm MapToViewModel(Entities.Entities.Order source)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Entities.Entities.Order, OrderVm>()
                .ForMember(x => x.OrderDetails, y => y.MapFrom(z => z.OrderItems.Select(OrderDetailVm.MapToViewModel)))

                .ForMember(x => x.Customer, y => y.MapFrom(z => CustomerVm.MapToViewModelNoRelations(z.Customer)))
            );

            var mapper = config.CreateMapper();
            var destination = mapper.Map<OrderVm>(source);
            return destination;
        }
    }
}
