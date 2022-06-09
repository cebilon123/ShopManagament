using System.Text.Json.Serialization;

namespace Api.Commands
{
    public class AddProductToOrderCommand
    {
        [JsonIgnore]
        public int OrderId { get; set; }

        public int ProductId { get; set; }
        public int Amount { get; set; }
    }
}