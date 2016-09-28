namespace DLTD.Web.Main.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NguonChiDao1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.T_VanBanChiDao", "NguonChiDao", c => c.Int(nullable: false));
            DropColumn("dbo.T_VanBanChiDao", "DoQuanTrong");
        }
        
        public override void Down()
        {
            AddColumn("dbo.T_VanBanChiDao", "DoQuanTrong", c => c.Int(nullable: false));
            DropColumn("dbo.T_VanBanChiDao", "NguonChiDao");
        }
    }
}
