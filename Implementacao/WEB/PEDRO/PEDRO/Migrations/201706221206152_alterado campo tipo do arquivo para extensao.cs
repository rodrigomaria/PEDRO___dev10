namespace PEDRO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alteradocampotipodoarquivoparaextensao : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ArchiveUsersModels", "extensao", c => c.String(nullable: false));
            DropColumn("dbo.ArchiveUsersModels", "tipoArquivo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ArchiveUsersModels", "tipoArquivo", c => c.String(nullable: false));
            DropColumn("dbo.ArchiveUsersModels", "extensao");
        }
    }
}
