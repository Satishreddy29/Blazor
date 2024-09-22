using Microsoft.EntityFrameworkCore;
using CustomerBlazorAuthApi.Models;

namespace CustomerBlazorAuthApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Seed();
        }

        public DbSet<User> Users { get; set; }
    }
}


// public class OrderContext : DbContext
//     {
//         public OrderContext(DbContextOptions<OrderContext> options) : base(options) { }

//         protected override void OnModelCreating(ModelBuilder modelBuilder)
//         {
//             modelBuilder.Entity<Category>()
//                 .HasMany(c => c.Products)
//                 .WithOne(a => a.Category)
//                 .HasForeignKey(a => a.CategoryId);

//             modelBuilder.Seed();
//         }

//         public DbSet<Product> Products { get; set; }
//         public DbSet<Category> Categories { get; set; }
//     }