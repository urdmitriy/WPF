using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ApiDog.Dto;
using ApiDog.Services.Interfaces;
using DesktopApp.Repository;

namespace DesktopApp.Forms
{
    public partial class CustomersWindow : Window
    {
        private readonly IRepositoryCustomer _repositoryCustomers;
        private List<CustomerDto>? _customers = new List<CustomerDto>();
        public CustomersWindow(IRepositoryCustomer repositoryCustomers)
        {
            _repositoryCustomers = repositoryCustomers;
            InitializeComponent();
        }

        private void CustomersWindow_OnClosing(object sender, CancelEventArgs e)
        {
            Owner.Show();
        }

        // private async void CustomersWindow_OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        // {
        //     if (!((CustomersWindow)sender).IsVisible) return;
        //     _customers.Add(new CustomerDto(){Name = "Загрузка..."});
        //     DataGridCustomers.ItemsSource = _customers;
        //     _customers = await _repositoryCustomers.GetAsync("", DateTime.MinValue, DateTime.MaxValue, 1);
        //     DataGridCustomers.ItemsSource = _customers;
        //     
        //     TextBlockCountCustomers.Text = _customers?.Count.ToString();
        //
        // }

        
        private void ButtonExit_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ButtonAddCustomer_OnClick(object sender, RoutedEventArgs e)
        {
            var addCustomerWindow = new AddCustomerWindow(_repositoryCustomers);
            addCustomerWindow.Owner = this;
            Hide();
            addCustomerWindow.Show();
        }

        private async void ButtonDeleteCustomer_OnClick(object sender, RoutedEventArgs e)
        {
            if (DataGridCustomers.SelectedItem == null)
            {
                MessageBox.Show("Нужно указать объект");
            }
            else
            {
                var customer = (CustomerDto)DataGridCustomers.SelectedItem;

                if (customer.CountContracts > 0)
                {
                    MessageBox.Show("Нельзя удалить организацию, у нее есть договоры или записи в РТН");
                }
                else
                {
                    var answer = MessageBox.Show($"Точно надо удалить \"{customer.Name}\"?",
                        "Нужно подтверждение", MessageBoxButton.YesNo);
                    if (answer != MessageBoxResult.Yes) return;
                
                    await _repositoryCustomers.DeleteAsync(customer.Id);
                    _customers = await _repositoryCustomers.GetAsync("",DateTime.MinValue, DateTime.MaxValue, 1);
                    UpdateDataGreed(TextBoxSearch.Text);
                    //_repositoryCustomer.UpdateGrid(DataGridCustomers,TextBoxSearch.Text);
                    //TextBlockCountCustomers.Text = _repositoryCustomer.countCustomers.ToString();
                }
            }
        }

        private void TextBoxSearch_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var searchValue = TextBoxSearch.Text.ToLower();
            UpdateDataGreed(searchValue);
        }

        private void ButtonClearSearch_OnClick(object sender, RoutedEventArgs e)
        {
            TextBoxSearch.Text = "";
            UpdateDataGreed(null);
        }

        private void UpdateDataGreed(string? search)
        {
            search ??= "";
            var searchValue = search.ToLower();
            var  result =  _customers?.Where(s => s.Name.ToLower().Contains(searchValue));
            if (result == null) return;
            IEnumerable<CustomerDto>? customers = result.ToList();
            DataGridCustomers.ItemsSource = customers;
            TextBlockCountCustomers.Text = customers.Count().ToString();
        }

        private void ButtonEditCustomer_OnClick(object sender, RoutedEventArgs e)
        {
            if (DataGridCustomers.SelectedItem==null) return;
            var editCustomer = (CustomerDto)DataGridCustomers.SelectedItem;
            
            var editCustomerWindow = new AddCustomerWindow(_repositoryCustomers)
            {
                Owner = this,
                Title = "Редактирование заказчика", 
            };
            editCustomerWindow.TextBoxNameCustomer.Text = editCustomer.Name;
            editCustomerWindow.TextBoxDescriptionCustomer.Text = editCustomer.Description;
            editCustomerWindow.TextBlockIdCustomer.Text = editCustomer.Id.ToString();
            editCustomerWindow.DataGridCustomers.Visibility = Visibility.Hidden;
            editCustomerWindow.ButtonAddCustomer.Content = "Обновить";
            Hide();
            editCustomerWindow.Show();
        }

        private void DataGridCustomers_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var customer = (CustomerDto)DataGridCustomers.SelectedItem;
            
        }

        private async void ButtonUpdate_OnClick(object sender, RoutedEventArgs e)
        {
            _customers = await _repositoryCustomers.GetAsync("", DateTime.MinValue, DateTime.MaxValue, 1);
            UpdateDataGreed(null);
        }

        private async void CustomersWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            _customers.Add(new CustomerDto(){Name = "Загрузка..."});
            DataGridCustomers.ItemsSource = _customers;
            _customers = await _repositoryCustomers.GetAsync("", DateTime.MinValue, DateTime.MaxValue, 1);
            DataGridCustomers.ItemsSource = _customers;
            
            TextBlockCountCustomers.Text = _customers?.Count.ToString();
        }
    }
}