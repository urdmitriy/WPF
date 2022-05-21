using System;
using System.ComponentModel;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using ApiDog.Dto;
using ApiDog.Services.Interfaces;
using DesktopApp.Repository;
using Microsoft.VisualBasic;

namespace DesktopApp.Forms
{
    public partial class EventWindow : Window
    {
        private readonly IRepositoryEvent _repositoryEvents;
        public EventWindow(IRepositoryEvent repositoryEvents)
        {
            _repositoryEvents = repositoryEvents;
            InitializeComponent();
            UpdateList();
        }

        private async void UpdateList()
        {
            var eventList = await _repositoryEvents.GetAsync("all");
            DataGridEvents.ItemsSource = eventList;
        }

        private async void ButtonHideEvent_OnClick(object sender, RoutedEventArgs e)
        {
            if (DataGridEvents.SelectedItem != null)
            {
                var event_ = (EventDto)DataGridEvents.SelectedItem;
                var result = await _repositoryEvents.HideEventAsync(event_.Id);
                UpdateList();
            }
            
        }

        private async void ButtonShowEvent_OnClick(object sender, RoutedEventArgs e)
        {
            if (DataGridEvents.SelectedItem != null)
            {
                var event_ = (EventDto)DataGridEvents.SelectedItem;
                var result = await _repositoryEvents.ShowEventAsync(event_.Id);
                UpdateList();
            }
        }

        private void EventWindow_OnClosing(object sender, CancelEventArgs e)
        {
            Owner.Show();
        }

        private void ButtonExit_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DataGridEvents_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGridEvents.SelectedItem != null)
            {
                var event_ = (EventDto)DataGridEvents.SelectedItem;
                TextBoxNameEvent.Text = event_.Name;
                TextBoxDescription.Text = event_.Description;
                TextBlockEventId.Text = event_.Id.ToString();
                TextBoxNameEvent.Visibility = Visibility.Visible;
                TextBoxDescription.Visibility = Visibility.Visible;
            }
        }

        private async void ButtonEditEvent_OnClick(object sender, RoutedEventArgs e)
        {
            var updateEvent = new EventDto()
            {
                Name = TextBoxNameEvent.Text,
                Description = TextBoxDescription.Text
            };
            updateEvent.Id = Convert.ToInt32(TextBlockEventId.Text);
            var result = await _repositoryEvents.UpdateAsync(updateEvent);
            if (result == 0) MessageBox.Show("Не удалось обновить");
            HideButtonUpdate();
            UpdateList();
        }

        private void ShowButtonUpdate()
        {
            ButtonUpdateEvent.Visibility = Visibility.Visible;
        }
        private void HideButtonUpdate()
        {
            ButtonUpdateEvent.Visibility = Visibility.Collapsed;
        }

        private void TextBoxNameEvent_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ShowButtonUpdate();
        }

        private void TextBoxDescription_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ShowButtonUpdate();
        }

        private async void ButtonAddEvent_OnClick(object sender, RoutedEventArgs e)
        {
            var newEventName = Interaction.InputBox("Название события", "Новое событие");
            if (!string.IsNullOrWhiteSpace(newEventName))
            {
                var newEvent = new EventDto() { Name = newEventName };
                var result = await _repositoryEvents.AddAsync(newEvent);
                if (result == 0) MessageBox.Show("Не удалось создать событие");
            }
            UpdateList();
        }
    }
}