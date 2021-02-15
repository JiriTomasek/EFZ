using System.Collections.Generic;

namespace EFZ.WebApplication.Models.Order
{
    public class OrderIndexModel : BaseIndexModel
    {
        public IList<OrderVm> Orders { get; set; }
    }
}