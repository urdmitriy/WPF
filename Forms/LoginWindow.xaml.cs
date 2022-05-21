using System;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Input;
using ApiDog.Dto;
using Microsoft.Extensions.Http;

namespace DesktopApp.Forms
{
    public partial class LoginWindow : Window
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public LoginWindow(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            InitializeComponent();
            LoadLogin();
        }
        private void ButtonEnter_OnClick(object sender, RoutedEventArgs e)
        {
            ShowMainWindow();
        }

        private void LoadLogin()
        {
            const string fileSettings = "settings.txt";
            if (!File.Exists(fileSettings)) return;
            var settings = File.ReadAllLines(fileSettings);
            TextBoxLogin.Text = settings[0];
        }
        
        private void ShowMainWindow()
        {
            var login = TextBoxLogin.Text;

            const string fileSettings = "settings.txt";
            File.WriteAllText(fileSettings, login);

            var token="";
            try
            {
                token = Services.GetTokenFromServer(new AccountDto() { Login = login, PasswordHash = PasswordBox.Password });
            }
            catch (Exception e)
            {
                MessageBox.Show("Не удалось подключиться к серверу");
                Close();
            }
            
            if (token == "None")
            {
                MessageBox.Show("Неверные данные для входа");
                Close();
            }
            else if (string.IsNullOrEmpty(token))
            {
                MessageBox.Show("Токен не получен от сервера");
                Close();
            }
            else
            {
                Services.Token = token;
                MainWindow mainWindow = new MainWindow(_httpClientFactory);
                mainWindow.Owner = this;
                Hide();
                mainWindow.Show();
            }
            
        }
        private void ButtonExit_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
        

        private void PasswordBox_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ShowMainWindow();
            }
        }
    }
}