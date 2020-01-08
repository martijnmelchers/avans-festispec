﻿using System.Windows.Controls;
using Festispec.UI.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Festispec.UI.Views.Login
{
    /// <summary>
    ///     Interaction logic for LoginPageEmployee.xaml
    /// </summary>
    public partial class LoginPageEmployee : Page
    {
        public LoginPageEmployee()
        {
            InitializeComponent();

            IServiceScope scope = AppServices.Instance.ServiceProvider.CreateScope();
            Unloaded += (sender, e) => scope.Dispose();

            DataContext = scope.ServiceProvider.GetRequiredService<MainViewModel>();
        }
    }
}