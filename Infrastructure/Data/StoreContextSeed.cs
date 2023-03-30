using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context)
        {
            if (!context.ProductBrands.Any())
            {
                var brandsData = File.ReadAllText("../Infrastructure/Data/SeedData/brands.json");

                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                foreach (var item in brands)
                {
                    string query = "SET IDENTITY_INSERT ProductBrands ON " +
                                   "Insert into ProductBrands(Id, Name) " +
                                   $"Values({item.Id}, '{item.Name}')";

                     context.Database.ExecuteSqlRaw(query);
                }
                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT ProductBrands OFF");

                await context.SaveChangesAsync();
            }

            if (!context.ProductTypes.Any())
            {
                var typesData = File.ReadAllText("../Infrastructure/Data/SeedData/types.json");

                var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                foreach (var item in types)
                {
                    string query = "SET IDENTITY_INSERT ProductTypes ON " +
                                   "Insert into ProductTypes(Id, Name) " +
                                   $"VALUES({item.Id}, '{item.Name}')";

                    context.Database.ExecuteSqlRaw(query);
                }
                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT ProductTypes OFF");

                await context.SaveChangesAsync();
            }

            if (!context.Products.Any())
            {
                var productsData = File.ReadAllText("../Infrastructure/Data/SeedData/products.json");

                var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                string truncateTableQuery = "TRUNCATE TABLE PRODUCTS";
                await context.Database.ExecuteSqlRawAsync(truncateTableQuery);
                foreach (var item in products)
                {
                    await context.Products.AddAsync(item);
                }
                await context.SaveChangesAsync();
            }

            if (context.ChangeTracker.HasChanges()) await context.SaveChangesAsync();
        }
    }
}
