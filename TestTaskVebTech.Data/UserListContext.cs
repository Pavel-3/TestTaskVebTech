using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using TestTaskVebTech.Data.Entities;
namespace TestTaskVebTech.Data
{
    public class UserListContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleUser> RoleUsers { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-D87V24D;Database=TestTaskDB;Trusted_Connection=True;encrypt=false");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>()
                .Property(role => role.RoleName)
                .HasConversion(
                    roleEnum => roleEnum.ToString(),
                    roleString => Enum.Parse<RoleName>(roleString, true));
            modelBuilder.Entity<Role>().HasData(
                new Role() { Id = 1, RoleName = RoleName.User },
                new Role() { Id = 2, RoleName = RoleName.Admin },
                new Role() { Id = 3, RoleName = RoleName.Support },
                new Role() { Id = 4, RoleName = RoleName.SuperAdmin });
        }
        public UserListContext(DbContextOptions<UserListContext> options) : base(options) { }
    }
}