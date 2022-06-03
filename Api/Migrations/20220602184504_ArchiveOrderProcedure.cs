using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <summary>
    /// Order bedzie zarchiwizowany tak dlugo jak posiada date wysylki oraz platnosci
    /// </summary>
    public partial class ArchiveOrderProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE PROCEDURE ArchiveOrder
                                (
                                @orderId INT
                                )
                                AS
                                BEGIN
                                SET NOCOUNT ON

                                INSERT INTO dbo.OrderArchives(OrderDate, OrderId, OrderPaid, OrderPaymentDate, OrderSend, OrderSendDate, PaymentMethod, Price, ShipmentMethod, UserId, Weight, WorkerId) 
                                SELECT OrderDate, Id, OrderPaid, OrderPaymentDate, OrderSend, OrderSendDate, PaymentMethod, Price, ShipmentMethod, UserId, Weight, WorkerId FROM dbo.Orders
                                WHERE dbo.Orders.Id = @orderId AND dbo.Orders.OrderPaymentDate != '0001-01-01 00:00:00.0000000' AND dbo.Orders.OrderSendDate != '0001-01-01 00:00:00.0000000'
                                END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE ArchiveOrder");
        }
    }
}
