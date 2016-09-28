namespace DLTD.Web.Main.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tinhhinhthuchiennoibo : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.T_VanBanChiDao", "TinhHinhThucHienNoiBo", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.T_VanBanChiDao", "TinhHinhThucHienNoiBo", c => c.Int(nullable: false));
        }
    }
}
