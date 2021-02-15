using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using EFZ.Entities.Entities;
using Microsoft.AspNetCore.Identity;

namespace EFZ.Domain.BusinessLogic.Interface
{
    public interface IUserBlProvider
    {
        void AddNewUser(User user);
        
        User GetUser(ClaimsPrincipal ctxUser);

        User GetUserByAccount(string account);

        User GetUserByEmail(string email);

        List<User> GetUsers();

        void UpdateUser(User user, List<long> modelRolesIdList);


        User GetUserById(int id);


        void DeleteUser(User user);
        Task<IdentityResult> CreateAsync(User user);
        Task<IdentityResult> DeleteAsync(User user);

        Task<User> GetUserAsyncById(long id, CancellationToken cancellationToken);
        Task<User> GetSingleAsyncByUserName(string userName, CancellationToken cancellationToken);

        public void RefreshUser();
    }
}