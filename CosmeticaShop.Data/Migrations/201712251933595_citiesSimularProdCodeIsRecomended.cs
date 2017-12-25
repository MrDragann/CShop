namespace CosmeticaShop.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class citiesSimularProdCodeIsRecomended : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 64),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SimilarProducts",
                c => new
                    {
                        ProductId = c.Int(nullable: false),
                        SimilarProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProductId, t.SimilarProductId })
                .ForeignKey("dbo.Products", t => t.ProductId)
                .ForeignKey("dbo.Products", t => t.SimilarProductId)
                .Index(t => t.ProductId)
                .Index(t => t.SimilarProductId);
            
            AddColumn("dbo.Products", "Code", c => c.String());
            AddColumn("dbo.Products", "IsRecommended", c => c.Boolean(nullable: false));
            AddColumn("dbo.OrderHeaders", "CouponJson", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SimilarProducts", "SimilarProductId", "dbo.Products");
            DropForeignKey("dbo.SimilarProducts", "ProductId", "dbo.Products");
            DropIndex("dbo.SimilarProducts", new[] { "SimilarProductId" });
            DropIndex("dbo.SimilarProducts", new[] { "ProductId" });
            DropColumn("dbo.OrderHeaders", "CouponJson");
            DropColumn("dbo.Products", "IsRecommended");
            DropColumn("dbo.Products", "Code");
            DropTable("dbo.SimilarProducts");
            DropTable("dbo.Cities");
        }
    }
}
