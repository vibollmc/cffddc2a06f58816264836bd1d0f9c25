using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using QLVB.Domain.Entities;

namespace QLVB.DAL
{
    public class QLVBDatabase : DbContext
    {
        static QLVBDatabase()
        {
            Database.SetInitializer<QLVBDatabase>(null); // must be turned off before mini profiler runs
        }

        #region DbSet
        public DbSet<Canbo> Canbos { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Config> Configs { get; set; }
        public DbSet<Quyen> Quyens { get; set; }
        public DbSet<NhomQuyen> NhomQuyens { get; set; }
        public DbSet<QuyenNhomQuyen> QuyenNhomQuyens { get; set; }
        public DbSet<UyQuyen> UyQuyens { get; set; }
        public DbSet<Home> Homes { get; set; }
        public DbSet<Donvitructhuoc> Donvitructhuocs { get; set; }
        public DbSet<Diachiluutru> Diachiluutrus { get; set; }
        public DbSet<NLogError> NlogErrors { get; set; }
        public DbSet<Nhatky> Nhatkys { get; set; }
        public DbSet<Chucdanh> Chucdanhs { get; set; }
        public DbSet<MotaTruong> MotaTruongs { get; set; }
        public DbSet<PhanloaiVanban> PhanloaiVanbans { get; set; }
        public DbSet<PhanloaiTruong> PhanloaiTruongs { get; set; }
        public DbSet<Linhvuc> Linhvucs { get; set; }
        public DbSet<Tinhchatvanban> Tinhchatvanbans { get; set; }
        public DbSet<Khoiphathanh> Khoiphathanhs { get; set; }
        public DbSet<Tochucdoitac> Tochucdoitacs { get; set; }
        public DbSet<Systems> Systems { get; set; }
        public DbSet<SoVanban> SoVanbans { get; set; }
        public DbSet<SovbKhoiph> SovbKhoiphs { get; set; }
        public DbSet<Vanbanden> Vanbandens { get; set; }
        public DbSet<CategoryVanban> CategoryVanban { get; set; }
        public DbSet<Vanbandi> Vanbandis { get; set; }
        public DbSet<AttachVanban> AttachVanbans { get; set; }
        public DbSet<Baocao> Baocaos { get; set; }
        public DbSet<MailContent> MailContents { get; set; }
        public DbSet<Vanbandenmail> Vanbandenmails { get; set; }
        public DbSet<Hosovanban> Hosovanbans { get; set; }
        public DbSet<Hosovanbanlienquan> Hosovanbanlienquans { get; set; }
        public DbSet<Hosocongviec> Hosocongviecs { get; set; }
        public DbSet<Doituongxuly> Doituongxulys { get; set; }
        public DbSet<Hosoykienxuly> Hosoykienxulys { get; set; }
        public DbSet<ChitietVanbanden> Chitietvanbandens { get; set; }
        public DbSet<ChitietVanbandi> Chitietvanbandis { get; set; }
        //public DbSet<UyQuyen> UyQuyens { get; set; }
        public DbSet<Hoibaovanban> Hoibaovanbans { get; set; }
        public DbSet<ChitietHoso> ChitietHosos { get; set; }
        public DbSet<VanbandenCanbo> VanbandenCanbos { get; set; }
        public DbSet<VanbandiCanbo> VanbandiCanbos { get; set; }
        public DbSet<MailInbox> MailInboxs { get; set; }
        public DbSet<MailOutbox> MailOutboxs { get; set; }
        public DbSet<AttachMail> AttachMails { get; set; }
        public DbSet<GuiVanban> GuiVanbans { get; set; }
        public DbSet<AttachHoso> AttachHosos { get; set; }
        public DbSet<Phieutrinh> Phieutrinhs { get; set; }
        public DbSet<TonghopCanbo> TonghopCanbos { get; set; }

        public DbSet<Connection> Connections { get; set; }
        public DbSet<PhanloaiQuytrinh> PhanloaiQuytrinhs { get; set; }
        public DbSet<Quytrinh> Quytrinhs { get; set; }
        public DbSet<QuytrinhNode> QuytrinhNodes { get; set; }
        public DbSet<QuytrinhConnection> QuytrinhConnections { get; set; }

        public DbSet<QuytrinhXuly> QuytrinhXulys { get; set; }
        public DbSet<HosoQuytrinhXuly> HosoQuytrinhxulys { get; set; }
        public DbSet<QuytrinhVersion> QuytrinhVersions { get; set; }
        public DbSet<HosoQuytrinh> HosoQuytrinhs { get; set; }

        public DbSet<Tuychon> Tuychons { get; set; }
        public DbSet<TuychonCanbo> TuychonCanbos { get; set; }


        //========view========================================
        public DbSet<Tinhtrangxuly> Tinhtrangxulys { get; set; }
        public DbSet<TinhTrangQuytrinh> TinhtrangQuytrinhs { get; set; }

        #endregion DbSet


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Config>().ToTable("Config").HasKey(m => m.intid);
            modelBuilder.Entity<Canbo>().ToTable("Canbo").HasKey(m => m.intid);

            modelBuilder.Entity<Menu>().ToTable("Menu").HasKey(m => m.Id);
            modelBuilder.Entity<Menu>()
                        .HasOptional(c => c.ParentMenu)
                        .WithMany(c => c.SubMenu).HasForeignKey(c => c.ParentId);
            //.Map(m => m.MapKey("Id"));


            modelBuilder.Entity<Quyen>().ToTable("Quyen").HasKey(m => m.intid);
            modelBuilder.Entity<NhomQuyen>().ToTable("NhomQuyen").HasKey(m => m.intid);
            modelBuilder.Entity<QuyenNhomQuyen>().ToTable("QuyenNhomQuyen")
                //.HasKey(m => m.intidquyen)
                //.HasKey(m => m.intidnhomquyen);
                        .HasKey(m => new { m.intidnhomquyen, m.intidquyen });
            modelBuilder.Entity<Home>().ToTable("Home").HasKey(m => m.intid);
            modelBuilder.Entity<UyQuyen>().ToTable("UyQuyen").HasKey(m => m.intid);

            modelBuilder.Entity<Donvitructhuoc>().ToTable("Donvitructhuoc").HasKey(m => m.Id);
            modelBuilder.Entity<Diachiluutru>().ToTable("Diachiluutru").HasKey(m => m.Id);

            modelBuilder.Entity<NLogError>().ToTable("NLog_Error").HasKey(m => m.Id);
            modelBuilder.Entity<Nhatky>().ToTable("Nhatky").HasKey(m => m.Id);
            modelBuilder.Entity<Chucdanh>().ToTable("Chucdanh").HasKey(m => m.intid);
            modelBuilder.Entity<MotaTruong>().ToTable("MotaTruong").HasKey(m => m.intid);
            modelBuilder.Entity<PhanloaiVanban>().ToTable("PhanloaiVanban").HasKey(m => m.intid);
            modelBuilder.Entity<PhanloaiTruong>().ToTable("PhanloaiTruong").HasKey(m => m.intid);
            modelBuilder.Entity<Linhvuc>().ToTable("Linhvuc").HasKey(m => m.intid);
            modelBuilder.Entity<Tinhchatvanban>().ToTable("Tinhchatvanban").HasKey(m => m.intid);
            modelBuilder.Entity<Khoiphathanh>().ToTable("Khoiphathanh").HasKey(m => m.intid);
            modelBuilder.Entity<Tochucdoitac>().ToTable("Tochucdoitac").HasKey(m => m.intid);
            modelBuilder.Entity<Systems>().ToTable("Systems").HasKey(m => m.intid);
            modelBuilder.Entity<SoVanban>().ToTable("SoVanban").HasKey(m => m.intid);
            modelBuilder.Entity<SovbKhoiph>().ToTable("SovbKhoiph").HasKey(m => m.intid);
            modelBuilder.Entity<Vanbanden>().ToTable("Vanbanden").HasKey(m => m.intid);
            modelBuilder.Entity<CategoryVanban>().ToTable("CategoryVanban").HasKey(m => m.intid);
            modelBuilder.Entity<Vanbandi>().ToTable("Vanbandi").HasKey(m => m.intid);
            modelBuilder.Entity<AttachVanban>().ToTable("AttachVanban").HasKey(m => m.intid);
            modelBuilder.Entity<Baocao>().ToTable("Baocao").HasKey(m => m.intid);
            modelBuilder.Entity<MailContent>().ToTable("MailContent").HasKey(m => m.intid);
            modelBuilder.Entity<Vanbandenmail>().ToTable("Vanbandenmail").HasKey(m => m.intid);

            modelBuilder.Entity<Hosovanban>().ToTable("Hosovanban").HasKey(m => m.intid);
            modelBuilder.Entity<Hosovanbanlienquan>().ToTable("Hosovanbanlienquan").HasKey(m => m.intid);

            modelBuilder.Entity<Hosocongviec>().ToTable("Hosocongviec").HasKey(m => m.intid);
            modelBuilder.Entity<Doituongxuly>().ToTable("Doituongxuly").HasKey(m => m.intid);
            modelBuilder.Entity<Hosoykienxuly>().ToTable("Hosoykienxuly").HasKey(m => m.intid);
            modelBuilder.Entity<ChitietVanbanden>().ToTable("ChitietVanbanden").HasKey(m => m.intid);
            modelBuilder.Entity<ChitietVanbandi>().ToTable("ChitietVanbandi").HasKey(m => m.intid);
            modelBuilder.Entity<Hoibaovanban>().ToTable("Hoibaovanban").HasKey(m => m.intid);
            modelBuilder.Entity<ChitietHoso>().ToTable("ChitietHoso").HasKey(m => m.intid);
            modelBuilder.Entity<VanbandenCanbo>().ToTable("VanbandenCanbo")
                                                .HasKey(m => new { m.intidcanbo, m.intidvanban });
            //.HasKey(m => m.intidcanbo)
            //.HasKey(m => m.intidvanban);
            modelBuilder.Entity<VanbandiCanbo>().ToTable("VanbandiCanbo")
                                                .HasKey(m => new { m.intidcanbo, m.intidvanban });
            //.HasKey(m => m.intidcanbo)
            //.HasKey(m => m.intidvanban);
            modelBuilder.Entity<MailInbox>().ToTable("MailInbox").HasKey(m => m.intid);
            modelBuilder.Entity<MailOutbox>().ToTable("MailOutbox").HasKey(m => m.intid);
            modelBuilder.Entity<AttachMail>().ToTable("AttachMail").HasKey(m => m.intid);

            modelBuilder.Entity<GuiVanban>()//.Property(f => f.strngaygui).HasColumnType("smalldatetime")
                .ToTable("GuiVanban")
                .HasKey(m => m.intid);
            //modelBuilder.Entity<GuiVanban>().Property(f => f.strngaygui).HasColumnType("smalldatetime");

            modelBuilder.Entity<AttachHoso>().ToTable("AttachHoso").HasKey(m => m.intid);
            modelBuilder.Entity<Phieutrinh>().ToTable("Phieutrinh").HasKey(m => m.intid);
            modelBuilder.Entity<TonghopCanbo>().ToTable("TonghopCanbo").HasKey(m => m.intid);

            modelBuilder.Entity<Connection>().ToTable("Connection").HasKey(m => m.Id);

            modelBuilder.Entity<PhanloaiQuytrinh>().ToTable("PhanloaiQuytrinh").HasKey(m => m.intid);
            modelBuilder.Entity<Quytrinh>().ToTable("Quytrinh").HasKey(m => m.intid);
            modelBuilder.Entity<QuytrinhNode>().ToTable("QuytrinhNode").HasKey(m => m.intid);
            modelBuilder.Entity<QuytrinhConnection>().ToTable("QuytrinhConnection").HasKey(m => m.intid);
            modelBuilder.Entity<QuytrinhXuly>().ToTable("QuytrinhXuly").HasKey(m => m.intid);
            modelBuilder.Entity<HosoQuytrinhXuly>().ToTable("HosoQuytrinhXuly").HasKey(m => m.intid);
            modelBuilder.Entity<QuytrinhVersion>().ToTable("QuytrinhVersion").HasKey(m => m.intid);
            modelBuilder.Entity<HosoQuytrinh>().ToTable("HosoQuytrinh").HasKey(m => m.intid);

            modelBuilder.Entity<Tuychon>().ToTable("Tuychon").HasKey(m => m.intid);
            modelBuilder.Entity<TuychonCanbo>().ToTable("TuychonCanbo").HasKey(m => m.intid);

            // view
            modelBuilder.Entity<Tinhtrangxuly>().ToTable("vTinhtrangxuly").HasKey(m => m.intidvanban);
            modelBuilder.Entity<TinhTrangQuytrinh>().ToTable("vTinhtrangQuytrinh").HasKey(m => m.intidvanban);


        }

    }
}
