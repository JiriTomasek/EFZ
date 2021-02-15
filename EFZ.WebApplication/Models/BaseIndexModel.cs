using EFZ.WebApplication.Models.AccountViewModels;

namespace EFZ.WebApplication.Models
{
    public abstract class BaseIndexModel
    {
       public string Title { get; set; }
       public string ActiveController { get; set; }
       public string ActiveAction { get; set; }


    }
}