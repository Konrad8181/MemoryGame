using MemoryGame.Models;
using ReactiveUI;
using System.Reactive;

namespace MemoryGame.ViewModels
{
    public class GameEndViewModel : ReactiveObject
    {
        private Score? _score;

        private string _nickname;

        private bool _submitVisibility;

        public ReactiveCommand<Unit, Score?> EndGameCommand { get; }

        public bool SubmitVisibility
        {
            get { return _submitVisibility; }
            private set { this.RaiseAndSetIfChanged(ref _submitVisibility, value); }
        }

        public string Nickname
        { 
            get => _nickname;
            set
            {
                _nickname = value;
                if (_score != null)
                {
                    _score.PlayerName = _nickname;
                    SubmitVisibility = _nickname.Length > 0;
                }
                this.RaisePropertyChanged(nameof(Nickname));
            }
        }

        public GameEndViewModel()
        {
            _score = new Models.Score() ?? null;
            EndGameCommand = ReactiveCommand.Create(() =>
            {
                return _score;
            }); 
        }
    }
}
