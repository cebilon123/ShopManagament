using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public int Telephone { get; set; }

        public int PrimaryAddressId { get; set; }
        public Address PrimaryAddress { get; set; }

        public int SecondaryAddressId { get; set; }
        public Address SecondaryAddress { get; set; }
    }
}