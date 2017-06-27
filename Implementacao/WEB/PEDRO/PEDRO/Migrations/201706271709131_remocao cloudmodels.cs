namespace PEDRO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class remocaocloudmodels : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CloudModels", "user_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.CloudModels", "ArchiveUsersModels_id", "dbo.ArchiveUsersModels");
            DropIndex("dbo.CloudModels", new[] { "user_Id" });
            DropIndex("dbo.CloudModels", new[] { "ArchiveUsersModels_id" });
            DropTable("dbo.CloudModels");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CloudModels",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        nome = c.String(nullable: false),
                        email = c.String(nullable: false),
                        pass = c.String(nullable: false),
                        confirmPass = c.String(),
                        user_Id = c.String(maxLength: 128),
                        ArchiveUsersModels_id = c.Int(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateIndex("dbo.CloudModels", "ArchiveUsersModels_id");
            CreateIndex("dbo.CloudModels", "user_Id");
            AddForeignKey("dbo.CloudModels", "ArchiveUsersModels_id", "dbo.ArchiveUsersModels", "id");
            AddForeignKey("dbo.CloudModels", "user_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
