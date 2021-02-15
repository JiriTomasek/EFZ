using System.Collections.Generic;
using EFZ.Entities.Entities;

namespace EFZ.WebApplication.Models.Home
{
    public class HomeIndexModel : BaseIndexModel
    {
        public HomeIndexModel()
        {
            //LoginViewModel = new LoginViewModel();
            //RegisterViewModel = new RegisterViewModel();
        }
        public List<User> Users { get; set; }
       
    }
}