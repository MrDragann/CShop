namespace CosmeticaShop.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sitePagesliderTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SitePages",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Title = c.String(maxLength: 128),
                        Content = c.String(),
                        ExtraContent = c.String(),
                        SeoKeywords = c.String(maxLength: 256),
                        SeoDescription = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Sliders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SitePage = c.Int(nullable: false),
                        DateCreate = c.DateTime(nullable: false),
                        Priority = c.Int(),
                        PhotoUrl = c.String(maxLength: 128),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Brands", "PhotoUrl", c => c.String(maxLength: 128));
            AddColumn("dbo.Brands", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Brands", "SeoKeywords", c => c.String(maxLength: 256));
            AddColumn("dbo.Brands", "SeoDescription", c => c.String(maxLength: 256));
            AddColumn("dbo.Products", "PhotoUrl", c => c.String(maxLength: 128));
            AddColumn("dbo.Products", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Products", "SeoKeywords", c => c.String(maxLength: 256));
            AddColumn("dbo.Products", "SeoDescription", c => c.String(maxLength: 256));
            AddColumn("dbo.Categories", "Priority", c => c.Int());
            AddColumn("dbo.Categories", "PhotoUrl", c => c.String(maxLength: 128));
            AddColumn("dbo.Categories", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Categories", "SeoKeywords", c => c.String(maxLength: 256));
            AddColumn("dbo.Categories", "SeoDescription", c => c.String(maxLength: 256));
            AddColumn("dbo.Users", "PhotoUrl", c => c.String(maxLength: 128));
            AlterColumn("dbo.Brands", "Name", c => c.String(maxLength: 128));
            AlterColumn("dbo.Brands", "KeyUrl", c => c.String(maxLength: 128));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Brands", "KeyUrl", c => c.String());
            AlterColumn("dbo.Brands", "Name", c => c.String());
            DropColumn("dbo.Users", "PhotoUrl");
            DropColumn("dbo.Categories", "SeoDescription");
            DropColumn("dbo.Categories", "SeoKeywords");
            DropColumn("dbo.Categories", "IsActive");
            DropColumn("dbo.Categories", "PhotoUrl");
            DropColumn("dbo.Categories", "Priority");
            DropColumn("dbo.Products", "SeoDescription");
            DropColumn("dbo.Products", "SeoKeywords");
            DropColumn("dbo.Products", "IsActive");
            DropColumn("dbo.Products", "PhotoUrl");
            DropColumn("dbo.Brands", "SeoDescription");
            DropColumn("dbo.Brands", "SeoKeywords");
            DropColumn("dbo.Brands", "IsActive");
            DropColumn("dbo.Brands", "PhotoUrl");
            DropTable("dbo.Sliders");
            DropTable("dbo.SitePages");
        }
    }
}
