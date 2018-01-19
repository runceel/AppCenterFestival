using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Concurrency;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AppCenterFestival.Utils
{
    public static class ObservableCollectionExtensions
    {
        public static ReadOnlyReactiveCollection<T> ToReadOnlyReactiveCollectionForCurrentContext<T>(this ObservableCollection<T> self, bool disposeElement = true) where T : class =>
            self.ToReadOnlyReactiveCollection(scheduler: new SynchronizationContextScheduler(SynchronizationContext.Current), disposeElement: disposeElement);

        public static ReadOnlyReactiveCollection<U> ToReadOnlyReactiveCollectionForCurrentContext<T, U>(this ObservableCollection<T> self, Func<T, U> converter, bool disposeElement = true) 
            where T : class 
            where U : class =>
            self.ToReadOnlyReactiveCollection(converter, scheduler: new SynchronizationContextScheduler(SynchronizationContext.Current), disposeElement: disposeElement);
        public static ReadOnlyReactiveCollection<T> ToReadOnlyReactiveCollectionForCurrentContext<T>(this ReadOnlyObservableCollection<T> self, bool disposeElement = true) where T : class =>
            self.ToReadOnlyReactiveCollection(scheduler: new SynchronizationContextScheduler(SynchronizationContext.Current), disposeElement: disposeElement);

        public static ReadOnlyReactiveCollection<U> ToReadOnlyReactiveCollectionForCurrentContext<T, U>(this ReadOnlyObservableCollection<T> self, Func<T, U> converter, bool disposeElement = true) 
            where T : class 
            where U : class =>
            self.ToReadOnlyReactiveCollection(converter, scheduler: new SynchronizationContextScheduler(SynchronizationContext.Current), disposeElement: disposeElement);
    }
}
