using System;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ApiDog.Dto;
using ApiDog.Services.Interfaces;
using DesktopApp.Repository;

namespace DesktopApp.Forms
{
    public partial class RTNWindow : Window
    {
        private readonly IRepositoryEvent _repositoryEvents;
        private readonly IRepositoryPerson _repositoryPersons;
        private readonly IRepositoryCustomer _repositoryCustomers;
        private readonly IRepositoryHistory _repositoryHistory;
        public RTNWindow(IRepositoryEvent repositoryEvents, IRepositoryPerson repositoryPersons, IRepositoryCustomer repositoryCustomers, IRepositoryHistory repositoryHistory)
        {
            _repositoryEvents = repositoryEvents;
            _repositoryPersons = repositoryPersons;
            _repositoryCustomers = repositoryCustomers;
            _repositoryHistory = repositoryHistory;
            InitializeComponent();
        }

        private void RTNWindow_OnClosing(object sender, CancelEventArgs e)
        {
            Owner.Show();
        }

        private void RTNWindow_OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateList();
        }

        private async void UpdateList()
        {
            StackPanelEvents.Children.Clear();
            GroupBoxPersonInfo.Visibility = Visibility.Collapsed;
            
            var buttonAllEvents = new Button()
            {
                Name = "ButtonAll", Content = "Все события",
                Margin = new Thickness(0, 2, 0, 2)
            };
            buttonAllEvents.Click += ButtonAllEventClick;
            
            StackPanelEvents.Children.Add(buttonAllEvents);

            var eventList = await _repositoryEvents.GetAsync("");

            if (eventList != null)
                foreach (var eventName in eventList)
                {
                    if (await _repositoryPersons.CountPersonWithEventAsync(eventName.Id) > 0)
                    {
                        var button = new Button()
                        {
                            Name = $"ButtonEventId{eventName.Id}",
                            Content = eventName.Name,
                            Margin = new Thickness(0, 2, 0, 2),
                        };
                        button.Click += ButtonEventClick;
                        if (button.Content.ToString() == TextBlockSelectedEvent.Text)
                        {
                            button.Background=Brushes.ForestGreen;
                            button.BorderBrush=Brushes.ForestGreen;
                        }
                        
                        StackPanelEvents.Children.Add(button);
                    }
                }
            

            if (TextBlockSelectedCustomer.Text != "")
            {
                ButtonAllPersonCustomer.Content = TextBlockSelectedCustomer.Text;
            }
            else
            {
                ButtonAllPersonCustomer.Content = "Все учащиеся организации";
            }
            
            var eventId = await _repositoryEvents.GetIdByNameAsync(TextBlockSelectedEvent.Text);
            var customerId = await _repositoryCustomers.GetIdByNameAsync(TextBlockSelectedCustomer.Text);
            
            var listPersons = await _repositoryPersons.GetAsync(TextBoxSearch.Text, customerId==0?null:customerId, 
                eventId==0?null:eventId);
            DataGridPersons.ItemsSource = listPersons;
        }

        private void ButtonArchiveClick(object sender, RoutedEventArgs e)
        {
            TextBlockSelectedEvent.Text = "Архив";
            UpdateList();
        }

        private void ButtonAllEventClick(object sender, RoutedEventArgs e)
        {
            TextBlockSelectedEvent.Text = null;
            UpdateList();
        }

        private void ButtonEventClick(object sender, RoutedEventArgs e)
        {
            TextBlockSelectedEvent.Text = ((Button)sender).Content.ToString();
            UpdateList();
        }

        private void ButtonAllPersonCustomer_OnClick(object sender, RoutedEventArgs e)
        {
            if (DataGridPersons.SelectedItem != null)
            {
                TextBlockSelectedCustomer.Text = ((PersonDto)DataGridPersons.SelectedItem).Customer;
                UpdateList();
            }
            
        }

        private void ButtonClearSearch_OnClick(object sender, RoutedEventArgs e)
        {
            TextBoxSearch.Text = "";
            UpdateList();
        }

        private void ButtonClearCustomerSearch_OnClick(object sender, RoutedEventArgs e)
        {
            TextBlockSelectedCustomer.Text = "";
            UpdateList();
        }

