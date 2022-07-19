using System.Configuration;
using Microsoft.EntityFrameworkCore;

namespace KundenVerzeichnis.Models
{
    /// <summary>
    /// Contains the DBContext instance for the database
    /// </summary>
    class Context : DbContext
    {
        
        public DbSet<Bill> Bills { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Patient> Patients{ get; set; }
        public DbSet<Treatment> Treatments { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        }
    }
}
