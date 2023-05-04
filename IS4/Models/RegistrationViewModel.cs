using System.ComponentModel.DataAnnotations;

namespace IS4.Models
{
    public class RegistrationViewModel
    {
        [Required]
        [MaxLength(40), MinLength(2)]
        public string LogIn { get; set; }      //This is need for enter in account
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [MaxLength(40), MinLength(2)]
        public string PersonFullName { get; set; }
        [Required]
        [MaxLength(25), MinLength(4)]
        public string UserName { get; set; }   //This is need for public name of user
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string NumberPhone { get; set; }
        //[Required]
        public string ReturnUrl { get; set; }
    }
}
