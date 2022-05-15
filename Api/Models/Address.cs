using System.Text.Json.Serialization;

namespace Api.Models
{
    public class Address
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
        public string Country { get; set; }
    }
}