using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Windows;
using ApiDog.Dto;
using ApiDog.Services.Interfaces;
using DesktopApp.Repository;

namespace DesktopApp.Forms
{
    public partial class SetEventWindow : Window
    {
        private PersonDto _person;
        private readonly IRepositoryEvent _repositoryEvents;
        private readonly IRepositoryPerson _repositoryPersons;
        private readonly IRepositoryHistory _repositoryHistorys;
        public SetEventWindow(PersonDto person, IRepositoryEvent repositoryEvents, IRepositoryPerson repositoryPersons, IRepositoryHistory repositoryHistorys)
        {
            _person = person;
            _repositoryEvents = repositoryEvents;
            _repositoryPersons = repositoryPersons;
            _repositoryHistorys = repositoryHistorys;

            InitializeComponent();
            TextBlockPersonName.Text = $"Смена события '{_person.Event}' для {person.FCs} на:";
            UpdateEventList();
        }

        private async void UpdateEventList()
        {
            var eventList = await _repositoryEvents.GetAsync("");
            if (eventList != null) ListBoxEvents.ItemsSource = eventList.Select(x => x.Name);
        }

        private void SetEventWindow_OnClosing(object sender, CancelEventArgs e)
        {
            Owner.Show();
        }

        private async void ButtonChangeEvent_OnClick(object sender, RoutedEventArgs e)
        {
            if (ListBoxEvents.SelectedItem != null)
            {
                int eventId = await _repositoryEvents.GetIdByNameAsync(ListBoxEvents.SelectedItem.ToString());
                var result = await _repositoryPersons.EventSetAsync(_person.Id, eventId);
                if (result != 0)
                {
                    var recordHistory = new HistoryPersonEventDto()
                    {
                        PersonId = _person.Id,
                        EventId = eventId,
                        DescriptionChangeEvent = TextBoxDescription.Text
                    };
                    var resultAddHistory = await _repositoryHistorys.AddAsync(recordHistory);
                    if (resultAddHistory == 0)
                    {
                        MessageBox.Show("Не удалось создать запись истории");
                    }
                }
                else
                {
                    MessageBox.Show("Не удалось сменить событие");
                }
                Close();
            }
        }

        private void ButtonExit_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}