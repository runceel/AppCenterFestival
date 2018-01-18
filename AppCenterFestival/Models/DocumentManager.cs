using Microsoft.Toolkit.Uwp.Helpers;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCenterFestival.Models
{
    public class DocumentManager
    {
        private ObservableCollection<Document> DocumentsSource { get; }
        public ReadOnlyObservableCollection<Document> Documents { get; }

        public ReactivePropertySlim<DocumentEditMode> DocumentEditMode { get; }

        private LocalObjectStorageHelper LocalObjectStorageHelper { get; } = new LocalObjectStorageHelper();

        private SerialDisposable DataChangedDisposable { get; } = new SerialDisposable();


        public DocumentManager()
        {
            DocumentsSource = new ObservableCollection<Document>();
            Documents = new ReadOnlyObservableCollection<Document>(DocumentsSource);
            DocumentEditMode = new ReactivePropertySlim<Models.DocumentEditMode>(Models.DocumentEditMode.Normal);

            StartObserveDataChanged();
        }

        private void StartObserveDataChanged()
        {
            DataChangedDisposable.Disposable = Observable.Merge(DocumentsSource.CollectionChangedAsObservable().ToUnit(),
                DocumentsSource.ObserveElementObservableProperty(x => x.Title).ToUnit(),
                DocumentsSource.ObserveElementObservableProperty(x => x.Content).ToUnit())
                .Throttle(TimeSpan.FromSeconds(5))
                .Subscribe(async _ => await LocalObjectStorageHelper.SaveFileAsync("docs.json", DocumentsSource.ToArray()));
        }

        public string AddDocument(string title)
        {
            var doc = new Document();
            doc.Title.Value = title;
            DocumentsSource.Add(doc);
            return doc.Id.Value;
        }

        public void SwitchEditMode() => DocumentEditMode.Value = DocumentEditMode.Value == Models.DocumentEditMode.Normal ?
                Models.DocumentEditMode.FullScreen :
                Models.DocumentEditMode.Normal;

        public void RemoveDocument(string id)
        {
            DocumentsSource.Remove(DocumentsSource.First(x => x.Id.Value == id));
        }

        public async Task LoadDataAsync()
        {
            if (!await LocalObjectStorageHelper.FileExistsAsync("docs.json"))
            {
                return;
            }

            DataChangedDisposable.Disposable = Disposable.Empty;
            var docs = await LocalObjectStorageHelper.ReadFileAsync("docs.json", Enumerable.Empty<Document>());
            foreach (var doc in docs)
            {
                DocumentsSource.Add(doc);
            }
            StartObserveDataChanged();
        }
    }
}
