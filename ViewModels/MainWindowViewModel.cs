using Avalonia.Controls.ApplicationLifetimes;
using MemoryGame.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Windows.Input;

namespace MemoryGame.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly Type DefaultNavigationPageType = typeof(MenuViewModel);

        private readonly Type ScoresNavigationPageType = typeof(ScoresViewModel);

        private readonly List<ReactiveObject> Pages = new();

        private ReactiveObject _currentPage;

        public ReactiveObject CurrentPage
        {
            get { return _currentPage; }
            private set { this.RaiseAndSetIfChanged(ref _currentPage, value); }
        }

        private bool _navigationPreviousVisibility;

        public bool NavigationPreviousVisibility
        {
            get { return _navigationPreviousVisibility; }
            private set { this.RaiseAndSetIfChanged(ref _navigationPreviousVisibility, value); }
        }

        public ICommand NavigatePreviousCommand { get; }

        public MainWindowViewModel()
        {
            InitializePages();
            NavigatePreviousCommand = ReactiveCommand.Create(NavigateMenuPage, CanNavigate);
            _currentPage = Pages[Pages.FindIndex(x => x.GetType() == DefaultNavigationPageType)];
        }

        public void HandleClosing()
        {

        }

        private void NavigateMenuPage()
        {
            NavigatePage(DefaultNavigationPageType);
        }

        private void NavigateScoresPage(Score score)
        {
            var scorePage = Pages.Where(x => x.GetType() == ScoresNavigationPageType).FirstOrDefault();
            if (scorePage != null) 
            {
                (scorePage as ScoresViewModel).UpdateScoresList(score);
            }
            NavigatePage(ScoresNavigationPageType);
        }

        private void InitializePages()
        {
            var menuViewModel = new MenuViewModel();
            var aboutViewModel = new AboutViewModel();
            var scoresViewModel = new ScoresViewModel();
            var playViewModel = new PlayViewModel();
            playViewModel.OnNaviagteScores += NavigateScoresPage;
            menuViewModel.OnNavigationRequested += NavigatePage;

            ((IClassicDesktopStyleApplicationLifetime)Avalonia.Application.Current.ApplicationLifetime).ShutdownRequested += delegate (object? sender, ShutdownRequestedEventArgs e)
            {
                scoresViewModel.Serialize();
            };
            Pages.Add(aboutViewModel);
            Pages.Add(menuViewModel);
            Pages.Add(scoresViewModel);
            Pages.Add(playViewModel);
        }

        public void NavigatePage(Type type)
        {
            var selectedPageIndex = Pages.FindIndex(x => x.GetType() == type);
            if (selectedPageIndex != -1)
            {
                var newPage = Pages[selectedPageIndex];
                if (newPage.GetType() == typeof(PlayViewModel))
                {
                    (newPage as PlayViewModel).ReshuffleCards();
                }
                CurrentPage = newPage;
            }
            NavigationPreviousVisibility = type != DefaultNavigationPageType;
        }
    }
}
