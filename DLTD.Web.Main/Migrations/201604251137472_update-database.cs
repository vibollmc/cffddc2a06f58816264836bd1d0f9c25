namespace DLTD.Web.Main.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedatabase : DbMigration
    {
        public override void Up()
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.T_DonVi", t => t.IdDonVi)
                .ForeignKey("dbo.T_DangNhap", t => t.UserId)
                .Index(t => t.IdDonVi)
                .Index(t => t.UserId);
            
            AddColumn("dbo.T_DonViPhoiHop", "VanBanDi_Id", c => c.Long());
            AddColumn("dbo.T_FileDinhKem", "VanBanDi_Id", c => c.Long());
            AddColumn("dbo.T_TinhHinhThucHien", "VanBanDi_Id", c => c.Long());
            CreateIndex("dbo.T_DonViPhoiHop", "VanBanDi_Id");
            CreateIndex("dbo.T_FileDinhKem", "VanBanDi_Id");
            CreateIndex("dbo.T_TinhHinhThucHien", "VanBanDi_Id");
            AddForeignKey("dbo.T_DonViPhoiHop", "VanBanDi_Id", "dbo.T_VanBanDi", "Id");
            AddForeignKey("dbo.T_FileDinhKem", "VanBanDi_Id", "dbo.T_VanBanDi", "Id");
            AddForeignKey("dbo.T_TinhHinhThucHien", "VanBanDi_Id", "dbo.T_VanBanDi", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.T_TinhHinhThucHien", "VanBanDi_Id", "dbo.T_VanBanDi");
            DropForeignKey("dbo.T_VanBanDi", "UserId", "dbo.T_DangNhap");
            DropForeignKey("dbo.T_FileDinhKem", "VanBanDi_Id", "dbo.T_VanBanDi");
            DropForeignKey("dbo.T_DonViPhoiHop", "VanBanDi_Id", "dbo.T_VanBanDi");
            DropForeignKey("dbo.T_VanBanDi", "IdDonVi", "dbo.T_DonVi");
            DropIndex("dbo.T_TinhHinhThucHien", new[] { "VanBanDi_Id" });
            DropIndex("dbo.T_VanBanDi", new[] { "UserId" });
            DropIndex("dbo.T_FileDinhKem", new[] { "VanBanDi_Id" });
            DropIndex("dbo.T_DonViPhoiHop", new[] { "VanBanDi_Id" });
            DropIndex("dbo.T_VanBanDi", new[] { "IdDonVi" });
            DropColumn("dbo.T_TinhHinhThucHien", "VanBanDi_Id");
            DropColumn("dbo.T_FileDinhKem", "VanBanDi_Id");
            DropColumn("dbo.T_DonViPhoiHop", "VanBanDi_Id");
            DropTable("dbo.T_VanBanDi");
        }
    }
}
