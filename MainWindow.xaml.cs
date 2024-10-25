using IN_bemutato.Models;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IN_bemutato
{

    public partial class MainWindow : Window
    {
        private List<Customer> customers;
        private List<Meat> meats;

        public MainWindow()
        {
            InitializeComponent();
            LoadData();
            PlaceholderText.Visibility = Visibility.Visible; // Alapértelmezés szerint látható
        }

        private void LoadData()
        {
            using (var context = new NvbemutatoContext())
            {
                customers = context.Customers.ToList();
                meats = context.Meats.ToList();
            }
        }


        private void OnListazClick(object sender, RoutedEventArgs e)
        {
            string filterType = ((ComboBoxItem)FilterComboBox.SelectedItem).Content.ToString();
            var results = new List<ResultItem>();

            ResultsDataGrid.Columns.Clear();

            ResultsDataGrid.AutoGenerateColumns = false;

            if (filterType == "All" || filterType == "Customers")
            {
                results.AddRange(customers
                    .Select(c => new ResultItem
                    {
                        Id = c.CustomerId,
                        NameOrType = c.CustomerName,
                        Address = c.Address,
                        ContactNumber = c.ContactNumber,
                        LastOrderDate = c.LastOrderDate,
                        TotalOrders = c.TotalOrders,
                        IsMeat = false,
                        IsCustomer = true
                    }));

                // Customers mezők
                ResultsDataGrid.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new Binding("Id"), Width = 50 });
                ResultsDataGrid.Columns.Add(new DataGridTextColumn { Header = "Customer Name", Binding = new Binding("NameOrType"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
                ResultsDataGrid.Columns.Add(new DataGridTextColumn { Header = "Address", Binding = new Binding("Address"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
                ResultsDataGrid.Columns.Add(new DataGridTextColumn { Header = "Contact Number", Binding = new Binding("ContactNumber"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
                ResultsDataGrid.Columns.Add(new DataGridTextColumn { Header = "Last Order Date", Binding = new Binding("LastOrderDate"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
                ResultsDataGrid.Columns.Add(new DataGridTextColumn { Header = "Total Orders", Binding = new Binding("TotalOrders"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            }

            if (filterType == "All" || filterType == "Meats")
            {
                results.AddRange(meats
                    .Select(m => new ResultItem
                    {
                        Id = m.MeatId,
                        NameOrType = m.MeatType,
                        Stock = m.CurrentStock,
                        LastArrival = m.LastArrivalDate,
                        PricePerKg = m.PricePerKg,
                        IsMeat = true,
                        IsCustomer = false
                    }));

                // Meats mezők
                ResultsDataGrid.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new Binding("Id"), Width = 50 });
                ResultsDataGrid.Columns.Add(new DataGridTextColumn { Header = "Meat Type", Binding = new Binding("NameOrType"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
                ResultsDataGrid.Columns.Add(new DataGridTextColumn { Header = "Stock", Binding = new Binding("Stock"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
                ResultsDataGrid.Columns.Add(new DataGridTextColumn { Header = "Last Arrival", Binding = new Binding("LastArrival"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
                ResultsDataGrid.Columns.Add(new DataGridTextColumn { Header = "Price per Kg", Binding = new Binding("PricePerKg"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            }

            ResultsDataGrid.ItemsSource = results;
        }






        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            PlaceholderText.Visibility = Visibility.Collapsed; // Helyettesítő szöveg eltüntetése
        }

        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchBox.Text))
            {
                PlaceholderText.Visibility = Visibility.Visible; // Megjeleníti a helyettesítő szöveget, ha üres a mező --> ROSSZ A POZI!
            }
        }

        private void FilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // ????
            OnListazClick(sender, null);
        }

        private void OnSearchClick(object sender, RoutedEventArgs e)
        {
            string searchTerm = SearchBox.Text.Trim().ToLower();
            var results = new List<ResultItem>();

            // Szűrés a customers listából
            results.AddRange(customers
                .Where(c => c.CustomerName.ToLower().Contains(searchTerm))
                .Select(c => new ResultItem
                {
                    Id = c.CustomerId,
                    NameOrType = c.CustomerName,
                    Stock = null,
                    LastArrival = null,
                    PricePerKg = null,
                    CustomerName = c.CustomerName,
                    Address = c.Address,
                    ContactNumber = c.ContactNumber,
                    LastOrderDate = c.LastOrderDate,
                    TotalOrders = c.TotalOrders,
                    IsMeat = false,
                    IsCustomer = true
                }));

            // Szűrés a meats listából
            results.AddRange(meats
                .Where(m => m.MeatType.ToLower().Contains(searchTerm))
                .Select(m => new ResultItem
                {
                    Id = m.MeatId,
                    NameOrType = m.MeatType,
                    Stock = m.CurrentStock,
                    LastArrival = m.LastArrivalDate,
                    PricePerKg = m.PricePerKg,
                    CustomerName = null,
                    Address = null,
                    ContactNumber = null,
                    LastOrderDate = null,
                    TotalOrders = null,
                    IsMeat = true,
                    IsCustomer = false
                }));

            ResultsDataGrid.ItemsSource = results;
        }

        private void ResultsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
 
        }

        private void OnModifyClick(object sender, RoutedEventArgs e)
        {
            var selectedItem = ResultsDataGrid.SelectedItem as ResultItem;

            if (selectedItem != null)
            {
                if (selectedItem.IsCustomer)
                {
                    ModifyWindow modifyWindow = new ModifyWindow(customers.First(c => c.CustomerId == selectedItem.Id));
                    if (modifyWindow.ShowDialog() == true)
                    {
                        LoadData(); // Frissítés
                    }
                }
                else if (selectedItem.IsMeat)
                {
                    ModifyWindow modifyWindow = new ModifyWindow(meats.First(m => m.MeatId == selectedItem.Id));
                    if (modifyWindow.ShowDialog() == true)
                    {
                        LoadData(); // Frissítés
                    }
                }
            }
            else
            {
                MessageBox.Show("Kérjük, válasszon ki egy elemet a módosításhoz.");
            }
        }

        private void OnDeleteClick(object sender, RoutedEventArgs e)
        {
            // Ellenőrzés itt: 
            if (ResultsDataGrid.SelectedItem is ResultItem selectedItem)
            {
                using (var context = new NvbemutatoContext())
                {
                    if (selectedItem.IsCustomer)
                    {
                        // Ügyfél törlése : 
                        var customerToDelete = context.Customers.Find(selectedItem.Id);
                        if (customerToDelete != null)
                        {
                            context.Customers.Remove(customerToDelete);
                        }
                    }
                    else if (selectedItem.IsMeat)
                    {
                        // Hús törlése: 
                        var meatToDelete = context.Meats.Find(selectedItem.Id);
                        if (meatToDelete != null)
                        {
                            context.Meats.Remove(meatToDelete);
                        }
                    }

                    context.SaveChanges(); // Módosítások mentése adatbázisba
                }

                // DataGrid frissítés
                OnListazClick(sender, null);
            }
            else
            {
                MessageBox.Show("Kérjük, válasszon ki egy elemet a törléshez.");
            }
        }



    }

    public class ResultItem
        {
            public int Id { get; set; }
            public string NameOrType { get; set; }
            public float? Stock { get; set; } 
            public DateOnly? LastArrival { get; set; }
            public decimal? PricePerKg { get; set; } 
            public string? CustomerName { get; set; } 
            public string? Address { get; set; } 
            public string? ContactNumber { get; set; } 
            public DateOnly? LastOrderDate { get; set; } 
            public int? TotalOrders { get; set; } 
            public bool IsMeat { get; set; } 
            public bool IsCustomer { get; set; }
    }
}
