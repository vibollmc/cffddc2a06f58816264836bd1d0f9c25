using System.Web.Configuration;

namespace DLTD.Web.Main.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateInitial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.T_DangNhap",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TenDangNhap = c.String(maxLength: 50),
                        MatKhau = c.String(maxLength: 255),
                        Ma = c.String(maxLength: 10),
                        Ten = c.String(maxLength: 50),
                        GioiTinh = c.Int(nullable: false),
                        NgaySinh = c.DateTime(),
                        Email = c.String(maxLength: 50),
                        SoDienThoai = c.String(maxLength: 20),
                        UrlImage = c.String(maxLength: 512),
                        IdDonVi = c.Int(),
                        TrangThai = c.Int(nullable: false),
                        DangNhapTu = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.T_DonViTrucThuoc", t => t.IdDonVi)
                .Index(t => t.IdDonVi);
            
            CreateTable(
                "dbo.T_DonViTrucThuoc",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        ParentId = c.Int(),
                        Ma = c.String(maxLength: 20),
                        Ten = c.String(maxLength: 512),
                        Level = c.Int(),
                        TrangThai = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.T_VanBanChiDao",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Int(),
                        NgayDen = c.DateTime(),
                        SoDen = c.String(maxLength: 20),
                        TrichYeuNoiDung = c.String(maxLength: 1024),
                        NoiGui = c.String(maxLength: 512),
                        KyHieu = c.String(maxLength: 20),
                        YKienChiDao = c.String(maxLength: 2048),
                        ThoiHanXuLy = c.DateTime(),
                        TrangThai = c.Int(nullable: false),
                        IdVanBan = c.Long(),
                        NgayTao = c.DateTime(),
                        IdDonVi = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.T_DonVi", t => t.IdDonVi)
                .ForeignKey("dbo.T_DangNhap", t => t.UserId)
                .Index(t => t.IdDonVi)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.T_DonVi",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        IdKhoi = c.Int(),
                        Ma = c.String(maxLength: 20),
                        Ten = c.String(maxLength: 512),
                        DiaChi = c.String(maxLength: 512),
                        DienThoai = c.String(maxLength: 20),
                        Fax = c.String(maxLength: 20),
                        Email = c.String(maxLength: 255),
                        EmailVbdt = c.String(maxLength: 255),
                        TrangThai = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.T_Khoi", t => t.IdKhoi)
                .Index(t => t.IdKhoi);
            
            CreateTable(
                "dbo.T_Khoi",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        KyHieu = c.String(maxLength: 20),
                        Ten = c.String(maxLength: 100),
                        MacDinh = c.Boolean(),
                        TrangThai = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.T_FileDinhKem",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(maxLength: 255),
                        Url = c.String(maxLength: 512),
                        CreatedAt = c.DateTime(),
                        IdVanBanChiDao = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.T_VanBanChiDao", t => t.IdVanBanChiDao)
                .Index(t => t.IdVanBanChiDao);

            Sql("insert into DLTD.dbo.T_Khoi select a.intid, a.strkyhieu, a.strtenkhoi, a.IsDefault, 1 from " + WebConfigurationManager.AppSettings["DBQLVBNAME"] + ".dbo.Khoiphathanh a");
            Sql("insert into DLTD.dbo.T_DonVi select a.intid, a.intidkhoi, a.strmatochucdoitac, a.strtentochucdoitac, a.strdiachi, a.strphone, a.strfax, a.stremail, a.stremailvbdt, 1 from " + WebConfigurationManager.AppSettings["DBQLVBNAME"] + ".dbo.Tochucdoitac a");
            Sql("insert into dltd.dbo.T_donvitructhuoc select Id, ParentId, strmadonvi, strtendonvi, intlevel, inttrangthai from [" + WebConfigurationManager.AppSettings["DBQLVBNAME"] + "].[dbo].[Donvitructhuoc]");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.T_VanBanChiDao", "UserId", "dbo.T_DangNhap");
            DropForeignKey("dbo.T_FileDinhKem", "IdVanBanChiDao", "dbo.T_VanBanChiDao");
            DropForeignKey("dbo.T_VanBanChiDao", "IdDonVi", "dbo.T_DonVi");
            DropForeignKey("dbo.T_DonVi", "IdKhoi", "dbo.T_Khoi");
            DropForeignKey("dbo.T_DangNhap", "IdDonVi", "dbo.T_DonViTrucThuoc");
            DropIndex("dbo.T_VanBanChiDao", new[] { "UserId" });
            DropIndex("dbo.T_FileDinhKem", new[] { "IdVanBanChiDao" });
            DropIndex("dbo.T_VanBanChiDao", new[] { "IdDonVi" });
            DropIndex("dbo.T_DonVi", new[] { "IdKhoi" });
            DropIndex("dbo.T_DangNhap", new[] { "IdDonVi" });
            DropTable("dbo.T_FileDinhKem");
            DropTable("dbo.T_Khoi");
            DropTable("dbo.T_DonVi");
            DropTable("dbo.T_VanBanChiDao");
            DropTable("dbo.T_DonViTrucThuoc");
            DropTable("dbo.T_DangNhap");
        }
    }
}
