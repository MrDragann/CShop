namespace CosmeticaShop.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class blogTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Blogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateCreate = c.DateTime(nullable: false),
                        Title = c.String(maxLength: 128),
                        Content = c.String(),
                        PreviewContent = c.String(),
                        PhotoUrl = c.String(maxLength: 128),
                        KeyUrl = c.String(maxLength: 128),
                        IsActive = c.Boolean(nullable: false),
                        IsDelete = c.DateTime(),
                        SeoKeywords = c.String(maxLength: 256),
                        SeoDescription = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Blogs");
        }
    }
}
