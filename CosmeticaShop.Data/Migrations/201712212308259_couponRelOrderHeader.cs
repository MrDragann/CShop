namespace CosmeticaShop.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class couponRelOrderHeader : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Coupons",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(maxLength: 32),
                        Discount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateCreate = c.DateTime(nullable: false),
                        IsDelete = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.OrderHeaders", "CouponId", c => c.Int());
            CreateIndex("dbo.OrderHeaders", "CouponId");
            AddForeignKey("dbo.OrderHeaders", "CouponId", "dbo.Coupons", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderHeaders", "CouponId", "dbo.Coupons");
            DropIndex("dbo.OrderHeaders", new[] { "CouponId" });
            DropColumn("dbo.OrderHeaders", "CouponId");
            DropTable("dbo.Coupons");
        }
    }
}
