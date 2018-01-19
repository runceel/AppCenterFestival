using AppCenterFestival.Events;
using AppCenterFestival.ViewModels;
using Microsoft.Practices.ServiceLocation;
using Prism.Events;
using Prism.Windows.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace AppCenterFestival.Views
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private MainPageViewModel ViewModel => (MainPageViewModel)DataContext;

        private IEventAggregator EventAggregator { get; }

        public MainPage()
        {
            this.InitializeComponent();
            EventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            EventAggregator.GetEvent<NotifyEvent>()
                .Unsubscribe(NotifyEventReceived);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            EventAggregator.GetEvent<NotifyEvent>()
                .Subscribe(NotifyEventReceived, ThreadOption.UIThread);
        }

        private void NotifyEventReceived(string message)
        {
            inAppNotification.Show(message, 5000);
        }
    }
}
