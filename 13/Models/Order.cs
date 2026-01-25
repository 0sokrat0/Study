using System.Collections.Generic;

namespace _13.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string? CustomerName { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public List<OrderItem>? OrderItems { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
