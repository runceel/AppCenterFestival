using AppCenterFestival.Models;
using Prism.Windows.Mvvm;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCenterFestival.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        public DocumentManager DocumentManager { get; }
        public ReactiveCommand SwitchDocumentEditModeCommand { get; }
        public ReadOnlyReactivePropertySlim<bool> IsFullScreen { get; }
        public ShellViewModel(DocumentManager documentManager)
        {
            DocumentManager = documentManager;
            SwitchDocumentEditModeCommand = new ReactiveCommand()
                .WithSubscribe(() => DocumentManager.SwitchEditMode());
            IsFullScreen = DocumentManager.DocumentEditMode
                .Select(x => x == DocumentEditMode.FullScreen)
                .ToReadOnlyReactivePropertySlim();
        }
    }
}
