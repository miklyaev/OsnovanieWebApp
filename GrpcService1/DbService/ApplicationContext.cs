using GrpcService1.DbService.Model;
using Microsoft.EntityFrameworkCore;

namespace GrpcService1.DbService
{
    public class ApplicationContext : DbContext
    {
        public DbSet<TUser> Users => Set<TUser>();
        public DbSet<TRole> Roles => Set<TRole>();
        public DbSet<TUserRole> UserRoles => Set<TUserRole>();
        public DbSet<TRoleGroup> RoleGroups => Set<TRoleGroup>();
        public DbSet<TGroup> Groups => Set<TGroup>();

        //public ApplicationContext() => Database.EnsureCreated();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=LearnDb;Username=postgres;Password=dasha2009");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TUser>()
            .HasMany<TUserRole>()
            .WithOne(s => s.User)
            //.HasForeignKey(s => s.User.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}