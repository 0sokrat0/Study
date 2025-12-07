using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Calculator;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void CalculateButton_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(FirstNumberTextBox.Text) || 
                string.IsNullOrWhiteSpace(SecondNumberTextBox.Text))
            {
                ResultTextBox.Text = "Ошибка: Введите оба числа";
                return;
            }

            if (!double.TryParse(FirstNumberTextBox.Text, out double firstNumber))
            {
                ResultTextBox.Text = "Ошибка: Неверный формат первого числа";
                return;
            }

            if (!double.TryParse(SecondNumberTextBox.Text, out double secondNumber))
            {
                ResultTextBox.Text = "Ошибка: Неверный формат второго числа";
                return;
            }

            if (OperationComboBox.SelectedItem == null)
            {
                ResultTextBox.Text = "Ошибка: Выберите операцию";
                return;
            }

            string operation = ((ComboBoxItem)OperationComboBox.SelectedItem).Content?.ToString() ?? "";

            double result = 0;

            switch (operation)
            {
                case "Сложение":
                    result = firstNumber + secondNumber;
                    break;

                case "Вычитание":
                    result = firstNumber - secondNumber;
                    break;

                case "Умножение":
                    result = firstNumber * secondNumber;
                    break;

                case "Деление":
                    if (secondNumber == 0)
                    {
                        ResultTextBox.Text = "Ошибка: Деление на ноль невозможно";
                        return;
                    }
                    result = firstNumber / secondNumber;
                    break;

                case "Остаток от деления":
                    if (secondNumber == 0)
                    {
                        ResultTextBox.Text = "Ошибка: Деление на ноль невозможно";
                        return;
                    }
                    result = firstNumber % secondNumber;
                    break;

                default:
                    ResultTextBox.Text = "Ошибка: Неизвестная операция";
                    return;
            }

            ResultTextBox.Text = result.ToString("F2");
        }
        catch (Exception ex)
        {
            ResultTextBox.Text = $"Ошибка: {ex.Message}";
        }
    }
}

