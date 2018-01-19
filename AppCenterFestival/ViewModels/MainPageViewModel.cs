﻿using AppCenterFestival.Logger;
using AppCenterFestival.Models;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Prism.Logging;
using Prism.Windows.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Reactive.Bindings.Notifiers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace AppCenterFestival.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public DocumentManager DocumentManager { get; }
        private IAppCenterLogger AppCenterLogger { get; }

        public ReadOnlyObservableCollection<Document> Documents => DocumentManager.Documents;

        public ReactivePropertySlim<string> NewDocumentTitle { get; }

        public ReactiveCommand AddDocumentCommand { get; }

        public ReactivePropertySlim<Document> SelectedDocument { get; }

        public ReadOnlyReactivePropertySlim<bool> IsEditorBladeOpen { get; }

        public ReadOnlyReactivePropertySlim<string> PreviewMarkdownText { get; }

        public ReadOnlyReactivePropertySlim<BladeMode> BladeMode { get; }

        private BooleanNotifier IsPreviewOpenNotifier { get; } = new BooleanNotifier();

        public ReadOnlyReactivePropertySlim<bool> IsPreviewOpen { get; }

        public ReactiveCommand SwitchPreviewCommand { get; }

        public ReactiveCommand<Document> RemoveDocumentCommand { get; }

        public MainPageViewModel(DocumentManager documentManager, IAppCenterLogger appCenterLogger)
        {
            this.DocumentManager = documentManager;
            this.AppCenterLogger = appCenterLogger;
            NewDocumentTitle = new ReactivePropertySlim<string>();
            AddDocumentCommand = NewDocumentTitle
                .Select(x => !string.IsNullOrEmpty(x))
                .ToReactiveCommand()
                .WithSubscribe(() =>
                {
                    if (NewDocumentTitle.Value == "野菜")
                    {
                        throw new InvalidOperationException("野菜 is invalid");
                    }

                    DocumentManager.AddDocument(NewDocumentTitle.Value);
                    AppCenterLogger.TrackEvent("Document created", ("name", NewDocumentTitle.Value));
                    NewDocumentTitle.Value = "";
                });

            SelectedDocument = new ReactivePropertySlim<Document>();
            IsEditorBladeOpen = SelectedDocument
                .Select(x => x != null)
                .ToReadOnlyReactivePropertySlim();

            BladeMode = DocumentManager.DocumentEditMode
                .Select(x => x == DocumentEditMode.Normal ? Microsoft.Toolkit.Uwp.UI.Controls.BladeMode.Normal : Microsoft.Toolkit.Uwp.UI.Controls.BladeMode.Fullscreen)
                .ToReadOnlyReactivePropertySlim();

            PreviewMarkdownText = SelectedDocument
                .SelectMany(x => x?.Content ?? Observable.Return(""))
                .Throttle(TimeSpan.FromSeconds(1))
                .ObserveOnUIDispatcher()
                .ToReadOnlyReactivePropertySlim();

            IsPreviewOpen = IsPreviewOpenNotifier.ToReadOnlyReactivePropertySlim();

            SwitchPreviewCommand = new ReactiveCommand()
                .WithSubscribe(() => IsPreviewOpenNotifier.SwitchValue());

            RemoveDocumentCommand = new ReactiveCommand<Document>()
                .WithSubscribe(x =>
                {
                    DocumentManager.RemoveDocument(x.Id.Value);
                    AppCenterLogger.TrackEvent("Document removed", ("name", x.Title.Value), ("content", x.Content.Value));
                    IsPreviewOpenNotifier.TurnOff();
                });

        }

    }
}
