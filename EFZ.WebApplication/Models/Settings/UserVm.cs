using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;
using EFZ.Entities.Entities;
using EFZ.Resources;

namespace EFZ.WebApplication.Models.Settings
{
    public class UserVm : BaseVm
    {

        public string UserName { get; set; }
        public string NormalizedUserName { get; internal set; }

        [Display(ResourceType = typeof(Labels), Name = "valEmail")]
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
       
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Labels), Name = "valPassword")]
        public string Password { get; set; }
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; } = 0;
        public IList<RoleVm> Roles { get; set; } = new List<RoleVm>();
        public string RolesIds { get; set; }

        public CustomerVm Customer { get; set; }
        public long? CustomerId { get; set; }
        public string AuthenticationType { get; set; }
        public bool IsAuthenticated { get; set; }
        public string Name { get; set; }

        public List<long> RolesIdList => RolesIds == null ? new List<long>() : RolesIds.Split(",").Select(long.Parse).ToList();

        public static User MapToEntityModel(UserVm source)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserVm, User>()
                .ForMember(x=>x.CustomerId, y=> y.MapFrom(z=> z.CustomerId == 0 ? null : z.CustomerId ))
                .ForMember(x => x.UserRoles, y => y.MapFrom(z => z.RolesIdList.Select(t=> new UserRole(){UserId = z.Id, RoleId = t})))
                );
            var mapper = config.CreateMapper();
            var destination = mapper.Map<User>(source);
            return destination;
        }

        public static UserVm MapToViewModelNoRelation(User source)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<User, UserVm>()
                .ForMember(x => x.Customer, y => y.Ignore())
            );

            var mapper = config.CreateMapper();
            var destination = mapper.Map<UserVm>(source);
            return destination;
        }

        public static UserVm MapToViewModel(User source)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<User, UserVm>()
                .ForMember(x=> x.Roles, y=>y.MapFrom(z=>z.UserRoles.Select(item=> RoleVm.MapToViewModel(item.Role))))

                .ForMember(x => x.CustomerId, y => y.MapFrom(z => z.CustomerId == null ? 0 : z.CustomerId))
                .ForMember(x => x.Customer, y => y.MapFrom(z => CustomerVm.MapToViewModelNoRelations(z.Customer)))
                .ForMember(x => x.RolesIds, y => y.MapFrom(z => string.Join(",", z.UserRoles.Select(item => item.RoleId.ToString()).ToList())))
            );

            var mapper = config.CreateMapper();
            var destination = mapper.Map<UserVm>(source);
            return destination;
        }
    }
}