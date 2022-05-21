using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ApiDog.Dto;
using ApiDog.Services.Interfaces;
using DesktopApp.Repository;

namespace DesktopApp.Forms
{
    
    public partial class AddContractWindow : Window
    {
        private readonly IRepositoryContract _repositoryContract;
        private readonly IRepositoryCustomer _repositoryCustomers;
        private IList<CustomerDto>? _customerDtos;
        private readonly Action _updateContractList;
        public AddContractWindow(Action updateContractList, IRepositoryContract repositoryContract, IRepositoryCustomer repositoryCustomers)
        {
            _updateContractList += updateContractList;
            _repositoryContract = repositoryContract;
            _repositoryCustomers = repositoryCustomers;
            _customerDtos = new List<CustomerDto>();
            InitializeComponent();
            UpdateList();
        }

        private void AddContractWindows_OnClosed(object? sender, EventArgs e)
        {
            //Owner.Show();
        }

        private void ButtonClose_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void UpdateList()
        {
            if (Services.GetRoleAndName().Role == "Administrator") return;
            TextBoxDescriptionBuh.Visibility = Visibility.Collapsed;
            TextBoxPercent.Visibility = Visibility.Collapsed;
            TextBoxPayedDate.Visibility = Visibility.Collapsed;
            TextBlockPercent.Visibility = Visibility.Collapsed;
            TextBlockPayedDate.Visibility = Visibility.Collapsed;
            TextBlockDescriptionBuh.Visibility = Visibility.Collapsed;
            
        }

        private async void ButtonAddContract_OnClick(object sender, RoutedEventArgs e)
        {
            await AddOrEditContractAsync();
        }
        
        private void ButtonFiltr_OnClick(object sender, RoutedEventArgs e)
        {
            ComboBoxCustomers.ItemsSource =
                _customerDtos?.Where(n => n.Name.ToLower().Contains(ComboBoxCustomers.Text.ToLower())).Select(m=>m.Name);
            ComboBoxCustomers.IsDropDownOpen = true;
        }

        private async void ButtonCopyContract_OnClick(object sender, RoutedEventArgs e)
        {
            TextBlockIdContract.Text = "0";
            TextBoxNumberContract.Text += " - копия";
            await AddOrEditContractAsync();
        }

        private async Task AddOrEditContractAsync()
        {
            var contract = new ContractDto();

            if (TextBlockIdContract.Text == "") TextBlockIdContract.Text = "0";
            contract.Id = Convert.ToInt32(TextBlockIdContract.Text);

            //if (contract.Id == 0 && _repositoryContract.GetIdByName(TextBoxNumberContract.Text) > 0)
            //{
            //    MessageBox.Show($"Номер для договора {TextBoxNumberContract.Text} уже занят");
            //}
            //else
            //{
                contract.Number = TextBoxNumberContract.Text;
            
                if (!DateTime.TryParse(DatePickerContractDate.SelectedDate.ToString(), out var contractDate))
                {
                    MessageBox.Show("Неверная дата договора, дата изменена");
                    DatePickerContractDate.SelectedDate  = new DateTime(DateTime.Now.Year, 1, 1);
                    return;
                }
                contract.Date = contractDate;
                
                var idCustomer = await _repositoryCustomers.GetIdByNameAsync(ComboBoxCustomers.Text);
                if (idCustomer == 0)
                {
                    MessageBox.Show("Не найден заказчик");
                    return;
                }

                contract.IdCustomer = idCustomer;
                
                contract.CustomerName = ComboBoxCustomers.Text;
                
                contract.Solution = TextBoxSolution.Text;

                if (!float.TryParse(TextBoxSumma.Text, out var contractSumma))
                {
                    MessageBox.Show("Неверная сумма договора");
                    TextBoxSumma.Text = "0";
                    return;
                }
                contract.Sum = contractSumma;
                
                contract.Payed = TextBoxPayed.Text;
                contract.Signed = TextBoxSigned.Text;
                contract.Documents = TextBoxDocuments.Text;
                contract.SignedAct = TextBoxActSigned.Text;
                contract.GroupNum = TextBoxGroup.Text;
                contract.IncApplication = TextBoxIncApplication.Text;
                
                if (DateTime.TryParse(DatePickerApplicationDate.SelectedDate.ToString(), out var contractDateIncApplication))
                {
                    contract.DateIncApplication = contractDateIncApplication;
                }
                
                contract.CountPerson = TextBoxCountPerson.Text;

                contract.Description = TextBoxDescription.Text;
                
                contract.DescriptionBuh = TextBoxDescriptionBuh.Text;
               
                contract.DatePayed = TextBoxPayedDate.Text;
                
                contract.Percent = TextBoxPercent.Text;
                
                if (contract.Id == 0) //новый договор
                {
                    contract.DateCreate = DateTime.Now;
                    contract.CreateUser = Services.GetRoleAndName().Name;
                    
                    contract.DateEdit = DateTime.Now;
                    contract.EditUser = Services.GetRoleAndName().Name;
                    
                    await _repositoryContract.AddAsync(contract);
                    Close();
                }
                else //редактирование договора
                {
                    DateTime.TryParse(TextBlockDateCreate.Text, out var dateCreateContract);
                    contract.DateCreate = dateCreateContract;
                    contract.CreateUser = TextBlockCreateUser.Text;
                    
                    contract.EditUser = Services.GetRoleAndName().Name;
                    contract.DateEdit = DateTime.Now;
                    
                    await _repositoryContract.UpdateAsync(contract);
                    Close();
                }
                _updateContractList.Invoke();
            //}
        }

        private async void AddContractWindows_OnLoaded(object sender, RoutedEventArgs e)
        {
            _customerDtos =  await _repositoryCustomers.GetAsync("", DateTime.MinValue, DateTime.MaxValue,0);
            ComboBoxCustomers.ItemsSource = _customerDtos?.Select(x => x.Name);
        }

    }
}