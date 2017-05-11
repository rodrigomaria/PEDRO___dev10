namespace PEDRO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatebd : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.CloudModels", name: "userid", newName: "user_Id");
            RenameIndex(table: "dbo.CloudModels", name: "IX_userid", newName: "IX_user_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.CloudModels", name: "IX_user_Id", newName: "IX_userid");
            RenameColumn(table: "dbo.CloudModels", name: "user_Id", newName: "userid");
        }
    }
}
