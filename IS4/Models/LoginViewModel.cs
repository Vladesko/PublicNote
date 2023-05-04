using System.ComponentModel.DataAnnotations;

namespace IS4.Models
{
    public class LoginViewModel
    {
        [Required]
        [MaxLength(50), MinLength(2)]
        public string LogIn { get; set; }       //This is for enter account 
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string UserName { get; set; }   //This is need for user.    (IdentityServer4 is is4)
                                               //User will write a login, but is4 dont know what is login.
                                               //Is4 find user by User Name.
                                               //Our program do User name is public name for user and login is for enter in account.
        [Required]
        public string ReturnUrl { get; set; }
    }
}
