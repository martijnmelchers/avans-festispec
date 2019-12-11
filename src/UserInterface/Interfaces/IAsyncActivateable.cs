using System.Threading.Tasks;

namespace Festispec.UI.ViewModels
{
    public interface IAsyncActivateable<TInput>
    {
        public Task Initialize(TInput input);
    }
}