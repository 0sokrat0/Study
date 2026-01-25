using System;
using System.Linq;
using System.Windows.Input;
using _13.Models;
using _13.Repositories;

namespace _13.ViewModels
{
    public class CheckoutViewModel : ViewModelBase
    {
        private readonly OrderRepository? _orderRepository;
        public ShoppingCartViewModel? ShoppingCart { get; }

        public string? CustomerName { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }

        public ICommand PlaceOrderCommand { get; }
        public ICommand GoBackCommand { get; }

        public CheckoutViewModel(ShoppingCartViewModel shoppingCart, Action goBack)
        {
            _orderRepository = new OrderRepository();
            ShoppingCart = shoppingCart;
            PlaceOrderCommand = new RelayCommand(PlaceOrder);
            GoBackCommand = new RelayCommand(_ => goBack());
        }

        public CheckoutViewModel()
        {
            // For designer
            PlaceOrderCommand = new RelayCommand(_ => { });
            GoBackCommand = new RelayCommand(_ => { });
        }

        private void PlaceOrder(object? parameter)
        {
            if (ShoppingCart != null && _orderRepository != null)
            {
                var order = new Order
                {
                    CustomerName = CustomerName,
                    Email = Email,
                    Address = Address,
                    OrderItems = ShoppingCart.Items.Select(vm => new OrderItem { Product = vm.Product, Quantity = 1 }).ToList(),
                    TotalPrice = ShoppingCart.TotalPrice
                };
                _orderRepository.Save(order);
            }
        }
    }
}
