using System.Collections.Generic;
using GalaSoft.MvvmLight.Views;

namespace Festispec.UI.Interfaces
{
    public interface IFrameNavigationService : INavigationService
    {
        object Parameter { get; }
        IEnumerable<string> Pages { get; }
    }
}