using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;
using EFZ.Core.NavigationProperty;
using EFZ.Entities.Entities;
using EFZ.Resources;

namespace EFZ.WebApplication.Models.Settings
{
    public class CustomerVm : BaseVm
    {
        [Required(ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "valNameRequired")]
        [Display(ResourceType = typeof(Labels), Name = "valName")]
        public string Name { get; set; }
        // ReSharper disable once InconsistentNaming
        [Required(ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "valIcoRequired")]
        public string IC { get; set; }
       
        // ReSharper disable once InconsistentNaming
        public string DIC { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "valAddress")]
        public long? AddressId { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "valAddress")]
        public AddressVm Address { get; set; }

        public string AddressString => Address == null ? string.Empty : 
            string.IsNullOrEmpty(Address.AlternativeStreetNumber) ?
            $"{Address.StreetName} {Address.StreetNumber}{System.Environment.NewLine}{Address.PostalCode} {Address.City}{System.Environment.NewLine}{Address.State}" : 
            $"{Address.StreetName} {Address.StreetNumber}/{Address.AlternativeStreetNumber}{System.Environment.NewLine}{Address.PostalCode} {Address.City}{System.Environment.NewLine}{Address.State}";

        public IList<UserVm> Users { get; set; }

        public string UserIds { get; set; }

        public static Customer MapToEntityModel(CustomerVm source)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<CustomerVm, Customer>()
                .ForMember(x => x.Address, y => y.MapFrom(z => AddressVm.MapToEntityModel(z.Address)))
            );
            var mapper = config.CreateMapper();
            var destination = mapper.Map<Customer>(source);
            return destination;
        }

        public static CustomerVm MapToViewModel(Customer source)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Customer, CustomerVm>()

                .ForMember(x => x.Address, y => y.MapFrom(z => AddressVm.MapToViewModel(z.Address)))

                .ForMember(x=>x.Users, y=>y.MapFrom(z=>z.Users.Select(UserVm.MapToViewModelNoRelation)))
                .ForMember(x => x.UserIds, y => y.MapFrom(z => string.Join(",", z.Users.Select(item => item.Id.ToString()).ToList())))
            );
            var mapper = config.CreateMapper();
            var destination = mapper.Map<CustomerVm>(source);
            return destination;
        }
        public static CustomerVm MapToViewModelNoRelations(Customer source)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Customer, CustomerVm>()
                .ForMember(x => x.Users, y => y.Ignore()));
            var mapper = config.CreateMapper();
            var destination = mapper.Map<CustomerVm>(source);
            return destination;
        }
       
    }
}