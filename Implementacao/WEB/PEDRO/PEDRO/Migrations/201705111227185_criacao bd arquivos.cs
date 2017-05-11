namespace PEDRO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class criacaobdarquivos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ArchiveUsersModels",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        user_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.AspNetUsers", t => t.user_Id)
                .Index(t => t.user_Id);
            
            AddColumn("dbo.CloudModels", "ArchiveUsersModels_id", c => c.Int());
            CreateIndex("dbo.CloudModels", "ArchiveUsersModels_id");
            AddForeignKey("dbo.CloudModels", "ArchiveUsersModels_id", "dbo.ArchiveUsersModels", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ArchiveUsersModels", "user_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.CloudModels", "ArchiveUsersModels_id", "dbo.ArchiveUsersModels");
            DropIndex("dbo.CloudModels", new[] { "ArchiveUsersModels_id" });
            DropIndex("dbo.ArchiveUsersModels", new[] { "user_Id" });
            DropColumn("dbo.CloudModels", "ArchiveUsersModels_id");
            DropTable("dbo.ArchiveUsersModels");
        }
    }
}
