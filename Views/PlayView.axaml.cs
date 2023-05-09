using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Shapes;
using MemoryGame.Models;
using MemoryGame.ViewModels;
using System.Diagnostics;
using System.Linq;

namespace MemoryGame.Views
{
    public partial class PlayView : UserControl
    {
        private PlayViewModel _playViewModel;

        public PlayView()
        {
            InitializeComponent();
            DrawCards();
        }

        private void DrawCards()
        {
            var marigin = 5;
            if (App.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                if (desktop.MainWindow.DataContext is MainWindowViewModel mainWindowViewModel)
                {
                    if (mainWindowViewModel.CurrentPage is PlayViewModel playViewModel)
                    {
                        _playViewModel = playViewModel;

                        foreach (var card in playViewModel.Cards)
                        {
                            var rec = new Rectangle()
                            {
                                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
                                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
                                Fill = card.Brush,
                                Stroke = card.Brush,
                                StrokeThickness = 2,
                                Margin = new Avalonia.Thickness(marigin),
                                IsVisible = !card.IsReversed,
                                Tag = card.Guid
                            };
                            var textBlock = new TextBlock()
                            {
                                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                                Text = card.Tag.ToString(),
                                FontSize = 32
                            };

                            textBlock.SetValue(Grid.RowProperty, card.Position.x);
                            textBlock.SetValue(Grid.ColumnProperty, card.Position.y);

                            rec.PointerPressed += (s, e) => HandleClick(card);

                            rec.SetValue(Grid.RowProperty, card.Position.x);
                            rec.SetValue(Grid.ColumnProperty, card.Position.y);

                            CardsGird.Children.Add(rec);
                            CardsGird.Children.Add(textBlock);
                        }
                    }
                }
            }
        }

        public void HandleClick(Card card)
        {
            _playViewModel.HandleClick(card);
            var cards = CardsGird.Children.Where(c => c is Rectangle);
            var singelCard = cards.First(c => ((Rectangle)c).Tag.ToString() == card.Guid.ToString());
            if (singelCard != null) 
            {
                singelCard.IsVisible = !card.IsReversed;
            }
        }
    }
}
