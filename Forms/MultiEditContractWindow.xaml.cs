using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using ApiDog.Dto;
using ApiDog.Services.Interfaces;
using DesktopApp.Repository;

namespace DesktopApp.Forms
{
    public partial class MultiEditContractWindow : Window
    {
        
        private readonly IRepositoryContract _repositoryContracts;
        private readonly IRepositoryCustomer _repositoryCustomers;
        private readonly List<ContractDto> _contractList = new List<ContractDto>();
        
        public MultiEditContractWindow(IEnumerable contractList, IRepositoryContract repositoryContracts, IRepositoryCustomer repositoryCustomers)
        {
            _repositoryContracts = repositoryContracts;
            _repositoryCustomers = repositoryCustomers;

            foreach (var contract in contractList)
            {
                _contractList?.Add((ContractDto)contract);
            }
            InitializeComponent();
            UpdateForm();
        }

        private void UpdateForm()
        {
            if (Services.GetRoleAndName().Role != "Administrator")
            {
                TextBoxDatePay.Visibility = Visibility.Collapsed;
            }
            
            TextBlockTitle.Text = "Пакетное редактирование Договоров: ";
            int i=0;
            foreach (var contract in _contractList)
            {
                var contractDto = (ContractDto)contract;
                TextBlockTitle.Text += contractDto.Number;
                if (i < _contractList.Count-1)
                {
                    TextBlockTitle.Text += ", ";
                }
                i++;
            }
            UpdateListCustomer();
        }
        private void MultiEditContractWindow_OnClosing(object sender, CancelEventArgs e)
        {
            Owner.Show();
        }

        private void ButtonClose_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void ButtonUpdate_OnClick(object sender, RoutedEventArgs e)
        {
            foreach (var contract in _contractList)
            {
                var updateContract = (ContractDto)contract;
                var customerName = (string)ListBoxCustomer.SelectedValue;
                if (!string.IsNullOrEmpty(customerName))
                {
                    updateContract.CustomerName = customerName;
                    updateContract.IdCustomer = await _repositoryCustomers.GetIdByNameAsync(customerName);
                }
                if (!string.IsNullOrEmpty(TextBoxPayed.Text)) updateContract.Payed = TextBoxPayed.Text;
                if (!string.IsNullOrEmpty(TextBoxSigned.Text)) updateContract.Signed = TextBoxSigned.Text;
                if (!string.IsNullOrEmpty(TextBoxSignedAct.Text)) updateContract.SignedAct = TextBoxSignedAct.Text;
                if (!string.IsNullOrEmpty(TextBoxDocuments.Text)) updateContract.Documents = TextBoxDocuments.Text;
                if (!string.IsNullOrEmpty(TextBoxDatePay.Text)) updateContract.DatePayed = TextBoxDatePay.Text;
                updateContract.EditUser = Services.GetRoleAndName().Name;
                var result =await _repositoryContracts.UpdateAsync(updateContract);
                if (result == 0) MessageBox.Show("Обновление не удалось");
            }
            Close();
        }

        private async void UpdateListCustomer()
        {
            ListBoxCustomer.ItemsSource = (await _repositoryCustomers.GetAsync(TextBoxSearchCustomer.Text, DateTime.MinValue, DateTime.MaxValue, 0)).Select(x=>x.Name);
        }
        private void ButtonSearchCustomer(object sender, RoutedEventArgs e)
        {
            UpdateListCustomer();
        }

        private void TextBoxSearchCustomer_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                UpdateListCustomer();
            }
        }
    }
}