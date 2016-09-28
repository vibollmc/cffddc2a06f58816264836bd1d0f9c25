namespace DLTD.Web.Main.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSomeNewTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.T_TinhHinhPhoiHop",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        IdDonViPhoiHop = c.Int(),
                        NoiDungThucHien = c.String(maxLength: 2048),
                        NgayXuLy = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.T_DonViPhoiHop", t => t.IdDonViPhoiHop)
                .Index(t => t.IdDonViPhoiHop);
            
            CreateTable(
                "dbo.T_TinhHinhThucHien",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        IdVanBanChiDao = c.Long(),
                        NoiDungThucHien = c.String(maxLength: 2048),
                        NgayBaoCao = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.T_VanBanChiDao", t => t.IdVanBanChiDao)
                .Index(t => t.IdVanBanChiDao);
            
            AddColumn("dbo.T_FileDinhKem", "IdTinhHinhPhoiHop", c => c.Long());
            AddColumn("dbo.T_FileDinhKem", "IdTinhHinhThucHien", c => c.Long());
            AddColumn("dbo.T_FileDinhKem", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.T_FileDinhKem", "IdTinhHinhPhoiHop");
            CreateIndex("dbo.T_FileDinhKem", "IdTinhHinhThucHien");
            AddForeignKey("dbo.T_FileDinhKem", "IdTinhHinhPhoiHop", "dbo.T_TinhHinhPhoiHop", "Id");
            AddForeignKey("dbo.T_FileDinhKem", "IdTinhHinhThucHien", "dbo.T_TinhHinhThucHien", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.T_TinhHinhThucHien", "IdVanBanChiDao", "dbo.T_VanBanChiDao");
            DropForeignKey("dbo.T_FileDinhKem", "IdTinhHinhThucHien", "dbo.T_TinhHinhThucHien");
            DropForeignKey("dbo.T_FileDinhKem", "IdTinhHinhPhoiHop", "dbo.T_TinhHinhPhoiHop");
            DropForeignKey("dbo.T_TinhHinhPhoiHop", "IdDonViPhoiHop", "dbo.T_DonViPhoiHop");
            DropIndex("dbo.T_TinhHinhThucHien", new[] { "IdVanBanChiDao" });
            DropIndex("dbo.T_FileDinhKem", new[] { "IdTinhHinhThucHien" });
            DropIndex("dbo.T_FileDinhKem", new[] { "IdTinhHinhPhoiHop" });
            DropIndex("dbo.T_TinhHinhPhoiHop", new[] { "IdDonViPhoiHop" });
            DropColumn("dbo.T_FileDinhKem", "Discriminator");
            DropColumn("dbo.T_FileDinhKem", "IdTinhHinhThucHien");
            DropColumn("dbo.T_FileDinhKem", "IdTinhHinhPhoiHop");
            DropTable("dbo.T_TinhHinhThucHien");
            DropTable("dbo.T_TinhHinhPhoiHop");
        }
    }
}
