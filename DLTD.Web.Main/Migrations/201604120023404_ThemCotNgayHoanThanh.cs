namespace DLTD.Web.Main.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThemCotNgayHoanThanh : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.T_VanBanChiDao", "NgayHoanThanh", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.T_VanBanChiDao", "NgayHoanThanh");
        }
    }
}
