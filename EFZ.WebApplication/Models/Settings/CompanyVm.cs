using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using EFZ.Core.NavigationProperty;
using EFZ.Entities.Entities;
using EFZ.Resources;

namespace EFZ.WebApplication.Models.Settings
{
    public class CompanyVm
    {
        public Guid Id { get; set; }
        [Required(ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "valNameRequired")]
        [Display(ResourceType = typeof(Labels), Name = "valName")]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "valIcoRequired")]
        // ReSharper disable once InconsistentNaming
        public string IC { get; set; }
        // ReSharper disable once InconsistentNaming
        public string DIC { get; set; }

       

        public long? AddressId { get; set; }

        public AddressVm Address { get; set; }

        public static Company MapToEntityModel(CompanyVm source)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<CompanyVm, Company>()
                .ForMember(x => x.Address, y => y.MapFrom(z => AddressVm.MapToEntityModel(z.Address)))
            );
            var mapper = config.CreateMapper();
            var destination = mapper.Map<Company>(source);
            return destination;
        }

        public static CompanyVm MapToViewModel(Company source)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Company, CompanyVm>()

                .ForMember(x => x.Address, y => y.MapFrom(z => AddressVm.MapToViewModel(z.Address)))

            );
            var mapper = config.CreateMapper();
            var destination = mapper.Map<CompanyVm>(source);
            return destination;
        }
    }
}