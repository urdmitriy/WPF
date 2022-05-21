using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using ApiDog.Services.Interfaces;
using DesktopApp.Repository;

namespace DesktopApp.Forms
{
    public partial class HistoryPersonEventWindow : Window
    {
        private readonly IRepositoryHistory _repositoryHistorys;
        private readonly int _personId;
        public HistoryPersonEventWindow(int personId, IRepositoryHistory repositoryHistorys)
        {
            _personId = personId;
            _repositoryHistorys = repositoryHistorys;
            InitializeComponent();
            
        }

        
        private async Task UpdateHistoryAsync(int personId)
        {
            var eventList = await _repositoryHistorys.GetAsync(personId);
            DataGridEvents.ItemsSource = eventList;
        }
        private void ButtonClose_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void HistoryPersonEventWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            await UpdateHistoryAsync(_personId);
        }
    }
}