using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ApiDog.Dto;
using ApiDog.Services.Interfaces;

namespace DesktopApp.Forms
{
    public partial class ContractsWindow : Window
    {
        private readonly IRepositoryContract _repositoryContract;
        private readonly IRepositoryCustomer _repositoryCustomers;
        private IList<ContractDto>? _contractDtos = new List<ContractDto>();
        public ContractsWindow(IRepositoryContract repositoryContract, IRepositoryCustomer repositoryCustomers)
        {
            _repositoryContract = repositoryContract;
            _repositoryCustomers = repositoryCustomers;
            InitializeComponent();
            DatePickerFrom.DisplayDate = new DateTime(DateTime.Now.Year, 1, 1);
            DatePickerTo.DisplayDate = new DateTime(DateTime.Now.Year, 12, 31);
            ButtonPreviusYear.Content = DateTime.Now.Year - 1;
            ButtonCurrentYear.Content = DateTime.Now.Year;
        }

        private void ContractsWindow_OnClosing(object sender, CancelEventArgs e)
        {
            Owner.Show();
        }

        private async void DatePickerFrom_OnSelectedDateChanged(object? sender, SelectionChangedEventArgs e)
        {
            await UpdateContractListAsync();
        }

        private async void DatePickerTo_OnSelectedDateChanged(object? sender, SelectionChangedEventArgs e)
        {
            await UpdateContractListAsync();
        }

        private async void ButtonCustomerSelect_OnClick(object sender, RoutedEventArgs e)
        {
            if (DataGridContract.SelectedItem == null) return;
            var customer = ((ContractDto)DataGridContract.SelectedItem).CustomerName;
            TextBoxSearch.Text = customer;
            await UpdateContractListAsync();
        }

        private async void ButtonClearSearchText_OnClick(object sender, RoutedEventArgs e)
        {
            TextBoxSearch.Text = "";
            await UpdateContractListAsync();
        }

        private async void ButtonPreviusYear_OnClick(object sender, RoutedEventArgs e)
        {
            ContractListClear();
            DatePickerFrom.DisplayDate = new DateTime(DateTime.Now.Year - 1, 1, 1);
            DatePickerTo.DisplayDate = new DateTime(DateTime.Now.Year - 1, 12, 31);
            await UpdateContractListAsync();
        }

        private async void ButtonCurrentYear_OnClick(object sender, RoutedEventArgs e)
        {
            ContractListClear();
            DatePickerFrom.DisplayDate  = new DateTime(DateTime.Now.Year, 1, 1);
            DatePickerTo.DisplayDate = new DateTime(DateTime.Now.Year, 12, 31);
            await UpdateContractListAsync();
        }

        private async void ButtonAllYears_OnClick(object sender, RoutedEventArgs e)
        {
            ContractListClear();
            DatePickerFrom.DisplayDate = DateTime.MinValue;
            DatePickerTo.DisplayDate = DateTime.MaxValue;
            await UpdateContractListAsync();
        }

