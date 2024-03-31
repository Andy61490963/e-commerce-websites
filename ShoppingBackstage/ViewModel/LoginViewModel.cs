using System.ComponentModel.DataAnnotations;

namespace ShoppingBackstage.ViewModel
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "帳號")]
        public string account { get; set; }

        [Required]
        [Display(Name = "密碼")]
        public string password { get; set; }
    }
}
