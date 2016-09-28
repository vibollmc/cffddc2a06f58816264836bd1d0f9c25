namespace DLTD.Web.Main.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addtralaivb : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.T_VanBanChiDao", "IsTralai", c => c.Int());
            AddColumn("dbo.T_VanBanChiDao", "LydoTraLai", c => c.Int());
            AddColumn("dbo.T_VanBanChiDao", "NgayTra", c => c.DateTime());
            AddColumn("dbo.T_VanBanChiDao", "TinhHinhThucHienNoiBo", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.T_VanBanChiDao", "TinhHinhThucHienNoiBo");
            DropColumn("dbo.T_VanBanChiDao", "NgayTra");
            DropColumn("dbo.T_VanBanChiDao", "LydoTraLai");
            DropColumn("dbo.T_VanBanChiDao", "IsTralai");
        }
    }
}
