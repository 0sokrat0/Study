using _13.Models;

namespace _13.ViewModels
{
    public class ProductViewModel
    {
        public Product Product { get; }

        public ProductViewModel(Product product)
        {
            Product = product;
        }

        public string Name => Product.Name;
        public decimal Price => Product.Price;
        public string ImageUrl => Product.ImageUrl;
    }
}
