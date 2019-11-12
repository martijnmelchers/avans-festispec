using GalaSoft.MvvmLight.Views;
using System.Collections.Generic;

namespace Festispec.UI.Interfaces
{
    public interface IFrameNavigationService : INavigationService
    {
        object Parameter { get; }
        IEnumerable<string> Pages { get; }
    }
}
