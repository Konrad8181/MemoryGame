using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace MemoryGame.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly Type DefaultNavigationPageType = typeof(MenuViewModel);

        private readonly List<ReactiveObject> Pages = new ();

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

        private void NavigateMenuPage()
        {
            NavigatePage(DefaultNavigationPageType);
        }

        private void InitializePages()
        {
            var menuViewModel = new MenuViewModel();
            var scoresViewModel = new ScoresViewModel();   
            var playViewModel = new PlayViewModel();   
            var aboutViewModel = new AboutViewModel();
            menuViewModel.OnNavigationRequested += NavigatePage;
            Pages.Add(aboutViewModel);
            Pages.Add(scoresViewModel);
            Pages.Add(playViewModel);
            Pages.Add(menuViewModel);
        }

        public void NavigatePage(Type type)
        {
            var selectedPageIndex = Pages.FindIndex(x => x.GetType() == type);
            if (selectedPageIndex != -1)
            {
                CurrentPage = Pages[selectedPageIndex];
            }
            if (type == DefaultNavigationPageType)
            {
                NavigationPreviousVisibility = false;
            }
            else
            {
                NavigationPreviousVisibility = true;
            }
        }
    }
}