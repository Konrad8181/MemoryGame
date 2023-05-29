using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using MemoryGame.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;

namespace MemoryGame.ViewModels
{
    public class PlayViewModel : ViewModelBase
    {
        private bool _reverseNext = false;

        private DateTime _playGameStartTime;

        private Score? _score;

        private List<int> _availableTags;

        public List<IBitmap> Images;

        public int Rows { get; set; } = 4;

        public int Columns { get; set; } = 4;

        public List<Card> Cards { get; private set; }

        public int CompletedPairs = 0;

        public ICommand EndGameCommand { get; }

        public Interaction<GameEndViewModel, Score?> ShowDialog { get; }

        public delegate void NavigateScores(Score score);

        public event NavigateScores OnNaviagteScores;

        public string CompletedPairsString 
        {
            get => $"Completed pairs: {CompletedPairs} / {(Rows * Columns) / 2}";
        }

        public PlayViewModel()
        {
            SanityCheck();
            LoadAssets();
            ShowDialog = new Interaction<GameEndViewModel, Score?>();

            EndGameCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var store = new GameEndViewModel();
                _score = await ShowDialog.Handle(store);
                _score.TimeScore = DateTime.Now.Subtract(_playGameStartTime);
                _score.Date = DateTime.Now;
                OnNaviagteScores?.Invoke(_score);
            });
        }

        public void ReshuffleCards()
        {
            Cards = new List<Card>();
            CompletedPairs = 0;
            this.RaisePropertyChanged(nameof(CompletedPairsString));
            GenerateRandomTags();
            CreateCards();
            AssignImage();
        }

        public void SetColumns(int columns)
        {
            Columns = columns;
        }

        public void SetRows(int rows)
        {
            Rows = rows;
        }

        private void SanityCheck()
        {
            if ((Columns * Rows) % 2 != 0)
            {
                Trace.WriteLine("Columns * Rows equation should be divisible by zero.");
                if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime)
                {
                    lifetime.Shutdown();
                }
            }
        }

        private void ClearPairs()
        {
            foreach (var card in Cards)
            {
                card.HasReveredPair = false;
                card.IsReversed = false;
            }
        }

        private void LoadAssets()
        {
            Images = new List<IBitmap>();
            foreach (var resource in App.Current.Resources)
            {
                if (resource.Value is ImageBrush brush)
                {
                    Images.Add(brush.Source);
                }
            }
        }

        private void CreateCards()
        {
            var index = 0;
            for (var row = 0; row < Rows; row++)
            {
                for (var column = 0; column < Columns; column++)
                {
                    var card = new Card(row, column, HandleClick, _availableTags[index], string.Empty);
                    Cards.Add(card);
                    index++;
                }
            }
        }

        public void HandleClick(Card card)
        {
            Trace.WriteLine($"Card at position X={card.Position.x}, Y={card.Position.y} has been clicked.");
            if (card.HasReveredPair)
            {
                return;
            }
            CheckIfRollback();
            card.IsReversed = true;
            UpdateState();
            CheckStopCondition();
        }

        private void CheckIfRollback()
        {
            if (_reverseNext)
            {
                var reversedCards = Cards.Where(c => c.IsReversed).ToList();
                var pairs = reversedCards.Where(r => r.HasReveredPair).ToList();
                var noPairsReversed = reversedCards.Except(pairs).ToList();
                foreach (var pair in noPairsReversed)
                {
                    pair.IsReversed = false;
                }
                _reverseNext = false;
            }
        }

        private void CheckStopCondition()
        {
            if (Cards.Where(r => r.HasReveredPair).Count() == (Columns * Rows))
            {
                EndGameCommand.Execute(null);
                Trace.WriteLine("Game over!");
            }
        }

        private void UpdateState()
        {
            var reversedCards = Cards.Where(c => c.IsReversed).ToList();
            var pairs = reversedCards.Where(r  => r.HasReveredPair).ToList();   
            var noPairsReversed = reversedCards.Except(pairs).ToList();
            if (noPairsReversed.Count % 2 == 0 && noPairsReversed.Count == 2)
            {
                if (noPairsReversed[0].Tag == noPairsReversed[1].Tag)
                {
                    foreach (var pair in noPairsReversed)
                    {
                        pair.HasReveredPair = true;
                    }
                    CompletedPairs++;
                    this.RaisePropertyChanged(nameof(CompletedPairsString));
                }
                else
                {
                    foreach (var pair in noPairsReversed)
                    {
                        _reverseNext = true;
                    }
                }
            }
        }

        private void GenerateRandomTags()
        {
            var list = Enumerable.Range(1, (Columns * Rows) / 2).ToList();
            list.AddRange(list);
            _availableTags = list.OrderBy(a => Guid.NewGuid()).ToList();
        }

        private void AssignImage()
        {
            foreach (var card in Cards)
            {
                card.Image = Images[card.Tag];
            }
            _playGameStartTime =DateTime.Now;
        }
    }
}
