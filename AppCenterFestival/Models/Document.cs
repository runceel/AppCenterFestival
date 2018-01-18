using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCenterFestival.Models
{
    public class Document
    {
        public ReactivePropertySlim<string> Id { get; } = new ReactivePropertySlim<string>(Guid.NewGuid().ToString());
        public ReactivePropertySlim<string> Title { get; } = new ReactivePropertySlim<string>();
        public ReactivePropertySlim<string> Content { get; } = new ReactivePropertySlim<string>();
    }
}
