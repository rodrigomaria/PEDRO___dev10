namespace PEDRO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class trocadevariavelchaveestrangeira : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.CloudModels", new[] { "user_Id" });
            DropColumn("dbo.CloudModels", "userid");
            RenameColumn(table: "dbo.CloudModels", name: "user_Id", newName: "userid");
            AlterColumn("dbo.CloudModels", "userid", c => c.String(maxLength: 128));
            CreateIndex("dbo.CloudModels", "userid");
        }
        
        public override void Down()
        {
            DropIndex("dbo.CloudModels", new[] { "userid" });
            AlterColumn("dbo.CloudModels", "userid", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.CloudModels", name: "userid", newName: "user_Id");
            AddColumn("dbo.CloudModels", "userid", c => c.Int(nullable: false));
            CreateIndex("dbo.CloudModels", "user_Id");
        }
    }
}
