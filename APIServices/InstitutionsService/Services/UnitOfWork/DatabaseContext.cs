using Entities.Base;
using Entities.Institution;
using Entities.Institution.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace InstitutionsService.Services.UnitOfWork
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InstitutionAddressVM>(entity => { entity.HasKey(e => e.InstitutionId); });

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // get the configuration from the app settings
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // define the database to use
            //optionsBuilder.UseMySql(config.GetConnectionString("DefaultConnection"));
            optionsBuilder.UseMySql(config.GetConnectionString("DockerConnection"));
        }

        #region DB Views

        public DbSet<InstitutionAddressVM> InstitutionAddressVM { get; set; }

        #endregion

        #region DB Tables

        public DbSet<Institution> Institution { get; set; }
        public DbSet<InstitutionAddress> InstitutionAddress { get; set; }

        public DbSet<Setting> Settings { get; set; }

        #endregion
    }
}
