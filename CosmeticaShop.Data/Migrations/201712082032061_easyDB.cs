namespace CosmeticaShop.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class easyDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Brands",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 128),
                        KeyUrl = c.String(maxLength: 128),
                        PhotoUrl = c.String(maxLength: 128),
                        IsActive = c.Boolean(nullable: false),
                        SeoKeywords = c.String(maxLength: 256),
                        SeoDescription = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BrandId = c.Int(),
                        CategoryId = c.Int(),
                        Name = c.String(maxLength: 128),
                        KeyUrl = c.String(maxLength: 128),
                        Description = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Discount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PhotoUrl = c.String(maxLength: 128),
                        IsActive = c.Boolean(nullable: false),
                        SeoKeywords = c.String(maxLength: 256),
                        SeoDescription = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Brands", t => t.BrandId)
                .ForeignKey("dbo.Categories", t => t.CategoryId)
                .Index(t => t.BrandId)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParentId = c.Int(),
                        Name = c.String(maxLength: 128),
                        KeyUrl = c.String(maxLength: 128),
                        Priority = c.Int(),
                        PhotoUrl = c.String(maxLength: 128),
                        IsActive = c.Boolean(nullable: false),
                        SeoKeywords = c.String(maxLength: 256),
                        SeoDescription = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.ParentId)
                .Index(t => t.ParentId);
            
            CreateTable(
                "dbo.OrderProducts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Discount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsDelete = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OrderHeaders", t => t.OrderId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.OrderId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.OrderHeaders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Guid(nullable: false),
                        DateCreate = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        Address = c.String(maxLength: 512),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsDelete = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Email = c.String(nullable: false, maxLength: 128),
                        PasswordHash = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(maxLength: 128),
                        LastName = c.String(maxLength: 128),
                        Status = c.Int(nullable: false),
                        DateBirth = c.DateTime(),
                        ConfirmationToken = c.Guid(),
                        TokenExpireDate = c.DateTime(),
                        PhotoUrl = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProductReviews",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Guid(nullable: false),
                        ProductId = c.Int(nullable: false),
                        DateCreate = c.DateTime(nullable: false),
                        Content = c.String(maxLength: 1024),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserAddresses",
                c => new
                    {
                        UserId = c.Guid(nullable: false),
                        Country = c.String(),
                        City = c.String(),
                        Address = c.String(),
                        Phone = c.String(),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.WishLists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Guid(nullable: false),
                        ProductId = c.Int(nullable: false),
                        DateCreate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.ProductId);
            
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
            
            CreateTable(
                "dbo.RoleUsers",
                c => new
                    {
                        Role_Id = c.Int(nullable: false),
                        User_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Role_Id, t.User_Id })
                .ForeignKey("dbo.Roles", t => t.Role_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.Role_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderProducts", "ProductId", "dbo.Products");
            DropForeignKey("dbo.OrderProducts", "OrderId", "dbo.OrderHeaders");
            DropForeignKey("dbo.OrderHeaders", "UserId", "dbo.Users");
            DropForeignKey("dbo.WishLists", "UserId", "dbo.Users");
            DropForeignKey("dbo.WishLists", "ProductId", "dbo.Products");
            DropForeignKey("dbo.UserAddresses", "UserId", "dbo.Users");
            DropForeignKey("dbo.RoleUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.RoleUsers", "Role_Id", "dbo.Roles");
            DropForeignKey("dbo.ProductReviews", "UserId", "dbo.Users");
            DropForeignKey("dbo.ProductReviews", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Products", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Categories", "ParentId", "dbo.Categories");
            DropForeignKey("dbo.Products", "BrandId", "dbo.Brands");
            DropIndex("dbo.RoleUsers", new[] { "User_Id" });
            DropIndex("dbo.RoleUsers", new[] { "Role_Id" });
            DropIndex("dbo.WishLists", new[] { "ProductId" });
            DropIndex("dbo.WishLists", new[] { "UserId" });
            DropIndex("dbo.UserAddresses", new[] { "UserId" });
            DropIndex("dbo.ProductReviews", new[] { "ProductId" });
            DropIndex("dbo.ProductReviews", new[] { "UserId" });
            DropIndex("dbo.OrderHeaders", new[] { "UserId" });
            DropIndex("dbo.OrderProducts", new[] { "ProductId" });
            DropIndex("dbo.OrderProducts", new[] { "OrderId" });
            DropIndex("dbo.Categories", new[] { "ParentId" });
            DropIndex("dbo.Products", new[] { "CategoryId" });
            DropIndex("dbo.Products", new[] { "BrandId" });
            DropTable("dbo.RoleUsers");
            DropTable("dbo.Sliders");
            DropTable("dbo.SitePages");
            DropTable("dbo.WishLists");
            DropTable("dbo.UserAddresses");
            DropTable("dbo.Roles");
            DropTable("dbo.ProductReviews");
            DropTable("dbo.Users");
            DropTable("dbo.OrderHeaders");
            DropTable("dbo.OrderProducts");
            DropTable("dbo.Categories");
            DropTable("dbo.Products");
            DropTable("dbo.Brands");
        }
    }
}
