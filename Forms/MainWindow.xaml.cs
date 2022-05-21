using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ApiDog.Dto;
using ApiDog.Services;
using ApiDog.Services.Interfaces;
using DesktopApp.Repository;

namespace DesktopApp.Forms
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IRepositoryCustomer _repositoryCustomer;
        private readonly IRepositoryContract _repositoryContract;
        private readonly IRepositoryEvent _repositoryEvent;
        private readonly IRepositoryHistory _repositoryHistory;
        private readonly IRepositoryPerson _repositoryPerson;
        private List<CustomerDto>? _customerDtos;
        public MainWindow(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _repositoryCustomer = new RepositoryCustomers(_httpClientFactory);
            _repositoryContract = new RepositoryContracts(_httpClientFactory);
            _repositoryEvent = new RepositoryEvents(_httpClientFactory);
            _repositoryHistory = new RepositoryHistorys(_httpClientFactory);
            _repositoryPerson = new RepositoryPersons(_httpClientFactory);
            InitializeComponent();
        }

        private async Task UpdateCustomerList()
        {
            _customerDtos = await _repositoryCustomer.GetAsync("", DateTime.MinValue, DateTime.MaxValue, 1);
        }

        private void ButtonCustomersClick(object sender, RoutedEventArgs e)
        {
            var customersWindow = new CustomersWindow(_repositoryCustomer);
            Hide();
            customersWindow.Owner = this;
            customersWindow.Show();
        }

        private void ButtonContractsClick(object sender, RoutedEventArgs e)
        {
            var contractWindow = new ContractsWindow(_repositoryContract, _repositoryCustomer);
            contractWindow.Owner = this;
            Hide();
            contractWindow.Show();
        }
        
        private void ButtonRTN_OnClick(object sender, RoutedEventArgs e)
        {
            var rtnWindow = new RTNWindow(_repositoryEvent, _repositoryPerson, _repositoryCustomer, _repositoryHistory);
            rtnWindow.Owner = this;
            
            Hide();
            rtnWindow.Show();
        }
        
        private void ButtonExitClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
        
        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            Owner.Close();
        }

        private async void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            await UpdateCustomerList();
        }
    }
}