using Avalonia.ReactiveUI;
using MemoryGame.ViewModels;
using ReactiveUI;
using System;

namespace MemoryGame.Views
{
    public partial class GameEndWindowView : ReactiveWindow<GameEndViewModel>
    {
        public GameEndWindowView()
        {
            InitializeComponent();
            this.WhenActivated(d => d(ViewModel!.EndGameCommand.Subscribe(Close)));
        }
    }
}
