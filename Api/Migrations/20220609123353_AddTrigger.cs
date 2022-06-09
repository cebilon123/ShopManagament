using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    public partial class AddTrigger : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE TRIGGER trgUpdateOrderPrice
                                ON dbo.OrderedProducts
                                AFTER INSERT, UPDATE
                                AS
                                BEGIN
	                                UPDATE dbo.Orders
	                                SET dbo.Orders.Price = (SELECT SUM(dbo.OrderedProducts.Count * dbo.OrderedProducts.Price) FROM dbo.OrderedProducts
							                                inner join inserted on inserted.OrderId = dbo.OrderedProducts.OrderId
							                                WHERE dbo.OrderedProducts.OrderId = inserted.OrderId)
	                                FROM inserted
	                                WHERE dbo.Orders.id = inserted.OrderId
                                END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP TRIGGER trgUpdateOrderPrice");
        }
    }
}