        private void ButtonExit_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void TextBoxSearch_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                await UpdateContractListAsync();
            }
        }

        private void DataGridContract_OnSelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (DataGridContract.SelectedItem != null)
            {
                var selectedContract = (ContractDto)DataGridContract.SelectedItem;
                TextBlockCreateUser.Text = selectedContract.CreateUser;
                TextBlockCreateDate.Text = selectedContract.DateCreate.ToString("g");
                TextBlockEditUser.Text = selectedContract.EditUser;
                TextBlockEditDate.Text = selectedContract.DateEdit.ToString("g");
                StackPanelDescription.Visibility = Visibility.Visible;
            }
            else
            {
                StackPanelDescription.Visibility = Visibility.Hidden;
            }
        }

        private void ButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            var addContractWindow = new AddContractWindow(UpdateContractList, _repositoryContract, _repositoryCustomers)
                {
                    //addContractWindow.Owner = this;
                    Topmost = true
                };
            addContractWindow.Show();
        }

        private async Task EditContract()
        {
               
            if (DataGridContract.SelectedItem == null) return;

            if (DataGridContract.SelectedItems.Count > 1)
            {
                var contractList = DataGridContract.SelectedItems;

                var multiEditWindow = new MultiEditContractWindow(contractList, _repositoryContract, _repositoryCustomers)
                {
                    Owner = this,
                    Topmost = true
                };
                multiEditWindow.Show();
                Hide();
            
            }
            else
            {
                ContractDto contract = (ContractDto)DataGridContract.SelectedItem;

                ContractDto contractInDataBase = await _repositoryContract.GetByIdAsync(contract.Id);

                if (contract.DateEdit != contractInDataBase.DateEdit) 
                {
                    MessageBox.Show("Данные были изменены, обновите данные");
                }
                else
                {
                    var editContractWindow = new AddContractWindow(UpdateContractList, _repositoryContract, _repositoryCustomers)
                {
                    //Owner = this,
                    Title = "Редактирование договора",
                    Topmost = true
                };

                editContractWindow.TextBlockIdContract.Text = contract.Id.ToString();
                editContractWindow.TextBoxNumberContract.Text = contract.Number;
                editContractWindow.DatePickerContractDate.SelectedDate = contract.Date;
                editContractWindow.TextBlockIdCustomer.Text = contract.IdCustomer.ToString();
                editContractWindow.ComboBoxCustomers.Text = contract.CustomerName;
                editContractWindow.TextBoxSolution.Text = contract.Solution;
                editContractWindow.TextBoxSumma.Text = contract.Sum.ToString();
                editContractWindow.TextBoxPayed.Text = contract.Payed;
                editContractWindow.TextBoxSigned.Text = contract.Signed;
                editContractWindow.TextBoxDocuments.Text = contract.Documents;
                editContractWindow.TextBoxActSigned.Text = contract.SignedAct;
                editContractWindow.TextBoxGroup.Text = contract.GroupNum;
                editContractWindow.TextBoxIncApplication.Text = contract.IncApplication;
                editContractWindow.DatePickerApplicationDate.SelectedDate = contract.DateIncApplication;
                editContractWindow.TextBoxCountPerson.Text = contract.CountPerson;
                editContractWindow.TextBlockDateCreate.Text = contract.DateCreate.ToString();
                editContractWindow.TextBoxDescription.Text = contract.Description;
                editContractWindow.TextBoxDescriptionBuh.Text = contract.DescriptionBuh;
                editContractWindow.TextBlockCreateUser.Text = contract.CreateUser;
                editContractWindow.TextBoxPayedDate.Text = contract.DatePayed;
                editContractWindow.TextBoxPercent.Text = contract.Percent;
                editContractWindow.TextBlockEditUser.Text = contract.EditUser;
                editContractWindow.ButtonAddContract.Content = "Обновить";
                editContractWindow.ButtonCopyContract.Visibility = Visibility.Visible;
                    
                //Hide();
                    
                editContractWindow.Show();
                }
            }
            
        }
        private async void ButtonEdit_OnClick(object sender, RoutedEventArgs e)
        {
           await EditContract();
        }

       
        private async void DataGridContract_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            await EditContract();
        }

        private async void ButtonDelete_OnClick(object sender, RoutedEventArgs e)
        {
            if (DataGridContract.SelectedItem != null)
            {
                var contract = (ContractDto)DataGridContract.SelectedItem;
                var requiest =
                    MessageBox.Show($"Удалить договор {contract.Number}?",
                        "Подтвердите удаление", MessageBoxButton.YesNo);
                if (requiest == MessageBoxResult.Yes)
                {
                    var deleteResult = await _repositoryContract.DeleteAsync(contract.Id);
                    if (deleteResult == 0)
                    {
                        MessageBox.Show("Удаление не было произведено");
                    }
                }
                await UpdateContractListAsync();
            }
        }

        private async void ButtonUpdate_OnClick(object sender, RoutedEventArgs e)
        {
            ContractListClear();
            await UpdateContractListAsync();
        }

        private async void FilterChange(object sender, RoutedEventArgs routedEventArgs)
        {
            await UpdateContractListAsync();
        }
        
        private void UpdateContractList()
        {
            if (Services.GetRoleAndName().Role != "Administrator")
            {
                DataGridContract.Columns[14].Visibility = Visibility.Collapsed;
                DataGridContract.Columns[15].Visibility = Visibility.Collapsed;
                DataGridContract.Columns[16].Visibility = Visibility.Collapsed;
                StackPanelSumm.Visibility = Visibility.Collapsed;
                StackPanelFilters.Visibility = Visibility.Collapsed;
            }
            var notAct = CheckBoxNotAct.IsChecked is true;
            var notDocuments = CheckBoxNotDocuments.IsChecked is true;
            var notPayed = CheckBoxNotPayed.IsChecked is true;
            var notSigned = CheckBoxNotSigned.IsChecked is true;
            var percentIsTrue = CheckBoxPercent.IsChecked is true;
            _contractDtos = _repositoryContract.Get(TextBoxSearch.Text, DatePickerFrom.DisplayDate,
                DatePickerTo.DisplayDate, notPayed, notSigned, notDocuments, notAct, percentIsTrue);
            DataGridContract.ItemsSource = _contractDtos;
            CalculateSummaryContractsData();
            
        }
        private async Task UpdateContractListAsync()
        {
            if (Services.GetRoleAndName().Role != "Administrator")
            {
                DataGridContract.Columns[14].Visibility = Visibility.Collapsed;
                DataGridContract.Columns[15].Visibility = Visibility.Collapsed;
                DataGridContract.Columns[16].Visibility = Visibility.Collapsed;
                StackPanelSumm.Visibility = Visibility.Collapsed;
                StackPanelFilters.Visibility = Visibility.Collapsed;
            }
            var notAct = CheckBoxNotAct.IsChecked is true;
            var notDocuments = CheckBoxNotDocuments.IsChecked is true;
            var notPayed = CheckBoxNotPayed.IsChecked is true;
            var notSigned = CheckBoxNotSigned.IsChecked is true;
            var percentIsTrue = CheckBoxPercent.IsChecked is true;
            _contractDtos = await _repositoryContract.GetAsync(TextBoxSearch.Text, DatePickerFrom.DisplayDate,
                DatePickerTo.DisplayDate, notPayed, notSigned, notDocuments, notAct, percentIsTrue);
            DataGridContract.ItemsSource = _contractDtos;
            CalculateSummaryContractsData();
        }

        private void CalculateSummaryContractsData()
        {
            if (_contractDtos == null) return;
            TextBlockCountRecords.Text = _contractDtos.Count.ToString();
            float summa = 0;
            float summaPercent = 0;
            
            foreach (var contract in _contractDtos)
            {
                summa += contract.Sum;

                if (float.TryParse(contract.Percent, out var summaPercentTemp))
                {
                    summaPercent += summaPercentTemp;
                }
            }

            TextBlockSumma.Text = summa.ToString("0#,#.00");
            TextBlockSummaPercent.Text = summaPercent.ToString("0#,#.00");
        }
        private async void ContractsWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            await UpdateContractListAsync();
        }

        private void ContractListClear()
        {
            _contractDtos = new List<ContractDto>();
            _contractDtos.Add(new ContractDto(){Number = "Загрузка..."});
            DataGridContract.ItemsSource = _contractDtos;
        }
    }
}