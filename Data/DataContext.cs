using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StatusApi.Models;

namespace water_level_dotnetcore_api.Data
{
    public class DataContext : IdentityDbContext
    {
        public DbSet<SensorStatus> SensorStatusItems { get; set; } = null!;

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {


        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SensorStatus>()
                .HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.UserId)
                .IsRequired();
        }
    }
}
