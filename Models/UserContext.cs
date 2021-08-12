using Microsoft.EntityFrameworkCore;

namespace LoginReg.Models
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<LoginUser> LoginUsers { get; set; }
    }
}