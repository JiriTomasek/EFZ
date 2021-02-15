using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using EFZ.Entities.Entities;
using EFZ.Resources;

namespace EFZ.WebApplication.Models.Settings
{
    public class RoleVm:BaseVm
    {

        public ICollection<UserVm> Users { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "valName")]
        public string Name { get; set; }
        public string NormalizedName { get; set; }

        public static Role MapToEntityModel(RoleVm source)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<RoleVm, Role>());
            var mapper = config.CreateMapper();
            var destination = mapper.Map<Role>(source);
            return destination;
        }

        public static RoleVm MapToViewModel(Role source)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Role, RoleVm>());
            var mapper = config.CreateMapper();
            var destination = mapper.Map<RoleVm>(source);
            return destination;
        }
    }
}