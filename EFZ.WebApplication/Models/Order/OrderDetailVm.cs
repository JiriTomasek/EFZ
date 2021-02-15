using AutoMapper;
using EFZ.Entities.Entities;
using EFZ.WebApplication.Models.Delivery;
using EFZ.WebApplication.Models.Settings;

namespace EFZ.WebApplication.Models.Order
{
    public class OrderDetailVm : BaseVm
    {
        public long OrderId { get; set; }
        public long? DeliveryId { get; set; }
        public string ProductName { get; set; }
        public long? Quantity { get; set; } = 0;
        public double? UnitPrice { get; set; } = 0;
        public double? Discount { get; set; } = 0;
        public double? TaxRate { get; set; } = 0;
        public double? TotalNet { get; set; } = 0;
        public double? TotalTax { get; set; } = 0;
        public long Delivered { get; set; }

        public static OrderItem MapToEntityModel(OrderDetailVm source)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<OrderDetailVm, OrderItem>()
            );
            var mapper = config.CreateMapper();
            var destination = mapper.Map<OrderItem>(source);
            return destination;
        }

        public static OrderDetailVm MapToViewModel(OrderItem source)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<OrderItem, OrderDetailVm>()
           );

            var mapper = config.CreateMapper();
            var destination = mapper.Map<OrderDetailVm>(source);
            return destination;
        }
    }
}