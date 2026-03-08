using Avalonia.Controls;
using Avalonia.Interactivity;
using RoguelikeGame.Models.Items;

namespace RoguelikeGame.Views;

public partial class ItemPickupDialog : Window
{
    public ItemPickupDialog(Item oldItem, Item newItem)
    {
        InitializeComponent();
        OldItemText.Text = oldItem.GetDescription();
        NewItemText.Text = newItem.GetDescription();
    }

    private void OnTakeClick(object? sender, RoutedEventArgs e) => Close(true);

    private void OnDiscardClick(object? sender, RoutedEventArgs e) => Close(false);
}
