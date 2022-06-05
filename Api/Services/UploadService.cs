using System;
using System.Globalization;
using System.IO;
using System.Linq;
using Api.Models;
using CsvHelper;
using CsvHelper.Configuration.Attributes;
using OfficeOpenXml;
using Product = Api.Result.Product;

namespace Api.Services
{
    public class UploadService
    {
        private readonly DatabaseContext _databaseContext;

        public UploadService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        private class UploadOrder
        {
            [Index(0)] [Ignore] public int Id { get; set; }
            [Index(1)] public int UserId { get; set; }
            [Index(2)] public int WorkerId { get; set; }
            [Index(4)] public float Price { get; set; }
            [Index(5)] public float Weight { get; set; }
            [Index(6)] public string ShipmentMethod { get; set; }
            [Index(7)] public string PaymentMethod { get; set; }
            [Index(8)] public bool OrderPaid { get; set; }
            [Index(9)] public bool OrderSend { get; set; }
            [Index(10)] public DateTime OrderDate { get; set; }
            [Index(11)] public DateTime OrderPaymentDate { get; set; }
            [Index(12)] public DateTime OrderSendDate { get; set; }

            public Order ToDatabaseOrder()
            {
                return new Order
                {
                    Id = Id,
                    Price = Price,
                    UserId = UserId == 0 ? 10 : UserId,
                    Weight = Weight,
                    OrderDate = OrderDate,
                    OrderPaid = OrderPaid,
                    OrderSend = OrderSend,
                    PaymentMethod = PaymentMethod,
                    ShipmentMethod = ShipmentMethod,
                    WorkerId = WorkerId == 0 ? 12 : WorkerId,
                    OrderPaymentDate = OrderPaymentDate,
                    OrderSendDate = OrderSendDate
                };
            }
        }

        private class UploadAddress
        {
            [Index(0)] [Ignore] public int Id { get; set; }
            [Index(1)] public string Street { get; set; }
            [Index(2)] public string Number { get; set; }
            [Index(3)] public string City { get; set; }
            [Index(4)] public string Postcode { get; set; }
            [Index(5)] public string Country { get; set; }

            public Address ToDatabaseAddress()
            {
                return new Address
                {
                    City = City,
                    Country = Country,
                    Id = Id,
                    Number = Number,
                    Postcode = Postcode,
                    Street = Street
                };
            }
        }

        private class UploadUser
        {
            [Index(0)] [Ignore] public int Id { get; set; }
            [Index(1)] public string Login { get; set; }
            [Index(2)] public string Password { get; set; }
            [Index(3)] public string Name { get; set; }
            [Index(4)] public string Surname { get; set; }
            [Index(5)] public DateTime BirthDate { get; set; }
            [Index(6)] public string Email { get; set; }
            [Index(7)] public int Telephone { get; set; }
            [Index(8)] public int PrimaryAddressId { get; set; }
            [Index(9)] public int SecondaryAddressId { get; set; }

            public User ToDatabaseUser()
            {
                return new User
                {
                    Email = Email,
                    Id = Id,
                    Login = Login,
                    Name = Name,
                    Password = Password,
                    Surname = Surname,
                    Telephone = Telephone,
                    BirthDate = BirthDate,
                    PrimaryAddressId = PrimaryAddressId,
                    SecondaryAddressId = SecondaryAddressId == 0 ? 5 : SecondaryAddressId
                };
            }
        }

        private class UploadProduct
        {
            [Index(0)] [Ignore] public int Id { get; set; }
            [Index(1)] public string Name { get; set; }
            [Index(2)] public float Price { get; set; }
            [Index(3)] public int Left { get; set; }
            [Index(4)] public float Weight { get; set; }
            [Index(5)] public string Category { get; set; }
            [Index(6)] public string Description { get; set; }

            public Models.Product ToDatabaseProduct()
            {
                return new Models.Product
                {
                    Category = Category,
                    Description = Description,
                    Id = Id,
                    Left = Left,
                    Name = Name,
                    Price = Price,
                    Weight = Weight
                };
            }
        }

        public void UploadOrders(MemoryStream fileStream)
        {
            fileStream.Position = 0;
            using var readStream = new StreamReader(fileStream);
            var csv = new CsvReader(readStream, CultureInfo.InvariantCulture);
            var records = csv.GetRecords<UploadOrder>();

            if (!records.Any()) return;

            var mappedRecords = records
                .Select(r => r.ToDatabaseOrder())
                .ToList();

            _databaseContext.Orders.AddRange(mappedRecords);
            _databaseContext.SaveChanges();
        }

        public void UploadProducts(MemoryStream fileStream)
        {
            fileStream.Position = 0;
            using var readStream = new StreamReader(fileStream);
            var csv = new CsvReader(readStream, CultureInfo.InvariantCulture);
            var records = csv.GetRecords<UploadProduct>();

            if (!records.Any()) return;

            var mappedRecords = records
                .Select(r => r.ToDatabaseProduct())
                .ToList();

            _databaseContext.Products.AddRange(mappedRecords);
            _databaseContext.SaveChanges();
        }

        public void UploadUsers(MemoryStream fileStream)
        {
            fileStream.Position = 0;
            using var readStream = new StreamReader(fileStream);
            var csv = new CsvReader(readStream, CultureInfo.InvariantCulture);
            var records = csv.GetRecords<UploadUser>();

            if (!records.Any()) return;

            var mappedRecords = records
                .Select(r => r.ToDatabaseUser())
                .ToList();

            _databaseContext.Users.AddRange(mappedRecords);
            _databaseContext.SaveChanges();
        }

        public void UploadAddresses(MemoryStream fileStream)
        {
            fileStream.Position = 0;
            using var readStream = new StreamReader(fileStream);
            var csv = new CsvReader(readStream, CultureInfo.InvariantCulture);
            var records = csv.GetRecords<UploadAddress>();

            if (!records.Any()) return;

            var mappedRecords = records
                .Select(r => r.ToDatabaseAddress())
                .ToList();

            _databaseContext.Addresses.AddRange(mappedRecords);
            _databaseContext.SaveChanges();
        }
    }
}