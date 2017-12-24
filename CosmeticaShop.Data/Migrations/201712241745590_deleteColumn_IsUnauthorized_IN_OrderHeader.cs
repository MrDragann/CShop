namespace CosmeticaShop.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deleteColumn_IsUnauthorized_IN_OrderHeader : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.OrderHeaders", "IsUnauthorized");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrderHeaders", "IsUnauthorized", c => c.Boolean(nullable: false));
        }
    }
}
