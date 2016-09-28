namespace DLTD.Web.Main.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NguonChiDao2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.T_Khoi", "NguonChiDao", c => c.Boolean());
            AlterColumn("dbo.T_VanBanChiDao", "NguonChiDao", c => c.Int());
            CreateIndex("dbo.T_VanBanChiDao", "NguonChiDao");
            Sql("Update DLTD.dbo.T_VanBanChiDao Set NguonChiDao = null");
            AddForeignKey("dbo.T_VanBanChiDao", "NguonChiDao", "dbo.T_Khoi", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.T_VanBanChiDao", "NguonChiDao", "dbo.T_Khoi");
            DropIndex("dbo.T_VanBanChiDao", new[] { "NguonChiDao" });
            AlterColumn("dbo.T_VanBanChiDao", "NguonChiDao", c => c.Int(nullable: false));
            DropColumn("dbo.T_Khoi", "NguonChiDao");
        }
    }
}
