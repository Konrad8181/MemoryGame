using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Shapes;
using Avalonia.ReactiveUI;
using MemoryGame.Models;
using MemoryGame.ViewModels;
using ReactiveUI;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryGame.Views
{
    public partial class PlayView :  ReactiveUserControl<PlayViewModel>
    {
        private const int Marigin = 5;

        private PlayViewModel _playViewModel;

        public PlayView()
        {
            InitializeComponent();
            DrawCards();
            this.WhenActivated(d => d(ViewModel!.ShowDialog.RegisterHandler(DoShowDialogAsync)));

        }

        private async Task DoShowDialogAsync(InteractionContext<GameEndViewModel, Score?> interaction)
        {
            var dialog = new GameEndWindowView();
            dialog.DataContext = interaction.Input;

            if (Avalonia.Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var result = await dialog.ShowDialog<Score?>(desktop.MainWindow);
                interaction.SetOutput(result);
            }
        }

        private void DrawCards()
        {
            if (App.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                if (desktop.MainWindow.DataContext is MainWindowViewModel mainWindowViewModel)
                {
                    if (mainWindowViewModel.CurrentPage is PlayViewModel playViewModel)
                    {
                        _playViewModel = playViewModel;
                        ClearGrid();
                        CreateGridDefinitions();

                        foreach (var card in playViewModel.Cards)
                        {
                            var rec = new Rectangle()
                            {
                                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
                                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
                                Fill = card.Brush,
                                Stroke = card.Brush,
                                StrokeThickness = 2,
                                Margin = new Avalonia.Thickness(Marigin),
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
                            var image = new Image()
                            {
                                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                                Source = card.Image,
                                Tag = card.Guid,
                                Margin = new Avalonia.Thickness(Marigin + 2)
                            };

                            textBlock.SetValue(Grid.RowProperty, card.Position.x);
                            textBlock.SetValue(Grid.ColumnProperty, card.Position.y);

                            rec.SetValue(Grid.RowProperty, card.Position.x);
                            rec.SetValue(Grid.ColumnProperty, card.Position.y);

                            image.SetValue(Grid.RowProperty, card.Position.x);
                            image.SetValue(Grid.ColumnProperty, card.Position.y);

                            rec.PointerPressed += (s, e) => HandleClick(card);

                            CardsGrid.Children.Add(image);
                            CardsGrid.Children.Add(rec);
                            //CardsGrid.Children.Add(textBlock);
                        }
                    }
                }
            }
        }

        private void ClearGrid()
        {
            CardsGrid.Children.Clear();
            CardsGrid.RowDefinitions.Clear();
            CardsGrid.ColumnDefinitions.Clear();
        }

        private void CreateGridDefinitions()
        { 
            if (_playViewModel == null)
            {
                return;
            }
            var columnsSB = new StringBuilder();
            var rowsSB = new StringBuilder();
            for (var i = 0; i < _playViewModel.Columns; i++)
            {
                columnsSB.Append("*,");
            }
            columnsSB.Remove(columnsSB.Length - 1, 1);
            for (var i = 0; i < _playViewModel.Rows; i++)
            {
                rowsSB.Append("*,");
            }
            rowsSB.Remove(rowsSB.Length - 1, 1);
            CardsGrid.ColumnDefinitions = new ColumnDefinitions(columnsSB.ToString());
            CardsGrid.RowDefinitions = new RowDefinitions(rowsSB.ToString());
        }

        private void HandleClick(Card card)
        {
            _playViewModel.HandleClick(card);
            UpdateCardsVisibility();
        }

        private void UpdateCardsVisibility()
        {
            var cards = CardsGrid.Children.Where(c => c is Rectangle);
            foreach (var card in cards)
            {
                foreach (var vmCard in _playViewModel.Cards)
                {
                    if (vmCard.Guid.ToString() == ((Rectangle)card).Tag.ToString())
                    {
                        card.IsVisible = !vmCard.IsReversed;
                    }
                }
            }
        }
    }
}
