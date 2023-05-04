using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IS4.Models
{
    public class AppUser : IdentityUser
    {
        public string LogIn { get; set; }     //Login need for enter in Account
        public string FullName { get; set; }  //Additional Information about User
    }
}
