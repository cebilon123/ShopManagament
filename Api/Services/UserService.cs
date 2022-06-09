using System.Collections.Generic;
using System.Linq;
using Api.Commands;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Services
{
    public class UserService
    {
        private readonly DatabaseContext _context;

        // tutaj tak samo wstrzykujemy tylko dbContext
        public UserService(DatabaseContext context)
        {
            _context = context;
        }
        
        /// <summary>
        /// Zarejestruj uzytkownika. 
        /// </summary>
        /// <param name="command">Komenda która wysyla sie w body do endpointa</param>
        /// <returns>True kiedy uzytkownik zostal stworzony, false, kiedy nie zostal stworzony</returns>
        public bool RegisterUser(RegisterUserCommand command)
        {
                       // select from Users where Users.Login == command.login
            var user = _context.Users.FirstOrDefault(u => u.Login == command.Login);

            if (user != null)
                return false;

            if (command.PrimaryAddress is null)
                return false;

            user = new User()
            {
                Email = command.Email,
                Login = command.Login,
                Name = command.Name,
                Password = command.Password,
                Surname = command.Surname,
                Telephone = command.Telephone,
                BirthDate = command.BirthDate,
                PrimaryAddress = command.PrimaryAddress,
                SecondaryAddress = command.SecondaryAddress,
            };

            _context.Users.Add(user);
            _context.SaveChanges();
            
            return true;
        }

        /// <summary>
        /// Zaloguj uzytkownika na podstawie danych z komendy
        /// </summary>
        /// <param name="command">Komenda do logowania</param>
        /// <returns>Result.User jesli poprawnie zalogowano, null jestli niepoprawne dane</returns>
        public Result.User Login(LoginUserCommand command)
        {
            var user = _context.Users
                .Include(u => u.PrimaryAddress)
                .Include(u => u.SecondaryAddress)
                .FirstOrDefault(u => u.Login == command.Login);

            if (user is null) return null;

            if (user.Password == command.Password) return Result.User.FromDatabase(user);
            
            return null;
        }

        public IEnumerable<Result.User> GetAllUsers(int page, int resultsPerPage)
        {
            var users = _context.Users.AsNoTracking()
                .Skip(page * resultsPerPage)
                .Take(resultsPerPage)
                .ToList();

            return users.Select(Result.User.FromDatabase);
        }
    }
}