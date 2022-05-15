using System;
using Api.Models;

namespace Api.Commands
{
    public class RegisterUserCommand
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public int Telephone { get; set; }
        public Address PrimaryAddress { get; set; }
        public Address SecondaryAddress { get; set; }
    }
}