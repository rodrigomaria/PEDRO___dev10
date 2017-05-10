namespace PEDRO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adiçãodechaveestrangeiraemcloud : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CloudModels", "userid", c => c.Int(nullable: false));
            AddColumn("dbo.CloudModels", "user_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.CloudModels", "user_Id");
            AddForeignKey("dbo.CloudModels", "user_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CloudModels", "user_Id", "dbo.AspNetUsers");
            DropIndex("dbo.CloudModels", new[] { "user_Id" });
            DropColumn("dbo.CloudModels", "user_Id");
            DropColumn("dbo.CloudModels", "userid");
        }
    }
}
