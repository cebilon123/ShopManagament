using Microsoft.EntityFrameworkCore;

namespace Api.Models
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderArchive> OrderArchives { get; set; }
        public DbSet<OrderedProduct> OrderedProducts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }

        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }

        public void ArchiveOrder(int orderId)
        {
            Database.ExecuteSqlRaw($"EXEC [dbo].[TempArchiveOrder] @orderId={orderId}");
            SaveChanges();
        }
    }
}