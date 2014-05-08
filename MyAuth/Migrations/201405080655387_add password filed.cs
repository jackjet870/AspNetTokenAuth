namespace MyAuth.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addpasswordfiled : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Password", c => c.String(maxLength: 100, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Password");
        }
    }
}
