using System;
using Api.Models;

namespace Api.Result
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public int Telephone { get; set; }
        public Address PrimaryAddress { get; set; }
        public Address SecondaryAddress { get; set; }

        /// <summary>
        /// Metoda mapująca usera z bazy danych do usera jako rezultat
        /// </summary>
        /// <param name="user">Model bazodanowy użytkownika</param>
        /// <returns>User jako result model</returns>
        public static User FromDatabase(Models.User user)
        {
            return new User
            {
                Email = user.Email,
                Id = user.Id,
                Login = user.Login,
                Name = user.Name,
                Surname = user.Surname,
                Telephone = user.Telephone,
                BirthDate = user.BirthDate,
                PrimaryAddress = user.PrimaryAddress ?? new Address(),
                SecondaryAddress = user.SecondaryAddress ?? new Address()
            };
        }
    }
}