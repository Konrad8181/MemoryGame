using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Shapes;
using MemoryGame.ViewModels;

namespace MemoryGame.Views
{
    public partial class PlayView : UserControl
    {
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
                        foreach (var card in playViewModel.Cards)
                        {
                            var rec = new Rectangle()
                            {
                                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
                                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
                                Fill = card.Brush,
                                Stroke = card.Brush,
                                StrokeThickness = 2,
                                Margin = new Avalonia.Thickness(marigin)
                            };

                            rec.PointerPressed += (s, e) => card.HandleClick(s, e);
                            rec.SetValue(Grid.RowProperty, card.Position.x);
                            rec.SetValue(Grid.ColumnProperty, card.Position.y);
                            CardsGird.Children.Add(rec);
                        }
                    }
                }
            }
        }
    }
}
