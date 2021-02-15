using System.ComponentModel.DataAnnotations;
using EFZ.Resources;

namespace EFZ.WebApplication.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "valEmailRequired")]
        [EmailAddress]
        [Display(ResourceType = typeof(Labels), Name = "valEmail")]
        public string RegisterEmail { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "valPasswordRequired")]
        [StringLength(100, ErrorMessage = "{0} musí byt dlouhé {2} až {1} znaků.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Labels), Name = "valPassword")]
        public string RegisterPassword { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "valPasswordRequired")]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Labels), Name = "valConfirmPassword")]
        [Compare("RegisterPassword", ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName  = "valPasswordNotMatch")]
        public string ConfirmPassword { get; set; }
    }
}