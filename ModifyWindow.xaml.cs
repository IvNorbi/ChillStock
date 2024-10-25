using System;
using System.Linq;
using System.Windows;
using IN_bemutato.Models; 

namespace IN_bemutato
{
    public partial class ModifyWindow : Window
    {
        private Customer _customer;
        private Meat _meat; 

        // Konstruktor ügyfélhez
        public ModifyWindow(Customer customer)
        {
            InitializeComponent();
            _customer = customer;

            // Mezők inicializálása:
            NameTextBox.Text = _customer.CustomerName ?? string.Empty;
            StockTextBox.Text = _customer.TotalOrders?.ToString() ?? string.Empty;
            LastArrivalTextBox.Text = _customer.LastOrderDate?.ToString("yyyy-MM-dd") ?? string.Empty;
        }

        // Konstruktor húsokhoz
        public ModifyWindow(Meat meat)
        {
            InitializeComponent();
            _meat = meat;

            // Mezők inicializálása:
            NameTextBox.Text = _meat.MeatType ?? string.Empty; // Használjuk az _meat változót
            StockTextBox.Text = _meat.CurrentStock.ToString();
            LastArrivalTextBox.Text = _meat.LastArrivalDate?.ToString("yyyy-MM-dd") ?? string.Empty;
            PricePerKgTextBox.Text = _meat.PricePerKg.ToString(); // Ha nincs formázás


        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_customer != null) // Ha _customer nem null, akkor ügyfelet módosítunk
            {
                _customer.CustomerName = NameTextBox.Text;
                _customer.TotalOrders = int.TryParse(StockTextBox.Text, out int totalOrders) ? totalOrders : (int?)null;
                _customer.LastOrderDate = DateOnly.TryParse(LastArrivalTextBox.Text, out DateOnly lastOrderDate) ? lastOrderDate : (DateOnly?)null;

                using (var context = new NvbemutatoContext())
                {
                    context.Customers.Update(_customer);
                    context.SaveChanges(); // Módosítások mentése
                }
            }
            else if (_meat != null) // Ha _meat nem null, akkor húst módosítunk
            {
                _meat.MeatType = NameTextBox.Text;
                _meat.CurrentStock = int.TryParse(StockTextBox.Text, out int currentStock) ? currentStock : 0;
                _meat.LastArrivalDate = DateOnly.TryParse(LastArrivalTextBox.Text, out DateOnly lastArrivalDate) ? lastArrivalDate : (DateOnly?)null;
                _meat.PricePerKg = decimal.TryParse(PricePerKgTextBox.Text, out decimal pricePerKg) ? pricePerKg : 0;

                using (var context = new NvbemutatoContext())
                {
                    context.Meats.Update(_meat);
                    context.SaveChanges(); // Módosítások mentése
                }
            }

            this.DialogResult = true; // A módosítás sikeres
            this.Close();
        }

    }
}
