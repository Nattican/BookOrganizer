namespace BookOrganizer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2nd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "Year", c => c.Int(nullable: false));
            AddColumn("dbo.Books", "Pages", c => c.Int(nullable: false));
            AddColumn("dbo.Books", "Mark", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Books", "Mark");
            DropColumn("dbo.Books", "Pages");
            DropColumn("dbo.Books", "Year");
        }
    }
}
