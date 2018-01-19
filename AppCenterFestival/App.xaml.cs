using AppCenterFestival.Models;
using AppCenterFestival.Views;
using Prism.Unity.Windows;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Practices.Unity;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Distribute;
using AppCenterFestival.Logger;
using Microsoft.AppCenter.Push;
using AppCenterFestival.Events;
using Reactive.Bindings;
using System.Reactive.Concurrency;

namespace AppCenterFestival
{
    /// <summary>
    /// 既定の Application クラスを補完するアプリケーション固有の動作を提供します。
    /// </summary>
    sealed partial class App : PrismUnityApplication
    {
        /// <summary>
        /// 単一アプリケーション オブジェクトを初期化します。これは、実行される作成したコードの
        ///最初の行であるため、main() または WinMain() と論理的に等価です。
        /// </summary>
        public App()
        {
            this.InitializeComponent();

            AppCenter.Start(Keys.AppCenterKey,
                typeof(Analytics),
                typeof(Crashes),
                typeof(Distribute),
                typeof(Push));

            Push.PushNotificationReceived += this.Push_PushNotificationReceived;
        }

        private void Push_PushNotificationReceived(object sender, PushNotificationReceivedEventArgs e)
        {
            if (e.CustomData.ContainsKey("theme"))
            {
                EventAggregator.GetEvent<NotifyEvent>()
                    .Publish($"{e.CustomData["theme"]} について書いてね！");
                Container.Resolve<DocumentManager>().AddDocument(e.CustomData["theme"]);
            }
        }

        protected override async Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {
            await Container.Resolve<DocumentManager>().LoadDataAsync();

            Push.CheckLaunchedFromNotification(args);
            NavigationService.Navigate("Main", null);
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            Container.RegisterType<DocumentManager>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IAppCenterLogger, AppCenterLogger>(new ContainerControlledLifetimeManager());
        }

        protected override UIElement CreateShell(Frame rootFrame)
        {
            var shell = Container.Resolve<Shell>();
            shell.FrameHost.Content = rootFrame;
            return shell;
        }
    }
}
