using System.Data.Entity;
using DLTD.Web.Main.Models.QLVB;

namespace DLTD.Web.Main.DAL
{
    public class QlvbDatabase : DbContext
    {
        static QlvbDatabase()
        {
            Database.SetInitializer<QlvbDatabase>(null); // must be turned off before mini profiler runs
        }

        #region DbSet
        public DbSet<Canbo> Canbos { get; set; }
        public DbSet<Donvitructhuoc> Donvitructhuocs { get; set; }
        public DbSet<Khoiphathanh> Khoiphathanhs { get; set; }
        public DbSet<Tochucdoitac> Tochucdoitacs { get; set; }

        #endregion DbSet


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Canbo>().ToTable("Canbo").HasKey(m => m.intid);
            modelBuilder.Entity<Donvitructhuoc>().ToTable("Donvitructhuoc").HasKey(m => m.Id);
            modelBuilder.Entity<Khoiphathanh>().ToTable("Khoiphathanh").HasKey(m => m.intid);
            modelBuilder.Entity<Tochucdoitac>().ToTable("Tochucdoitac").HasKey(m => m.intid);
        }

    }
}
