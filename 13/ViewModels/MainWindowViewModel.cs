using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using _13.Models;
using _13.Repositories;

namespace _13.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly ProductRepository _productRepository;
        private readonly OrderRepository _orderRepository;

        private object? _currentViewModel;
        public object? CurrentViewModel
        {
            get => _currentViewModel;
            set => SetProperty(ref _currentViewModel, value);
        }

        public ObservableCollection<ProductViewModel> Products { get; } = new();
        public ShoppingCartViewModel ShoppingCart { get; } = new();

        public ICommand AddToCartCommand { get; }
        public ICommand CheckoutCommand { get; }

        public MainWindowViewModel()
        {
            _productRepository = new ProductRepository();
            _orderRepository = new OrderRepository();

            foreach (var product in _productRepository.GetAll())
            {
                Products.Add(new ProductViewModel(product));
            }

            AddToCartCommand = new RelayCommand(AddToCart);
            CheckoutCommand = new RelayCommand(Checkout);

            CurrentViewModel = this;
        }

        private void AddToCart(object? parameter)
        {
            if (parameter is ProductViewModel productViewModel)
            {
                ShoppingCart.AddToCart(productViewModel.Product);
            }
        }

        private void Checkout(object? parameter)
        {
            CurrentViewModel = new CheckoutViewModel(ShoppingCart, GoBack);
        }

        private void GoBack()
        {
            CurrentViewModel = this;
        }
    }
}
