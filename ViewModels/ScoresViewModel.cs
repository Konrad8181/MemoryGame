using MemoryGame.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MemoryGame.ViewModels
{
    public class ScoresViewModel : ViewModelBase
    {
        private ObservableCollection<Score> _scoresList;
        public ObservableCollection<Score> ScoresList
        {
            get { return _scoresList; }
            private set { this.RaiseAndSetIfChanged(ref _scoresList, value); }
        }

        public ScoresViewModel()
        {
            var scoresMock = new List<Score>(){
                new Score()
                {
                    Date = DateTime.Now,
                    PlayerName="Player1",
                    TimeScore = new TimeSpan(0,2,0)
                },
                new Score()
                {
                    Date = DateTime.Now,
                    PlayerName="Player2",
                    TimeScore = new TimeSpan(4,2,0)
                },
                new Score()
                {
                    Date = DateTime.Now,
                    PlayerName="Player3",
                    TimeScore = new TimeSpan(0,6,9)
                }
            };
            ScoresList = new ObservableCollection<Score>(scoresMock);
        }
    }
}
