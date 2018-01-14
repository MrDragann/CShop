namespace CosmeticaShop.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class productsIsDeleteProp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "IsDelete", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "IsDelete");
        }
    }
}
