using ByteBrew_Coffee_Roasters.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ByteBrew_Coffee_Roasters.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, Guid>
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductReview> ProductReviews { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(entity => entity.ToTable(name: "Users"));
            builder.Entity<Role>(entity => entity.ToTable(name: "Roles"));
            builder.Entity<IdentityUserRole<Guid>>(entity =>
                entity.ToTable(name: "UserRoles").HasKey(x => x.UserId));
            builder.Entity<IdentityUserClaim<Guid>>(entity =>
                entity.ToTable(name: "UserClaim"));
            builder.Entity<IdentityUserLogin<Guid>>(entity =>
                entity.ToTable("UserLogins").HasKey(x => x.UserId));
            builder.Entity<IdentityUserToken<Guid>>(entity =>
            {
                entity.ToTable("UserTokens").HasKey(x => x.UserId);
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });
            });
            builder.Entity<IdentityRoleClaim<Guid>>(entity =>
                entity.ToTable("RoleClaims"));
        }
    }
}
