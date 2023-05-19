using ReactiveUI;
using System.Collections.ObjectModel;
using MemoryGame.Models;
using System.Collections.Generic;
using System;

namespace MemoryGame.ViewModels
{
    public class ScoresViewModel : ViewModelBase
    {
        private ObservableCollection<Score> _scoresList;
       
        public ObservableCollection<Score> ScoresList
        {
            get => _scoresList;
            set => this.RaiseAndSetIfChanged(ref _scoresList, value);
        }

        public ScoresViewModel()
        {
            var scores = new List<Score>()
            {
                new Score()
                {
                    PlayerName="Player1",
                    Date = DateTime.Now,
                    TimeScore = new TimeSpan(0,0,1)
                },
                new Score()
                {
                    PlayerName="Player2",
                    Date = DateTime.Now,
                    TimeScore = new TimeSpan(4,2,0)
                },
                new Score()
                {
                    PlayerName="Player2",
                    Date = DateTime.Now,
                    TimeScore = new TimeSpan(0,9,1)
                }
            };
            _scoresList = new ObservableCollection<Score>(scores);
        }
    }
}
