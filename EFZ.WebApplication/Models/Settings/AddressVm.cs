using System.ComponentModel.DataAnnotations;
using AutoMapper;
using EFZ.Entities.Entities;
using EFZ.Resources;

namespace EFZ.WebApplication.Models.Settings
{
    public class AddressVm : BaseVm
    {
        [Display(ResourceType = typeof(Labels), Name = "valStreetName")]

        public string StreetName { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "valStreetNumber")]

        public string StreetNumber { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "valAlternativeStreetNumber")]

        public string AlternativeStreetNumber { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "valPostalCode")]
        public string PostalCode { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "valCity")]

        public string City { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "valState")]

        public string State { get; set; }

        public static AddressVm MapToViewModel(Address source)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Address, AddressVm>()
            );
            var mapper = config.CreateMapper();
            var destination = mapper.Map<AddressVm>(source);
            return destination;
        }
        public static Address MapToEntityModel(AddressVm source)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<AddressVm, Address>());
            var mapper = config.CreateMapper();
            var destination = mapper.Map<Address>(source);
            return destination;
        }
    }
}