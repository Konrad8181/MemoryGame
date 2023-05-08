using Avalonia.Controls;
using Avalonia.Media;
using System;
using System.Diagnostics;
using static MemoryGame.Delegate;

namespace MemoryGame.Models
{
    public class Card
    {
        public Image Image { get; set; }

        public (int x, int y) Position{ get; set; }

        public event Click OnClick;

        public bool IsReversed { get; set; }

        public bool HasReversedPair { get; set; }

        public int Tag { get; set; }

        public Brush Brush { get; set; }

        public Card(int x, int y, Click click, int tag, string imagePath)
        {
            Position = new(x, y);
            OnClick = click;
            Tag = tag;
            Image = new Image();
            IsReversed = false;
            HasReversedPair = false;
            Random rand = new Random();
            Brush = new SolidColorBrush(Color.FromRgb((byte)rand.Next(0, 256), (byte)rand.Next(0, 256), (byte)rand.Next(0, 256)));
            
        }

        public void HandleClick(object sender, EventArgs e)
        {
            Trace.WriteLine($"Card at position X={Position.x}, Y={Position.y} has been clicked.");
        }
    }
}
