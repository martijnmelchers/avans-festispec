using Festispec.UI.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Festispec.UI.Services
{
    /** Courtesy of https://stackoverflow.com/questions/28966819/mvvm-light-5-0-how-to-use-the-navigation-service */
    public class FrameNavigationService : IFrameNavigationService, INotifyPropertyChanged
    {
        #region Fields
        private readonly Dictionary<string, Uri> _pagesByKey;
        private readonly List<string> _historic;
        private string _currentPageKey;
        #endregion

        #region Properties                                              
        public string CurrentPageKey
        {
            get
            {
                return _currentPageKey;
            }

            private set
            {
                if (_currentPageKey == value)
                    return;

                _currentPageKey = value;
                OnPropertyChanged(nameof(CurrentPageKey));
            }
        }
        public object Parameter { get; private set; }

        public IEnumerable<string> Pages { get => _pagesByKey.Keys; }

        #endregion

        #region Ctors and Methods
        public FrameNavigationService()
        {
            _pagesByKey = new Dictionary<string, Uri>();
            _historic = new List<string>();
        }
        public void GoBack()
        {
            if (_historic.Count > 1)
            {
                _historic.RemoveAt(_historic.Count - 1);
                NavigateTo(_historic.Last(), null);
            }
        }
        public void NavigateTo(string pageKey)
        {
            NavigateTo(pageKey, null);
            MessageBox.Show(CurrentPageKey);
        }

        public virtual void NavigateTo(string pageKey, object parameter)
        {
            lock (_pagesByKey)
            {
                if (!_pagesByKey.ContainsKey(pageKey))
                    throw new ArgumentException(string.Format("No such page: {0} ", pageKey), nameof(pageKey));


                if (GetDescendantFromName(Application.Current.MainWindow, "MainFrame") is Frame frame)
                    frame.Source = _pagesByKey[pageKey];

                Parameter = parameter;
                _historic.Add(pageKey);
                CurrentPageKey = pageKey;
            }
        }

        public void Configure(string key, Uri pageType)
        {
            lock (_pagesByKey)
            {
                if (_pagesByKey.ContainsKey(key))
                    _pagesByKey[key] = pageType;
                else
                    _pagesByKey.Add(key, pageType);
            }
        }

        private static FrameworkElement GetDescendantFromName(DependencyObject parent, string name)
        {
            var count = VisualTreeHelper.GetChildrenCount(parent);

            if (count < 1)
                return null;

            for (var i = 0; i < count; i++)
            {
                if (VisualTreeHelper.GetChild(parent, i) is FrameworkElement frameworkElement)
                {
                    if (frameworkElement.Name == name)
                        return frameworkElement;

                    frameworkElement = GetDescendantFromName(frameworkElement, name);

                    if (frameworkElement != null)
                        return frameworkElement;
                }
            }
            return null;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
