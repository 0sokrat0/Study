using System;
using System.Collections.Generic;
using _13.databese;
using _13.Models;
using Npgsql;

namespace _13.Repositories
{
    public class OrderRepository
    {
        public void Save(Order order)
        {
            ArgumentNullException.ThrowIfNull(order);

            using var conn = Database.GetConnection();
            using var cmd = new NpgsqlCommand("INSERT INTO orders (customer_name, email, address, total_price) VALUES (@customer_name, @email, @address, @total_price) RETURNING id", conn);
            cmd.Parameters.AddWithValue("customer_name", order.CustomerName ?? string.Empty);
            cmd.Parameters.AddWithValue("email", order.Email ?? string.Empty);
            cmd.Parameters.AddWithValue("address", order.Address ?? string.Empty);
            cmd.Parameters.AddWithValue("total_price", order.TotalPrice);
            var result = cmd.ExecuteScalar();
            if (result is null || result is DBNull)
            {
                throw new InvalidOperationException("Failed to create order. No id returned.");
            }

            var orderId = Convert.ToInt32(result);

            foreach (var item in order.OrderItems ?? new List<OrderItem>())
            {
                var productId = item.Product?.Id
                    ?? throw new InvalidOperationException("Order item product is missing.");

                using var itemCmd = new NpgsqlCommand("INSERT INTO order_items (order_id, product_id, quantity) VALUES (@order_id, @product_id, @quantity)", conn);
                itemCmd.Parameters.AddWithValue("order_id", orderId);
                itemCmd.Parameters.AddWithValue("product_id", productId);
                itemCmd.Parameters.AddWithValue("quantity", item.Quantity);
                itemCmd.ExecuteNonQuery();
            }
        }
    }
}
