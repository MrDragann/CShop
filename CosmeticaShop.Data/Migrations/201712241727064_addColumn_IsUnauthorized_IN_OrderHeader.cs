namespace CosmeticaShop.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addColumn_IsUnauthorized_IN_OrderHeader : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderHeaders", "IsUnauthorized", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderHeaders", "IsUnauthorized");
        }
    }
}
