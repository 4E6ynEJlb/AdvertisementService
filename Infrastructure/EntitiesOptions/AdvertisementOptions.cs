using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntitiesOptions
{
    internal static class AdvertisementOptions
    {
        internal static void ApplyAdvertisementOptions(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Advertisement>().HasKey(a => a.Id);
            modelBuilder.Entity<Advertisement>().HasIndex(a => a.Id).IsUnique();
            modelBuilder.Entity<Advertisement>().HasIndex(a => a.WillBeDeleted);
            modelBuilder.Entity<Advertisement>().Property(a => a.Id).IsRequired();
            modelBuilder.Entity<Advertisement>().Property(a => a.Number).IsRequired();
            modelBuilder.Entity<Advertisement>().Property(a => a.UserId).IsRequired();
            modelBuilder.Entity<Advertisement>().Property(a => a.Text).HasMaxLength(512).IsRequired();
            modelBuilder.Entity<Advertisement>().Property(a => a.ImageName).HasMaxLength(36);
            modelBuilder.Entity<Advertisement>().Property(a => a.ImageContentType).HasMaxLength(24);
            modelBuilder.Entity<Advertisement>().Property(a => a.AlreadyRated).IsRequired();
            modelBuilder.Entity<Advertisement>().Property(a => a.Created).IsRequired();
            modelBuilder.Entity<Advertisement>().Property(a => a.WillBeDeleted).IsRequired();            
        }
    }
}
