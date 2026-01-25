using System.Collections.Generic;
using _13.databese;
using _13.Models;
using Npgsql;

namespace _13.Repositories
{
    public class ProductRepository
    {
        public IEnumerable<Product> GetAll()
        {
            var products = new List<Product>();
            using var conn = Database.GetConnection();
            
            using var cmd = new NpgsqlCommand("SELECT id, name, price, image_url FROM products", conn);
            using var reader = cmd.ExecuteReader();
            
            while (reader.Read())
            {
                products.Add(new Product
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Price = reader.GetDecimal(2),
                    ImageUrl = reader.GetString(3)
                });
            }
            return products;
        }
    }
}
