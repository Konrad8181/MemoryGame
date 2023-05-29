using ReactiveUI;
using System.Collections.ObjectModel;
using MemoryGame.Models;
using System.Collections.Generic;
using System;
using System.Linq;

namespace MemoryGame.ViewModels
{
    public class ScoresViewModel : ViewModelBase
    {
        private List<Score> _scores;

        private ObservableCollection<Score> _scoresList;
       
        public ObservableCollection<Score> ScoresList
        {
            get => _scoresList;
            set => this.RaiseAndSetIfChanged(ref _scoresList, value);
        }

        public ScoresViewModel()
        {
            _scores = new List<Score>();
            ReadFromFile();
        }

        public void Serialize()
        {
            var gameScoreSaver = new GameScoresSaver();
            try
            {
                if (_scores.Any())
                {
                    gameScoreSaver.Serialize(_scores);
                }
            }
            catch (Exception)
            {
            }
        }

        public void UpdateScoresList(Score score)
        {
            _scores.Add(score);
            _scoresList = new ObservableCollection<Score>(_scores);
        }

        private void ReadFromFile()
        {
            var gameScoreSaver = new GameScoresSaver();
            List<Score>? tryScores;
            try
            {
                tryScores = gameScoreSaver.Desrialize();
            }
            catch (Exception)
            {
                return;
            }
            ScoresList = new ObservableCollection<Score>(tryScores);
        }
    }
}
