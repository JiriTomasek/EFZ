using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using EFZ.Core;
using EFZ.Entities.Entities;
using EFZ.Resources;
using EFZ.WebApplication.Models.Settings;

namespace EFZ.WebApplication.Models.Invoice
{
    public class InvoiceVm : BaseVm
    {
        public long CustomerId { get; set; }
        public Guid? CompanyId { get; set; } = Guid.Parse(Constants.CompanyId);
        public long? OrderId { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "valOrderNumber")]
        public string OrderNumber { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "valInvoiceNumber")]
        public string InvoiceNumber { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "valInvoiceDate")]
        public DateTime InvoiceDate { get; set; }
        public string InvoiceType { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "valTotalNet")]
        public double ItemsNet { get; set; }
        public double ItemsTax { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "valItemsTotal")]
        public double ItemsTotal { get; set; }
        public long? InvoiceAddressId { get; set; }
        
        public string InvoiceText { get; set; }

     
        public InvoiceAddress InvoiceAddress { get; set; }

        public CompanyVm Company { get; set; }

        public CustomerVm Customer { get; set; }

        public IList<InvoiceItem> InvoiceItems { get; set; }
        public static Entities.Entities.Invoice MapToEntityModel(InvoiceVm source)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<InvoiceVm, Entities.Entities.Invoice>()
                .ForMember(x => x.Customer, y => y.MapFrom(z => CustomerVm.MapToEntityModel(z.Customer)))

            );
            var mapper = config.CreateMapper();
            var destination = mapper.Map<Entities.Entities.Invoice>(source);
            return destination;
        }

        public static InvoiceVm MapToViewModel(Entities.Entities.Invoice source)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Entities.Entities.Invoice, InvoiceVm>()
                .ForMember(x => x.Customer, y => y.MapFrom(z => CustomerVm.MapToViewModelNoRelations(z.Customer)))
                .ForMember(x => x.Company, y => y.MapFrom(z => CompanyVm.MapToViewModel(z.Company)))

            );

            var mapper = config.CreateMapper();
            var destination = mapper.Map<InvoiceVm>(source);
            return destination;
        }
    }
}