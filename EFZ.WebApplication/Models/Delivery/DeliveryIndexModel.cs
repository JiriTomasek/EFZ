using System.Collections.Generic;

namespace EFZ.WebApplication.Models.Delivery
{
    public class DeliveryIndexModel : BaseIndexModel
    {
        public IList<DeliveryVm> Deliveries { get; set; }
    }
}