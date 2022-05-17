using System.Collections.Generic;
using System.Linq;
using Api.Commands;
using Api.Models;

namespace Api.Services
{
    public class ProductService
    {
        private readonly DatabaseContext _context;

        // wstrzykujemy context tak jak w user service
        public ProductService(DatabaseContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Dodaj nowy produkt.
        /// </summary>
        /// <returns>Zwraca:
        /// id produktu utworzonego</returns>
        public int AddProduct(CreateProductCommand command)
        {
            var product = new Product
            {
                Category = command.Category,
                Description = command.Description,
                Left = command.Left,
                Name = command.Name,
                Price = command.Price,
                Weight = command.Weight
            };

            _context.Add(product);
            _context.SaveChanges();

            return product.Id;
        }

        /// <summary>
        /// Pobierz wszystkie produkty z bazy danych
        /// </summary>
        /// <returns>Zwraca produkt z bazy danych zmapowane na result.Product model (jest to model
        /// do zwracania z api)</returns>
        public ICollection<Result.Product> GetAllProducts()
        {
            return _context.Products
                .Select(product => Result.Product.FromModel(product))// ten select tutaj mapuje tabele produkt na nasz result model produkt
                .ToList(); // tutaj po prostu tą kolekcję mapujemy znów do listy która implementuje interfejs ICollection<Result.Product> (kompilator inaczej wyrzuca błąd)
        }

        /// <summary>
        /// Pobierz dane o wybranym produkcie z bazy danych
        /// </summary>
        /// <param name="id">id produktu</param>
        /// <returns>Zmapowany produkt na result.Product (jak wyzej tylko dla jednego egzemplarza)</returns>
        public Result.Product? GetProduct(int id) // ? oznacza ze z metody moze zostac również zwrócony null
        {
            var product = _context.Products.FirstOrDefault(product => product.Id == id); // to jest SELECT * FROM PRODUCTS WHERE products.Id = id
            
            // czy produkt jest null? Tak: zwróć null : Nie: zwróc ten produkt zmapowany
            return product is null ? null : Result.Product.FromModel(product);
        }

        /// <summary>
        /// Zaktualizuj produkt
        /// </summary>
        /// <param name="id">id produktu do aktualizacji</param>
        /// <param name="command">pola modyfikacji produktu</param>
        /// <returns>false jesli nie ma produktu</returns>
        public bool UpdateProduct(int id, CreateProductCommand command)
        {
            var product = _context.Products.FirstOrDefault(product => product.Id == id);

            if (product is null) return false;

            product.Category = command.Category;
            product.Description = command.Description;
            product.Left = command.Left;
            product.Name = command.Name;
            product.Price = command.Price;
            product.Weight = command.Weight;
            
            _context.Products.Update(product);
            _context.SaveChanges();
            
            return true;
        }

        /// <summary>
        /// Usun produkt z bazy danych
        /// </summary>
        /// <param name="id">id produktu do usuniecia</param>
        /// <returns>true jesli usuniety, false jesli nie znaleziony</returns>
        public bool RemoveProduct(int id)
        {
            var product = _context.Products.FirstOrDefault(product => product.Id == id);

            if (product is null) return false;

            _context.Products.Remove(product);
            _context.SaveChanges();

            return true;
        }
    }
}