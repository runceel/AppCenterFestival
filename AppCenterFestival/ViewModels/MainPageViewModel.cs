using Prism.Windows.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCenterFestival.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public ReactivePropertySlim<string> Input { get; }

        public ReadOnlyReactivePropertySlim<string> Output { get; }

        public MainPageViewModel()
        {
            Input = new ReactivePropertySlim<string>("");
            Output = Input
                .Delay(TimeSpan.FromSeconds(1))
                .Select(x => x.ToUpper())
                .ObserveOnUIDispatcher()
                .ToReadOnlyReactivePropertySlim();
        }
    }
}
