using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    public partial class AddTriggerWieght : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE TRIGGER trgUpdateOrderWeight
                                    ON dbo.OrderedProducts
                                    AFTER INSERT, UPDATE
                                    AS
                                    BEGIN
	                                    UPDATE dbo.Orders
	                                    SET dbo.Orders.Weight = (SELECT SUM(dbo.OrderedProducts.Count * dbo.OrderedProducts.Weight) FROM dbo.OrderedProducts
							                                    inner join inserted on inserted.OrderId = dbo.OrderedProducts.OrderId
							                                    WHERE dbo.OrderedProducts.OrderId = inserted.OrderId)
	                                    FROM inserted
	                                    WHERE dbo.Orders.id = inserted.OrderId
                                    END
                                    ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP TRIGGER trgUpdateOrderWeight");
        }
    }
}