        private void TextBoxSearch_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                UpdateList();
            }
        }

        private void DataGridPerons_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGridPersons.SelectedItem != null)
            {
                var dateOfSubmission = ((PersonDto)DataGridPersons.SelectedItem).DateOfSubmission;
                var dateEzam = ((PersonDto)DataGridPersons.SelectedItem).DateEzam;
                
                GroupBoxPersonInfo.Header = $"Информация об учащемся {((PersonDto)DataGridPersons.SelectedItem).FCs}";
                GroupBoxPersonInfo.Visibility = Visibility.Visible;
                TextBlockDateOfSubmission.Text =
                    dateOfSubmission == DateTime.MinValue ? "не указано" : dateOfSubmission.ToString("d");
                TextBlockPost.Text = ((PersonDto)DataGridPersons.SelectedItem).Post;
                TextBlockPhone.Text = ((PersonDto)DataGridPersons.SelectedItem).Phone;
                TextBlockEmail.Text = ((PersonDto)DataGridPersons.SelectedItem).Email;
                TextBlockAccess.Text = ((PersonDto)DataGridPersons.SelectedItem).Access;
                TextBlockCertification.Text = ((PersonDto)DataGridPersons.SelectedItem).Certification;
                TextBlockDateEzam.Text = dateEzam == DateTime.MinValue ? "не указано" : dateEzam.ToString("g");
                TextBlockEvent.Text = ((PersonDto)DataGridPersons.SelectedItem).Event;
                TextBlockGroup.Text = ((PersonDto)DataGridPersons.SelectedItem).GroupNum;
            }
        }

        private void ButtonAddPerson_OnClick(object sender, RoutedEventArgs e)
        {
            var addPersonWindow = new AddPersonWindow(_repositoryCustomers, _repositoryPersons);
            addPersonWindow.Owner = this;
            Hide();
            addPersonWindow.Show();
        }


        private void ButtonEditPerson_OnClick(object sender, RoutedEventArgs e)
        {
            if (DataGridPersons.SelectedItem != null)
            {
                var currentPerson = (PersonDto)DataGridPersons.SelectedItem;
            
                var addPersonWindow = new AddPersonWindow(_repositoryCustomers, _repositoryPersons);
                addPersonWindow.Owner = this;

                addPersonWindow.TextBlockIdPerson.Text = currentPerson.Id.ToString();
                addPersonWindow.TextBoxAccess.Text = currentPerson.Access;
                addPersonWindow.TextBoxApplication.Text = currentPerson.Application;
                addPersonWindow.TextBoxApproval.Text = currentPerson.Approval;
                addPersonWindow.TextBoxAttorney.Text = currentPerson.Attorney;
                addPersonWindow.TextBoxCertification.Text = currentPerson.Certification;
                addPersonWindow.ComboBoxCustomers.Text = currentPerson.Customer;
                addPersonWindow.TextBoxEmail.Text = currentPerson.Email;
                addPersonWindow.TextBoxNote.Text = currentPerson.Note;
                addPersonWindow.TextBoxPhone.Text = currentPerson.Phone;
                addPersonWindow.TextBoxPost.Text = currentPerson.Post;
                addPersonWindow.TextBoxQuestionnaire.Text = currentPerson.Questionnaire;
                addPersonWindow.TextBoxFCs.Text = currentPerson.FCs;
                addPersonWindow.TextBoxIdentityCard.Text = currentPerson.IdentityCard;
                addPersonWindow.TextBoxPaymentCard.Text = currentPerson.PaymentCard;
                addPersonWindow.TextBoxGroup.Text = currentPerson.GroupNum;
                addPersonWindow.DatePickerExam.SelectedDate = currentPerson.DateEzam == DateTime.MinValue
                    ? null : currentPerson.DateEzam;
                addPersonWindow.DatePickerDateSub.SelectedDate = currentPerson.DateOfSubmission== DateTime.MinValue
                    ? null : currentPerson.DateOfSubmission;
                addPersonWindow.TextBoxTimeExam.Text = currentPerson.DateEzam.ToString("t");
                addPersonWindow.Title = "Редактирование учащегося";
                addPersonWindow.ButtonAddPerson.Content = "Обновить";
                Hide();
                addPersonWindow.Show();
            }
            
        }

        private void ButtonShowHistory_OnClick(object sender, RoutedEventArgs e)
        {
            if (DataGridPersons.SelectedItem != null)
            {
                var person = (PersonDto)DataGridPersons.SelectedItem;
                var hisoryPerson = new HistoryPersonEventWindow(person.Id, _repositoryHistory);
                hisoryPerson.Show();
            }
            
        }

        private void ButtonEditEvents_OnClick(object sender, RoutedEventArgs e)
        {
            var eventWindow = new EventWindow(_repositoryEvents);
            eventWindow.Owner = this;
            Hide();
            eventWindow.Show();
        }

        private void ButtonChangeEvent_OnClick(object sender, RoutedEventArgs e)
        {
            if (DataGridPersons.SelectedItem != null)
            {
                var person = (PersonDto)DataGridPersons.SelectedItem;
                var setEventWindow = new SetEventWindow(person, _repositoryEvents,_repositoryPersons, _repositoryHistory);
                setEventWindow.Owner = this;
                setEventWindow.TextBoxDescription.Text = DateTime.Now.ToString("g") + " - ";
                Hide();
                setEventWindow.Show();
            }
            else
            {
                MessageBox.Show("Выберете персону");
            }
        }

        private void ButtonUpdate_OnClick(object sender, RoutedEventArgs e)
        {
            UpdateList();
        }
    }
}