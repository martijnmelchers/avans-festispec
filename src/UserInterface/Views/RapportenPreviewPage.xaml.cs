﻿using Festispec.DomainServices.Interfaces;
using Festispec.DomainServices.Services;
using Microsoft.Extensions.DependencyInjection;

using System.Windows.Controls;

using Festispec.UI.ViewModels;

namespace Festispec.UI.Views
{
    /// <summary>
    /// Interaction logic for RapportenPreviewPage.xaml
    /// </summary>
    public partial class RapportenPreviewPage : Page
    {

        private readonly IServiceScope _scope;

        public RapportenPreviewPage()
        {
            InitializeComponent();


            _scope = AppServices.Instance.ServiceProvider.CreateScope();
            Unloaded += (sender, e) => _scope.Dispose();

            DataContext = _scope.ServiceProvider.GetRequiredService<RapportPreviewViewModel>();
        }
    }
}