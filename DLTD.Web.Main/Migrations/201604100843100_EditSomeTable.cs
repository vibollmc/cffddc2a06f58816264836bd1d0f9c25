namespace DLTD.Web.Main.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditSomeTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.T_TinhHinhPhoiHop", "UserId", c => c.Int());
            AddColumn("dbo.T_TinhHinhThucHien", "UserId", c => c.Int());
            CreateIndex("dbo.T_TinhHinhThucHien", "UserId");
            CreateIndex("dbo.T_TinhHinhPhoiHop", "UserId");
            AddForeignKey("dbo.T_TinhHinhThucHien", "UserId", "dbo.T_DangNhap", "Id");
            AddForeignKey("dbo.T_TinhHinhPhoiHop", "UserId", "dbo.T_DangNhap", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.T_TinhHinhPhoiHop", "UserId", "dbo.T_DangNhap");
            DropForeignKey("dbo.T_TinhHinhThucHien", "UserId", "dbo.T_DangNhap");
            DropIndex("dbo.T_TinhHinhPhoiHop", new[] { "UserId" });
            DropIndex("dbo.T_TinhHinhThucHien", new[] { "UserId" });
            DropColumn("dbo.T_TinhHinhThucHien", "UserId");
            DropColumn("dbo.T_TinhHinhPhoiHop", "UserId");
        }
    }
}
