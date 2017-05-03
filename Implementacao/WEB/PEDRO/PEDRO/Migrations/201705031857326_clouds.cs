namespace PEDRO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class clouds : DbMigration
    {
        public override void Up()
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
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CloudModels");
        }
    }
}
