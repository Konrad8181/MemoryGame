using ReactiveUI;
using System;
using System.Windows.Input;

namespace MemoryGame.ViewModels
{
    public class MenuViewModel : ViewModelBase
    {
        public ICommand NavigatePlayCommand { get; }
        
        public ICommand NavigateScoresCommand { get; }
        
        public ICommand NavigateAboutCommand { get; }

        public delegate void NavigationRequested(Type type);

        public event NavigationRequested OnNavigationRequested;

        public MenuViewModel()
        {
            NavigatePlayCommand = ReactiveCommand.Create(NavigatePlay, CanNavigate);
            NavigateScoresCommand = ReactiveCommand.Create(NavigateScores, CanNavigate);
            NavigateAboutCommand = ReactiveCommand.Create(NavigateAbout, CanNavigate);
        }

        private void NavigatePlay()
        {
            if (OnNavigationRequested == null)
            {
                return;
            }
            OnNavigationRequested.Invoke(typeof(PlayViewModel));
        }

        private void NavigateScores()
        {
            OnNavigationRequested.Invoke(typeof(ScoresViewModel));
        }

        private void NavigateAbout()
        {
            OnNavigationRequested.Invoke(typeof(AboutViewModel));
        }
    }
}
