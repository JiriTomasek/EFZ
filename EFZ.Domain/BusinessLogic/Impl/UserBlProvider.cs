using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using EFZ.Core.Entities.Dao;
using EFZ.Core.Mapping;
using EFZ.Domain.BusinessLogic.Interface;
using EFZ.Entities.Entities;
using Microsoft.AspNetCore.Identity;

namespace EFZ.Domain.BusinessLogic.Impl
{
    public class UserBlProvider : IUserBlProvider
    {

        private static List<User> _users = new List<User>();

        private readonly ICommonDao<User> _userDao;
        private readonly ICommonDao<UserRole> _userRoleDao;

        public UserBlProvider(IBaseDaoFactory daoFactory)
        {
            _userDao = daoFactory.GetDao<User>();
            _userRoleDao = daoFactory.GetDao<UserRole>();
            if(!_users.Any())
                RefreshUser();
        }
        public void AddNewUser(User user)
        {
            if (user.Id == 0)
            {
                _userDao.AddItem(user);
            }

            RefreshUser();
        }

        public  void RefreshUser()
        {
            _users = _userDao.GetCollection(null,true).ToList();
        }


        public User GetUser(ClaimsPrincipal ctxUser)
        {
            return _users.FirstOrDefault(item => item.Email.ToLower().Equals(ctxUser.Identity?.Name?.ToLower()));
        }

        public User GetUserByAccount(string account)
        {
            return _users.FirstOrDefault(item => item.UserName.Equals(account));
        }

        public User GetUserByEmail(string email)
        {
            return _users.FirstOrDefault(item => item.Email.ToLower().Equals(email.ToLower()));
        }

        public List<User> GetUsers()
        {
            return _users;
        }

        public void UpdateUser(User user, List<long> modelRolesIdList)
        {
            _userDao.UpdateItem(user);
            UpdateRelation(user,modelRolesIdList);
            RefreshUser();
        }

        private void  UpdateRelation(User user, List<long> modelRolesIdList)
        {
            var collection = _userRoleDao.GetCollection(t => t.UserId.Equals(user.Id)).ToList();

            while (modelRolesIdList.Any())
            {
                if (!collection.Any(t => t.RoleId.Equals(modelRolesIdList[0])))
                {
                    _userRoleDao.AddItem(
                        user.UserRoles.FirstOrDefault(t => t.RoleId.Equals(modelRolesIdList[0])));
                }

                collection.Remove(collection.FirstOrDefault(t => t.RoleId.Equals((modelRolesIdList[0]))));
                modelRolesIdList.Remove(modelRolesIdList[0]);
            }

            if (collection.Any())
            {
                _userRoleDao.DeleteRange(collection);
            }

        }

        public User GetUserById(int id)
        {
            return _users.FirstOrDefault(u => u.Id.Equals(id));
        }

        public void DeleteUser(User user)
        {
            _userDao.DeleteItem(user);
            RefreshUser();
        }

        public Task<IdentityResult> CreateAsync(User user)
        {
            var result =_userDao.CreateAsync(user);
            var t = Task.Run(() => RefreshUserAsync(result));
            return result;
        }

        private void RefreshUserAsync(Task<IdentityResult> result)
        {
            while (result.IsCompleted)
            {
            }
            RefreshUser();
        }

        public Task<IdentityResult> DeleteAsync(User user)
        {
            var result = _userDao.DeleteAsync(user);
            var t = Task.Run(() => RefreshUserAsync(result));
            return result;
        }

        public Task<User> GetUserAsyncById(long id, CancellationToken cancellationToken)
        {
            return _userDao.GetSingleAsync(t => t.Id.Equals(id), cancellationToken);
        }

        public Task<User> GetSingleAsyncByUserName(string userName, CancellationToken cancellationToken)
        {
            return _userDao.GetSingleAsync(t => t.UserName.ToLower().Equals(userName.ToLower()), cancellationToken);
        }

       
    }
}