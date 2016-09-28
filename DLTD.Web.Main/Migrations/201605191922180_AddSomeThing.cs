using System.Web.Configuration;

namespace DLTD.Web.Main.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSomeThing : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.T_VanBanChiDao", "UserId", "dbo.T_DangNhap");
            DropIndex("dbo.T_VanBanChiDao", new[] { "UserId" });
            AddColumn("dbo.T_DangNhap", "NhomNguoiDung", c => c.Int());
            AddColumn("dbo.T_Khoi", "NguonChiDao", c => c.Boolean());
            AddColumn("dbo.T_VanBanChiDao", "IdNguonChiDao", c => c.Int());
            AddColumn("dbo.T_VanBanChiDao", "IdNguoiChiDao", c => c.Int(nullable: false));
            AddColumn("dbo.T_VanBanChiDao", "IdNguoiTheoDoi", c => c.Int(nullable: false));
            AlterColumn("dbo.T_VanBanChiDao", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.T_VanBanChiDao", "IdNguoiChiDao");
            CreateIndex("dbo.T_VanBanChiDao", "IdNguoiTheoDoi");
            CreateIndex("dbo.T_VanBanChiDao", "IdNguonChiDao");
            CreateIndex("dbo.T_VanBanChiDao", "UserId");
            AddForeignKey("dbo.T_VanBanChiDao", "IdNguoiChiDao", "dbo.T_DangNhap", "Id");
            AddForeignKey("dbo.T_VanBanChiDao", "IdNguoiTheoDoi", "dbo.T_DangNhap", "Id");
            AddForeignKey("dbo.T_VanBanChiDao", "IdNguonChiDao", "dbo.T_Khoi", "Id");
            AddForeignKey("dbo.T_VanBanChiDao", "UserId", "dbo.T_DangNhap", "Id");
            DropColumn("dbo.T_VanBanChiDao", "NguonChiDao");

            Sql("INSERT INTO [DLTD].[dbo].[T_DangNhap]([TenDangNhap],[Ma],[Ten],[GioiTinh],[Email],[SoDienThoai],[IdDonVi],[TrangThai],[DangNhapTu],[NhomNguoiDung])" +
                " SELECT [strusername],[strmacanbo],[strhoten],[intgioitinh],[stremail],[strdienthoai],[intdonvi],1,1,[intnhomquyen]" +
                " FROM [" + WebConfigurationManager.AppSettings["DBQLVBNAME"] + "].[dbo].[Canbo] WHERE strusername is not null and strusername != '' and inttrangthai = 1");
        }
        
        public override void Down()
        {
            AddColumn("dbo.T_VanBanChiDao", "NguonChiDao", c => c.Int(nullable: false));
            DropForeignKey("dbo.T_VanBanChiDao", "UserId", "dbo.T_DangNhap");
            DropForeignKey("dbo.T_VanBanChiDao", "IdNguonChiDao", "dbo.T_Khoi");
            DropForeignKey("dbo.T_VanBanChiDao", "IdNguoiTheoDoi", "dbo.T_DangNhap");
            DropForeignKey("dbo.T_VanBanChiDao", "IdNguoiChiDao", "dbo.T_DangNhap");
            DropIndex("dbo.T_VanBanChiDao", new[] { "UserId" });
            DropIndex("dbo.T_VanBanChiDao", new[] { "IdNguonChiDao" });
            DropIndex("dbo.T_VanBanChiDao", new[] { "IdNguoiTheoDoi" });
            DropIndex("dbo.T_VanBanChiDao", new[] { "IdNguoiChiDao" });
            AlterColumn("dbo.T_VanBanChiDao", "UserId", c => c.Int());
            DropColumn("dbo.T_VanBanChiDao", "IdNguoiTheoDoi");
            DropColumn("dbo.T_VanBanChiDao", "IdNguoiChiDao");
            DropColumn("dbo.T_VanBanChiDao", "IdNguonChiDao");
            DropColumn("dbo.T_Khoi", "NguonChiDao");
            DropColumn("dbo.T_DangNhap", "NhomNguoiDung");
            CreateIndex("dbo.T_VanBanChiDao", "UserId");
            AddForeignKey("dbo.T_VanBanChiDao", "UserId", "dbo.T_DangNhap", "Id");
        }
    }
}
