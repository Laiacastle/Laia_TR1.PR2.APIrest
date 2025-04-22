using System.ComponentModel.DataAnnotations;

namespace Client_Laia_T1.PR2.APIrest.Models
{
    public class LoginClient
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
