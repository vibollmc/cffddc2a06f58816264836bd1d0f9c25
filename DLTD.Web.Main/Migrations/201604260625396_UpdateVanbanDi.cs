namespace DLTD.Web.Main.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateVanbanDi : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.T_FileDinhKem", "VanBanDi_Id", "dbo.T_VanBanDi");
            DropIndex("dbo.T_FileDinhKem", new[] { "VanBanDi_Id" });
            AddColumn("dbo.T_FileDinhKem", "IdVanBanDi", c => c.Long());
            CreateIndex("dbo.T_FileDinhKem", "IdVanBanDi");
            AddForeignKey("dbo.T_FileDinhKem", "IdVanBanDi", "dbo.T_VanBanDi", "Id");
            DropColumn("dbo.T_FileDinhKem", "VanBanDi_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.T_FileDinhKem", "VanBanDi_Id", c => c.Long());
            DropForeignKey("dbo.T_FileDinhKem", "IdVanBanDi", "dbo.T_VanBanDi");
            DropIndex("dbo.T_FileDinhKem", new[] { "IdVanBanDi" });
            DropColumn("dbo.T_FileDinhKem", "IdVanBanDi");
            CreateIndex("dbo.T_FileDinhKem", "VanBanDi_Id");
            AddForeignKey("dbo.T_FileDinhKem", "VanBanDi_Id", "dbo.T_VanBanDi", "Id");
        }
    }
}
