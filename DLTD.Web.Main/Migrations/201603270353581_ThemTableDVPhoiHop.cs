namespace DLTD.Web.Main.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThemTableDVPhoiHop : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.T_DonViPhoihop",
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
            
            AddColumn("dbo.T_VanBanChiDao", "DoKhan", c => c.Int(nullable: false));
            AddColumn("dbo.T_VanBanChiDao", "DoQuanTrong", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.T_DonViPhoihop", "IdVanBan", "dbo.T_VanBanChiDao");
            DropForeignKey("dbo.T_DonViPhoihop", "IdDonvi", "dbo.T_DonVi");
            DropIndex("dbo.T_DonViPhoihop", new[] { "IdVanBan" });
            DropIndex("dbo.T_DonViPhoihop", new[] { "IdDonvi" });
            DropColumn("dbo.T_VanBanChiDao", "DoQuanTrong");
            DropColumn("dbo.T_VanBanChiDao", "DoKhan");
            DropTable("dbo.T_DonViPhoihop");
        }
    }
}
