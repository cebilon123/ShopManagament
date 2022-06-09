using System.Collections.Generic;
using System.Linq;
using Api.Commands;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Product = Api.Result.Product;

namespace DefaultNamespace
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;
        private readonly DatabaseContext _context;

        public ProductsController(ProductService productService, DatabaseContext context)
        {
            _productService = productService;
            _context = context;
        }

        /// <summary>
        /// Dodajemy produkt na podstawie modelu z kommendy
        /// </summary>
        [HttpPost]
        public ActionResult<int> CreateProduct(CreateProductCommand command)
        {
            var id = _productService.AddProduct(command);
            return Ok(id);
        }

        /// <summary>
        /// Zwraca wszystkie produkty z naszego sklepu
        /// </summary>
        [HttpGet]
        public ICollection<Product> GetProducts()
        {
            return _productService.GetAllProducts();
        }

        /// <summary>
        /// Zwracamy dane na temat wybranego produktu po id tego produktu podanym jako query param (odpal apkę, patrz
        /// na swagger jak endpoint wyglada)
        /// </summary>
        [HttpGet("{id:int}")]
        public ActionResult<Product?> GetProduct(int id)
        {
            var product = _productService.GetProduct(id);
            return product is null ? NotFound() : Ok(product); // jesli produkt jest null (nie ma takiego) zwraca odpowiedz not found, w innym wypadku zwraca ten produkt
        }

        /// <summary>
        /// Aktualizuj produkt z podanym id
        /// </summary>
        [HttpPut("{id:int}")]
        public ActionResult UpdateProduct(int id, CreateProductCommand command)  // create product poniewaz mozna zaktualizowac kazde pole w produkcie oprócz id
        {
            var isUpdated = _productService.UpdateProduct(id, command);
            return isUpdated ? Ok() : BadRequest("Najprawdopodobniej nie znaleziono produktu z podanym id");
        }

        /// <summary>
        /// Usun produkt z podanym id
        /// </summary>
        [HttpDelete("{id:int}")]
        public ActionResult DeleteProduct(int id)
        {
            var isDeleted = _productService.RemoveProduct(id);
            
            return isDeleted ? Ok() : BadRequest("Najprawdopodobniej nie znaleziono produktu z podanym id");
        }

        [HttpGet("Categories")]
        public IEnumerable<string> GetCategories()
        {
            return _context.Products.Select(p => p.Category).Distinct();
        }
    }
}