using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using DLTD.Web.Main.Models;

namespace DLTD.Web.Main.DAL
{
    public sealed class MainDbContext : DbContext
    {
        public DbSet<Khoi> Khoi { get; set; }
        public DbSet<DonVi> DonVi { get; set; }
        public DbSet<DangNhap> DangNhap { get; set; }
        public DbSet<DonViTrucThuoc> DonViTrucThuoc { get; set; }
        public DbSet<VanBanChiDao> VanBanChiDao { get; set; }
        public DbSet<FileVanBanChiDao> FileVanBanChiDao { get; set; }
        public DbSet<FileTinhHinhPhoiHop> FileTinhHinhPhoiHop { get; set; }
        public DbSet<FileTinhHinhThucHien> FileTinhHinhThucHien { get; set; }
        public DbSet<FileDinhKem> FileDinhKem { get; set; }
        public DbSet<DonViPhoiHop> DonViPhoiHop { get; set; }
        public DbSet<TinhHinhThucHien> TinhHinhThucHien { get; set; }
        public DbSet<TinhHinhPhoiHop> TinhHinhPhoiHop { get; set; }

        public DbSet<MarkedDatabaseChange> MarkedDatabaseChange { get; set; }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<MainDbContext>(null);
        }

    }
}
