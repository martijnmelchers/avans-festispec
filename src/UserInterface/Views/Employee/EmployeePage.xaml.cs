﻿using System.Windows.Controls;
using Festispec.UI.ViewModels.Employees;
using Microsoft.Extensions.DependencyInjection;

namespace Festispec.UI.Views.Employee
{
    /// <summary>
    /// Interaction logic for EmployeeInformationScreen.xaml
    /// </summary>
    public partial class EmployeePage : Page
    {
        public EmployeePage()
        {
            InitializeComponent();

            IServiceScope scope = AppServices.Instance.ServiceProvider.CreateScope();
            Unloaded += (sender, e) => scope.Dispose();

            DataContext = scope.ServiceProvider.GetRequiredService<EmployeeViewModel>();
        }


    }
}