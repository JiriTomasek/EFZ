using System.ComponentModel.DataAnnotations;
using AutoMapper;
using EFZ.Entities.Entities;
using EFZ.Resources;
using EFZ.WebApplication.Models.Order;

namespace EFZ.WebApplication.Models.Invoice
{
    public class InvoiceAddressVm
    {
        [Display(ResourceType = typeof(Labels), Name = "valTitleAddress")]
        public string Title { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "valFirstName")]
        public string FirstName { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "valLastName")]
        public string LAstName { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "valCompany")]
        public string Company { get; set; }
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
        [Display(ResourceType = typeof(Labels), Name = "valEmail")]
        public string Email { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "valTelephone")]
        public string Telephone { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "valMobile")]
        public string Mobile { get; set; }
        public string Fax { get; set; }
        // ReSharper disable once InconsistentNaming
        public string IC { get; set; }
        // ReSharper disable once InconsistentNaming
        public string VAT { get; set; }
        // ReSharper disable once InconsistentNaming
        public string DIC { get; set; }
        public static InvoiceAddress MapToEntityModel(InvoiceAddressVm source)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<InvoiceAddressVm, InvoiceAddress>()
            );
            var mapper = config.CreateMapper();
            var destination = mapper.Map<InvoiceAddress>(source);
            return destination;
        }

        public static InvoiceAddressVm MapToViewModel(InvoiceAddress source)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<InvoiceAddress, InvoiceAddressVm>()
            );

            var mapper = config.CreateMapper();
            var destination = mapper.Map<InvoiceAddressVm>(source);
            return destination;
        }
    }
}