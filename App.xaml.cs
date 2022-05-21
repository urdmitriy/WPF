using System;
using System.Windows;
using ApiDog.Services;
using ApiDog.Services.Interfaces;
using DesktopApp.Forms;
using Microsoft.Extensions.DependencyInjection;
using Polly;


namespace DesktopApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public ServiceProvider ServiceProvider { get; private set; }
        
        protected override void OnStartup(StartupEventArgs e)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();
            var mainView = ServiceProvider.GetRequiredService<LoginWindow>();
            mainView.Show();
            base.OnStartup(e);
        }
        private void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient("Vertical")
                .AddTransientHttpErrorPolicy(p=>
                    p.WaitAndRetryAsync(3,_=>TimeSpan.FromMilliseconds(1000)));

            services.AddTransient(typeof(LoginWindow));

        }
   
    }
}