namespace _13.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public Product? Product { get; set; }
        public int Quantity { get; set; }
    }
}
