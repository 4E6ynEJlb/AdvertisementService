using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntitiesOptions
{
    internal static class UserOptions
    {
        internal static void ApplyUserOptions(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<User>().HasIndex(u => u.Id).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.Login).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.Password).IsUnique();
            modelBuilder.Entity<User>().HasMany(u => u.Advertisements).WithOne(a => a.User).HasForeignKey(a => a.UserId);
            modelBuilder.Entity<User>().Property(u => u.Id).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.Login).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.Password).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.Name).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.IsAdmin).IsRequired(); 
        }
    }
}
