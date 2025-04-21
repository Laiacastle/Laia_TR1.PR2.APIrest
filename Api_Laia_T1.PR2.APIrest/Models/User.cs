using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api_Laia_T1.PR2.APIrest.Models
{
    [Table("User")]
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }

        public List<Game> Games { get; set; } = new List<Game>();
    }
}
