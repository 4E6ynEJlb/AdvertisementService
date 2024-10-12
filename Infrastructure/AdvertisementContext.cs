using Domain.Models.Entities;
using Infrastructure.EntitiesOptions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class AdvertisementContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Advertisement> Advertisements { get; set; }
        public AdvertisementContext() { }
        public AdvertisementContext(DbContextOptions<AdvertisementContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyUserOptions();
            modelBuilder.ApplyAdvertisementOptions();
            base.OnModelCreating(modelBuilder);
        }
    }
}
