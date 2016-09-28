namespace DLTD.Web.Main.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Thaydoicadong : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.T_VanBanDi", "IdDonVi", "dbo.T_DonVi");
            DropForeignKey("dbo.T_DonViPhoiHop", "VanBanDi_Id", "dbo.T_VanBanDi");
            DropForeignKey("dbo.T_VanBanDi", "UserId", "dbo.T_DangNhap");
            DropForeignKey("dbo.T_TinhHinhThucHien", "VanBanDi_Id", "dbo.T_VanBanDi");
            DropForeignKey("dbo.T_FileDinhKem", "IdVanBanDi", "dbo.T_VanBanDi");
            DropIndex("dbo.T_VanBanDi", new[] { "IdDonVi" });
            DropIndex("dbo.T_DonViPhoiHop", new[] { "VanBanDi_Id" });
            DropIndex("dbo.T_VanBanDi", new[] { "UserId" });
            DropIndex("dbo.T_TinhHinhThucHien", new[] { "VanBanDi_Id" });
            DropIndex("dbo.T_FileDinhKem", new[] { "IdVanBanDi" });
            AddColumn("dbo.T_VanBanChiDao", "Trichyeu", c => c.String());
            AddColumn("dbo.T_VanBanChiDao", "SoKH", c => c.String(maxLength: 512));
            AddColumn("dbo.T_VanBanChiDao", "Ngayky", c => c.DateTime());
            DropColumn("dbo.T_DonViPhoiHop", "VanBanDi_Id");
            DropColumn("dbo.T_VanBanChiDao", "NgayDen");
            DropColumn("dbo.T_VanBanChiDao", "SoDen");
            DropColumn("dbo.T_VanBanChiDao", "TrichYeuNoiDung");
            DropColumn("dbo.T_VanBanChiDao", "NoiGui");
            DropColumn("dbo.T_VanBanChiDao", "KyHieu");
            DropColumn("dbo.T_FileDinhKem", "IdVanBanDi");
            DropColumn("dbo.T_TinhHinhThucHien", "VanBanDi_Id");
            DropTable("dbo.T_VanBanDi");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.T_VanBanDi",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Int(),
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
                        NguonChiDao = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.T_TinhHinhThucHien", "VanBanDi_Id", c => c.Long());
            AddColumn("dbo.T_FileDinhKem", "IdVanBanDi", c => c.Long());
            AddColumn("dbo.T_VanBanChiDao", "KyHieu", c => c.String(maxLength: 20));
            AddColumn("dbo.T_VanBanChiDao", "NoiGui", c => c.String(maxLength: 512));
            AddColumn("dbo.T_VanBanChiDao", "TrichYeuNoiDung", c => c.String(maxLength: 1024));
            AddColumn("dbo.T_VanBanChiDao", "SoDen", c => c.String(maxLength: 20));
            AddColumn("dbo.T_VanBanChiDao", "NgayDen", c => c.DateTime());
            AddColumn("dbo.T_DonViPhoiHop", "VanBanDi_Id", c => c.Long());
            DropColumn("dbo.T_VanBanChiDao", "Ngayky");
            DropColumn("dbo.T_VanBanChiDao", "SoKH");
            DropColumn("dbo.T_VanBanChiDao", "Trichyeu");
            CreateIndex("dbo.T_FileDinhKem", "IdVanBanDi");
            CreateIndex("dbo.T_TinhHinhThucHien", "VanBanDi_Id");
            CreateIndex("dbo.T_VanBanDi", "UserId");
            CreateIndex("dbo.T_DonViPhoiHop", "VanBanDi_Id");
            CreateIndex("dbo.T_VanBanDi", "IdDonVi");
            AddForeignKey("dbo.T_FileDinhKem", "IdVanBanDi", "dbo.T_VanBanDi", "Id");
            AddForeignKey("dbo.T_TinhHinhThucHien", "VanBanDi_Id", "dbo.T_VanBanDi", "Id");
            AddForeignKey("dbo.T_VanBanDi", "UserId", "dbo.T_DangNhap", "Id");
            AddForeignKey("dbo.T_DonViPhoiHop", "VanBanDi_Id", "dbo.T_VanBanDi", "Id");
            AddForeignKey("dbo.T_VanBanDi", "IdDonVi", "dbo.T_DonVi", "Id");
        }
    }
}
