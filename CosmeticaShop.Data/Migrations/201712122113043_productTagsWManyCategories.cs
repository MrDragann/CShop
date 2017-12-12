namespace CosmeticaShop.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class productTagsWManyCategories : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Products", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Products", new[] { "CategoryId" });
            CreateTable(
                "dbo.ProductTags",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProductCategories",
                c => new
                    {
                        ProductId = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProductId, t.CategoryId })
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.ProductProductTags",
                c => new
                    {
                        ProductId = c.Int(nullable: false),
                        ProductTagId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProductId, t.ProductTagId })
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.ProductTags", t => t.ProductTagId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.ProductTagId);
            
            AddColumn("dbo.Products", "DateCreate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Products", "IsInStock", c => c.Boolean(nullable: false));
            DropColumn("dbo.Products", "CategoryId");
            DropColumn("dbo.Categories", "PhotoUrl");
            DropColumn("dbo.Categories", "SeoKeywords");
            DropColumn("dbo.Categories", "SeoDescription");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Categories", "SeoDescription", c => c.String(maxLength: 256));
            AddColumn("dbo.Categories", "SeoKeywords", c => c.String(maxLength: 256));
            AddColumn("dbo.Categories", "PhotoUrl", c => c.String(maxLength: 128));
            AddColumn("dbo.Products", "CategoryId", c => c.Int());
            DropForeignKey("dbo.ProductProductTags", "ProductTagId", "dbo.ProductTags");
            DropForeignKey("dbo.ProductProductTags", "ProductId", "dbo.Products");
            DropForeignKey("dbo.ProductCategories", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.ProductCategories", "ProductId", "dbo.Products");
            DropIndex("dbo.ProductProductTags", new[] { "ProductTagId" });
            DropIndex("dbo.ProductProductTags", new[] { "ProductId" });
            DropIndex("dbo.ProductCategories", new[] { "CategoryId" });
            DropIndex("dbo.ProductCategories", new[] { "ProductId" });
            DropColumn("dbo.Products", "IsInStock");
            DropColumn("dbo.Products", "DateCreate");
            DropTable("dbo.ProductProductTags");
            DropTable("dbo.ProductCategories");
            DropTable("dbo.ProductTags");
            CreateIndex("dbo.Products", "CategoryId");
            AddForeignKey("dbo.Products", "CategoryId", "dbo.Categories", "Id");
        }
    }
}
