using System.Collections.ObjectModel;
using System.Linq;
using _13.Models;

namespace _13.ViewModels
{
    public class ShoppingCartViewModel : ViewModelBase
    {
        private readonly ObservableCollection<ProductViewModel> _items;

        public ShoppingCartViewModel()
        {
            _items = new ObservableCollection<ProductViewModel>();
        }

        public ObservableCollection<ProductViewModel> Items => _items;

        public decimal TotalPrice => _items.Sum(x => x.Price);

        public void AddToCart(Product product)
        {
            var existingItem = _items.FirstOrDefault(x => x.Product.Id == product.Id);
            if (existingItem != null)
            {
                
            }
            else
            {
                _items.Add(new ProductViewModel(product));
            }
        }

        public void RemoveFromCart(ProductViewModel item)
        {
            _items.Remove(item);
        }
    }
}
