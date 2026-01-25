using _13.databese;
using _13.Models;
using Npgsql;

namespace _13.Repositories
{
    public class OrderRepository
    {
        public void Save(Order order)
        {
            using var conn = Database.GetConnection();
            using var cmd = new NpgsqlCommand("INSERT INTO orders (customer_name, email, address, total_price) VALUES (@customer_name, @email, @address, @total_price) RETURNING id", conn);
            cmd.Parameters.AddWithValue("customer_name", order.CustomerName);
            cmd.Parameters.AddWithValue("email", order.Email);
            cmd.Parameters.AddWithValue("address", order.Address);
            cmd.Parameters.AddWithValue("total_price", order.TotalPrice);
            var orderId = (int)cmd.ExecuteScalar();

            foreach (var item in order.OrderItems)
            {
                using var itemCmd = new NpgsqlCommand("INSERT INTO order_items (order_id, product_id, quantity) VALUES (@order_id, @product_id, @quantity)", conn);
                itemCmd.Parameters.AddWithValue("order_id", orderId);
                itemCmd.Parameters.AddWithValue("product_id", item.Product.Id);
                itemCmd.Parameters.AddWithValue("quantity", item.Quantity);
                itemCmd.ExecuteNonQuery();
            }
        }
    }
}
