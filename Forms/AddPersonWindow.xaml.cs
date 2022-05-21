using System;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Windows;
using ApiDog.Dto;
using ApiDog.Services.Interfaces;
using DesktopApp.Repository;

namespace DesktopApp.Forms
{
    public partial class AddPersonWindow : Window
    {
        private readonly IRepositoryCustomer _repositoryCustomers;
        private readonly IRepositoryPerson _repositoryPerson;
        public AddPersonWindow(IRepositoryCustomer repositoryCustomers, IRepositoryPerson repositoryPerson)
        {
            _repositoryCustomers = repositoryCustomers;
            _repositoryPerson = repositoryPerson;

            InitializeComponent();
            UpdateListCustomers();
        }

        private void AddPersonWindow_OnClosing(object sender, CancelEventArgs e)
        {
            Owner.Show();
        }

        private void ButtonClose_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void ButtonAddPerson_OnClick(object sender, RoutedEventArgs e)
        {
            var newPerson = new PersonDto()
            {
                Access = TextBoxAccess.Text,
                Application = TextBoxApplication.Text,
                Approval = TextBoxApproval.Text,
                Attorney = TextBoxAttorney.Text,
                Certification = TextBoxCertification.Text,
                Customer = ComboBoxCustomers.Text,
                Email = TextBoxEmail.Text,
                Note = TextBoxNote.Text,
                Phone = TextBoxPhone.Text,
                Post = TextBoxPost.Text,
                Questionnaire = TextBoxQuestionnaire.Text,
                FCs = TextBoxFCs.Text,
                IdentityCard = TextBoxIdentityCard.Text,
                PaymentCard = TextBoxPaymentCard.Text,
                GroupNum = TextBoxGroup.Text
            };

            if (TextBlockIdPerson.Text != "")
            {
                newPerson.Id = Convert.ToInt32(TextBlockIdPerson.Text);
            }
            newPerson.CustomerId = await _repositoryCustomers.GetIdByNameAsync(newPerson.Customer);

            //DateTime.TryParse(DatePickerExam.SelectedDate.ToString(), out var dateExam);
            TimeSpan.TryParse(TextBoxTimeExam.Text, out var timeExam);
            DateTime.TryParse(DatePickerDateSub.SelectedDate.ToString(), out var dateSub);
            DateTime.TryParse(DatePickerExam.SelectedDate.ToString(), out var dateExam);
            
            
            newPerson.DateOfSubmission = dateSub;
            newPerson.DateEzam = dateExam.Add(timeExam);

            int result;

            if (newPerson.CustomerId != 0)
            {
                if (newPerson.Id == 0)
                {
                    result = await _repositoryPerson.AddAsync(newPerson);
                }
                else
                {
                    result = await _repositoryPerson.UpdateAsync(newPerson);
                }
            
                if (result != 1) MessageBox.Show("Не удалось добавить/изменить персону");
                Close();
            }
            else
            {
                MessageBox.Show($"Организации {newPerson.Customer} нет в базе");
            }
            
            
        }

        private void ButtonFiltr_OnClick(object sender, RoutedEventArgs e)
        {
            UpdateListCustomers();
            ComboBoxCustomers.IsDropDownOpen = true;
        }
        private async void UpdateListCustomers()
        {
            var customersList = await _repositoryCustomers.GetAsync(ComboBoxCustomers.Text,DateTime.Now, DateTime.Now, 0);
            if (customersList != null)
                ComboBoxCustomers.ItemsSource = customersList.Select(x => x.Name);
        }
    }
}