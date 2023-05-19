using System.ComponentModel.DataAnnotations;

namespace MakeYouPro.Bank.Dal.Auth.Models
{
    public class UserDal
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
