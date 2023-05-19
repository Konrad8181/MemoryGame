using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using System;
using static MemoryGame.Delegate;

namespace MemoryGame.Models
{
    public class Card
    {
        public Guid Guid { get; }

        public IBitmap Image { get; set; }

        public (int x, int y) Position { get; }

        public event Click OnClik;

        public bool IsReversed { get; set; }

        public bool HasReveredPair { get; set; }

        public int Tag { get; set; }

        public Brush Brush { get; set; }

        public Card(int x, int y, Click click, int tag, string imagePath)
        {
            Guid = Guid.NewGuid();
            Position = new (x, y);
            OnClik = click;
            Tag = tag;
            IsReversed = false;
            HasReveredPair = false;
            Random rand = new Random();
            Brush = new SolidColorBrush(Color.FromRgb((byte)rand.Next(0, 256), (byte)rand.Next(0, 256), (byte)rand.Next(0, 256)));
        }
    }
}
