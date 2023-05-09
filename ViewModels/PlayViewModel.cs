using MemoryGame.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MemoryGame.ViewModels
{
    public class PlayViewModel : ViewModelBase
    {
        public const int Rows = 4;

        public const int Columns = 5;

        private List<int> _availableTags;

        public List<Card> Cards { get; }

        public PlayViewModel()
        {
            Cards = new List<Card>();
            GenerateRandomTags();
            CreateCards();
        }

        private void GenerateRandomTags()
        {
            var list = Enumerable.Range(1, (Columns * Rows) / 2).ToList();
            list.AddRange(list);
            _availableTags = list.OrderBy(a => Guid.NewGuid()).ToList();
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
            card.IsReversed = !card.IsReversed;
            Trace.WriteLine($"Card at position X={card.Position.x}, Y={card.Position.y} has been clicked.");
        }
    }
}
