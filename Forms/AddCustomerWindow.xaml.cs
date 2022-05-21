using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ApiDog.Dto;
using ApiDog.Services.Interfaces;
using DesktopApp.Repository;
using DesktopApp.Validation;

namespace DesktopApp.Forms
{
    public partial class AddCustomerWindow : Window
    {
        private readonly IRepositoryCustomer _repositoryCustomers;
        private List<CustomerDto>? _customers;
        public AddCustomerWindow(IRepositoryCustomer repositoryCustomers)
        {
            _repositoryCustomers = repositoryCustomers;
            InitializeComponent();
        }

        private void AddCustomerWindow_OnClosed(object? sender, EventArgs e)
        {
            Owner.Show();
        }

        private void TextBoxNameCustomer_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (_customers==null) return;
            if (_customers.Count > 0 && TextBoxNameCustomer.Text.Length > 0)
            {
                DataGridCustomers.Visibility = Visibility.Visible;
                DataGridCustomers.ItemsSource = _customers.Where(s=>s.Name.ToLower().Contains
                    (TextBoxNameCustomer.Text.ToLower()));
            }
            else
            {
                DataGridCustomers.Visibility = Visibility.Hidden;
            }
        }

        private async void ButtonAddCustomer_OnClick(object sender, RoutedEventArgs e)
        {

            
            var newCustomer = new CustomerDto()
            {
                Id = Convert.ToInt32(TextBlockIdCustomer.Text),
                Name = TextBoxNameCustomer.Text,
                Description = TextBoxDescriptionCustomer.Text
            };
            
            var validation = new ValidationCustomer();
            var resultValid = validation.Validate(newCustomer);
            if (!resultValid.IsValid)
            {
                foreach (var variable in resultValid.Errors)
                {
                    MessageBox.Show(variable.ErrorMessage);
                }
                return;
            }

            if (newCustomer.Id == 0) //если новый заказчик
            {
                var idCustomer = await _repositoryCustomers.GetIdByNameAsync(newCustomer.Name);
                if (idCustomer!=0)
                {
                    MessageBox.Show("Такое наименование уже есть в базе");
                    return; //Если в базе уже есть, выходим
                }
                var result = await _repositoryCustomers.AddAsync(newCustomer);
                if (result == 0) MessageBox.Show("Не добавлено");
            }
            else //если редактирование заказчика
            {
                var idCustomer = await _repositoryCustomers.GetIdByNameAsync(newCustomer.Name);
                if (idCustomer!=0 && idCustomer!=newCustomer.Id)
                {
                    MessageBox.Show("Такое наименование уже есть в базе");
                    return; //Если в базе уже есть, выходим
                }
                var result = await _repositoryCustomers.UpdateAsync(newCustomer);
                if (result != 1) MessageBox.Show("Обновление не удалось");
            }
            
            Close();
        }

        private void ButtonExit_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void AddCustomerWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            await UpdateCustomerList();
        }

        private async Task UpdateCustomerList()
        {
            _customers = await _repositoryCustomers.GetAsync("", DateTime.MinValue, DateTime.MaxValue, 0);
        }
    }
}