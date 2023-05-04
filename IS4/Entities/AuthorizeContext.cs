using IS4.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IS4.Entities
{
    public class AuthorizeContext : IdentityDbContext<AppUser>
    {
        public AuthorizeContext(DbContextOptions<AuthorizeContext> options) : base(options)
        {

        }
    }
}
