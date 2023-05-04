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
            Navigate(typeof(PlayViewModel));
        }

        private void NavigateScores()
        {
            Navigate(typeof(ScoresViewModel));
        }

        private void NavigateAbout()
        {
            Navigate(typeof(AboutViewModel));
        }

        private void Navigate(Type type)
        {
            if (OnNavigationRequested == null)
            {
                return;
            }
            OnNavigationRequested.Invoke(type);
        }
    }
}
