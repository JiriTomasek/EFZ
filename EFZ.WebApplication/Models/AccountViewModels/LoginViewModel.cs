using System.ComponentModel.DataAnnotations;
using System.Security.Policy;
using EFZ.Resources;

namespace EFZ.WebApplication.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "valEmailRequired")]
        [EmailAddress(ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "valEmailAddressCorect")]
        [Display(ResourceType = typeof(Labels), Name = "valEmail")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "valPasswordRequired")]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Labels), Name = "valPassword")]
        public string Password { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "valRememberMe")]
        public bool RememberMe { get; set; }
    }
}