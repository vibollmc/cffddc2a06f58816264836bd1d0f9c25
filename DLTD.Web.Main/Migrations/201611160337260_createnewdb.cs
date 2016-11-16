namespace DLTD.Web.Main.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createnewdb : DbMigration
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
                        NhomNguoiDung = c.Int(),
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
                "dbo.T_TinhHinhPhoiHop",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        IdDonViPhoiHop = c.Int(),
                        NoiDungThucHien = c.String(maxLength: 2048),
                        NgayXuLy = c.DateTime(),
                        UserId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.T_DonViPhoiHop", t => t.IdDonViPhoiHop)
                .ForeignKey("dbo.T_DangNhap", t => t.UserId)
                .Index(t => t.IdDonViPhoiHop)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.T_DonViPhoiHop",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdVanBan = c.Long(),
                        IdDonvi = c.Int(),
                        TrangThai = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.T_DonVi", t => t.IdDonvi)
                .ForeignKey("dbo.T_VanBanChiDao", t => t.IdVanBan)
                .Index(t => t.IdDonvi)
                .Index(t => t.IdVanBan);
            
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
                        NguonChiDao = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.T_VanBanChiDao",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Trichyeu = c.String(),
                        SoKH = c.String(maxLength: 512),
                        YKienChiDao = c.String(maxLength: 2048),
                        ThoiHanXuLy = c.DateTime(),
                        Ngayky = c.DateTime(),
                        TrangThai = c.Int(nullable: false),
                        IdVanBan = c.Long(),
                        NgayTao = c.DateTime(),
                        NgayHoanThanh = c.DateTime(),
                        IdDonVi = c.Int(),
                        DoKhan = c.Int(nullable: false),
                        IdNguonChiDao = c.Int(),
                        IdNguoiChiDao = c.Int(nullable: false),
                        IdNguoiTheoDoi = c.Int(nullable: false),
                        IsTralai = c.Int(),
                        LydoTraLai = c.String(),
                        NgayTra = c.DateTime(),
                        GhiChu = c.String(),
                        TinhHinhThucHienNoiBo = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.T_DonVi", t => t.IdDonVi)
                .ForeignKey("dbo.T_DangNhap", t => t.IdNguoiChiDao)
                .ForeignKey("dbo.T_DangNhap", t => t.UserId)
                .ForeignKey("dbo.T_DangNhap", t => t.IdNguoiTheoDoi)
                .ForeignKey("dbo.T_Khoi", t => t.IdNguonChiDao)
                .Index(t => t.IdDonVi)
                .Index(t => t.IdNguoiChiDao)
                .Index(t => t.UserId)
                .Index(t => t.IdNguoiTheoDoi)
                .Index(t => t.IdNguonChiDao);
            
            CreateTable(
                "dbo.T_FileDinhKem",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(maxLength: 255),
                        Url = c.String(maxLength: 512),
                        CreatedAt = c.DateTime(),
                        IdVanBanChiDao = c.Long(),
                        IdTinhHinhThucHien = c.Long(),
                        IdTinhHinhPhoiHop = c.Long(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.T_VanBanChiDao", t => t.IdVanBanChiDao)
                .ForeignKey("dbo.T_TinhHinhThucHien", t => t.IdTinhHinhThucHien)
                .ForeignKey("dbo.T_TinhHinhPhoiHop", t => t.IdTinhHinhPhoiHop)
                .Index(t => t.IdVanBanChiDao)
                .Index(t => t.IdTinhHinhThucHien)
                .Index(t => t.IdTinhHinhPhoiHop);
            
            CreateTable(
                "dbo.T_TinhHinhThucHien",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        IdVanBanChiDao = c.Long(),
                        NoiDungThucHien = c.String(maxLength: 2048),
                        NgayBaoCao = c.DateTime(),
                        UserId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.T_DangNhap", t => t.UserId)
                .ForeignKey("dbo.T_VanBanChiDao", t => t.IdVanBanChiDao)
                .Index(t => t.UserId)
                .Index(t => t.IdVanBanChiDao);
            
            CreateTable(
                "dbo.T_MarkedDatabaseChange",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ConnectionString = c.String(maxLength: 512),
                        IsSyncData = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.T_TinhHinhPhoiHop", "UserId", "dbo.T_DangNhap");
            DropForeignKey("dbo.T_FileDinhKem", "IdTinhHinhPhoiHop", "dbo.T_TinhHinhPhoiHop");
            DropForeignKey("dbo.T_TinhHinhPhoiHop", "IdDonViPhoiHop", "dbo.T_DonViPhoiHop");
            DropForeignKey("dbo.T_DonViPhoiHop", "IdVanBan", "dbo.T_VanBanChiDao");
            DropForeignKey("dbo.T_DonViPhoiHop", "IdDonvi", "dbo.T_DonVi");
            DropForeignKey("dbo.T_DonVi", "IdKhoi", "dbo.T_Khoi");
            DropForeignKey("dbo.T_TinhHinhThucHien", "IdVanBanChiDao", "dbo.T_VanBanChiDao");
            DropForeignKey("dbo.T_TinhHinhThucHien", "UserId", "dbo.T_DangNhap");
            DropForeignKey("dbo.T_FileDinhKem", "IdTinhHinhThucHien", "dbo.T_TinhHinhThucHien");
            DropForeignKey("dbo.T_VanBanChiDao", "IdNguonChiDao", "dbo.T_Khoi");
            DropForeignKey("dbo.T_VanBanChiDao", "IdNguoiTheoDoi", "dbo.T_DangNhap");
            DropForeignKey("dbo.T_VanBanChiDao", "UserId", "dbo.T_DangNhap");
            DropForeignKey("dbo.T_VanBanChiDao", "IdNguoiChiDao", "dbo.T_DangNhap");
            DropForeignKey("dbo.T_FileDinhKem", "IdVanBanChiDao", "dbo.T_VanBanChiDao");
            DropForeignKey("dbo.T_VanBanChiDao", "IdDonVi", "dbo.T_DonVi");
            DropForeignKey("dbo.T_DangNhap", "IdDonVi", "dbo.T_DonViTrucThuoc");
            DropIndex("dbo.T_TinhHinhPhoiHop", new[] { "UserId" });
            DropIndex("dbo.T_FileDinhKem", new[] { "IdTinhHinhPhoiHop" });
            DropIndex("dbo.T_TinhHinhPhoiHop", new[] { "IdDonViPhoiHop" });
            DropIndex("dbo.T_DonViPhoiHop", new[] { "IdVanBan" });
            DropIndex("dbo.T_DonViPhoiHop", new[] { "IdDonvi" });
            DropIndex("dbo.T_DonVi", new[] { "IdKhoi" });
            DropIndex("dbo.T_TinhHinhThucHien", new[] { "IdVanBanChiDao" });
            DropIndex("dbo.T_TinhHinhThucHien", new[] { "UserId" });
            DropIndex("dbo.T_FileDinhKem", new[] { "IdTinhHinhThucHien" });
            DropIndex("dbo.T_VanBanChiDao", new[] { "IdNguonChiDao" });
            DropIndex("dbo.T_VanBanChiDao", new[] { "IdNguoiTheoDoi" });
            DropIndex("dbo.T_VanBanChiDao", new[] { "UserId" });
            DropIndex("dbo.T_VanBanChiDao", new[] { "IdNguoiChiDao" });
            DropIndex("dbo.T_FileDinhKem", new[] { "IdVanBanChiDao" });
            DropIndex("dbo.T_VanBanChiDao", new[] { "IdDonVi" });
            DropIndex("dbo.T_DangNhap", new[] { "IdDonVi" });
            DropTable("dbo.T_MarkedDatabaseChange");
            DropTable("dbo.T_TinhHinhThucHien");
            DropTable("dbo.T_FileDinhKem");
            DropTable("dbo.T_VanBanChiDao");
            DropTable("dbo.T_Khoi");
            DropTable("dbo.T_DonVi");
            DropTable("dbo.T_DonViPhoiHop");
            DropTable("dbo.T_TinhHinhPhoiHop");
            DropTable("dbo.T_DonViTrucThuoc");
            DropTable("dbo.T_DangNhap");
        }
    }
}
