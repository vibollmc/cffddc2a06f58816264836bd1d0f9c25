namespace DLTD.Web.Main.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.T_VanBanChiDao", "IsTralai", c => c.Boolean());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.T_VanBanChiDao", "IsTralai", c => c.Int());
        }
    }
}
