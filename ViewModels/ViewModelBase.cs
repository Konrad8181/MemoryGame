using ReactiveUI;
using System;

namespace MemoryGame.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
        public bool CanNavigatePage = true;

        public IObservable<bool> CanNavigate { get; set; }

        public ViewModelBase()
        {
            CanNavigate = this.WhenAnyValue(x => x.CanNavigatePage);
        }
    }
}