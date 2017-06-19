namespace PEDRO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class compartilhamento : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SharedArchiveModels",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        proprietario_id = c.String(),
                        usuario_id = c.String(),
                        arquivo_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SharedArchiveModels");
        }
    }
}
