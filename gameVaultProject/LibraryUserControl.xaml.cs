using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using gameVaultClassLibrary;

namespace gameVaultProject
{
    /// <summary>
    /// Logique d'interaction pour LibraryUserControl.xaml
    /// </summary>
    public partial class LibraryUserControl : UserControl
    {
        public List<Game> AllGames { get; set; }
        private SortMode currentSortMode;

        public enum SortMode
        {
            ByTitle,
            ByLastPlayed,
            ByTimePlayed,
            ByFavorites
        }

        public LibraryUserControl(User user)
        {
            InitializeComponent();

            ExtractAllGames(user);

            currentSortMode = SortMode.ByTitle;
            SortAllGamesList();
        }

        public void ExtractAllGames(User user)
        {
            AllGames = new List<Game>(user.Library.GameList);
        }

        public void InitializeGamePanel()
        {
            AllGamesWrapPanel.Children.Clear();

            if (AllGames.Count == 0)
            {
                // Inform the user
                TextBlock noFavoritesGamesTextBlock = new TextBlock();
                noFavoritesGamesTextBlock.Text = "No favorites games";
                noFavoritesGamesTextBlock.Foreground = new SolidColorBrush(Colors.White);
                noFavoritesGamesTextBlock.FontSize = 16;
                noFavoritesGamesTextBlock.Margin = new Thickness(20, 20, 20, 20);

                AllGamesWrapPanel.Children.Add(noFavoritesGamesTextBlock);
            }
            else
            {
                // Display all games
                foreach (var game in AllGames)
                {
                    var card = new GameCardUserControl(game);
                    card.DataContext = game;

                    card.GameClicked += (s, g) =>
                    {
                        ((MainWindow)Application.Current.MainWindow).SelectedGame = game;
                        ((MainWindow)Application.Current.MainWindow).ShowGame();
                    };

                    AllGamesWrapPanel.Children.Add(card);
                }
            }
        }

        private void SortButton_Click(object sender, RoutedEventArgs e)
        {
            // Switch to next mode
            currentSortMode = currentSortMode switch
            {
                SortMode.ByTitle => SortMode.ByLastPlayed,
                SortMode.ByLastPlayed => SortMode.ByTimePlayed,
                SortMode.ByTimePlayed => SortMode.ByFavorites,
                SortMode.ByFavorites => SortMode.ByTitle,
                _ => SortMode.ByTitle
            };

            // Sort the game list with this mode
            SortAllGamesList();
        }

        private void SortAllGamesList()
        {
            switch (currentSortMode)
            {
                case SortMode.ByTitle:
                    SortDescriptionLabel.Content = "By title";
                    AllGames = AllGames.OrderBy(g => g.Title).ToList();
                    break;
                case SortMode.ByLastPlayed:
                    SortDescriptionLabel.Content = "By last played";
                    AllGames = AllGames.OrderByDescending(g => g.LastPlayedDate).ToList();
                    break;
                case SortMode.ByTimePlayed:
                    SortDescriptionLabel.Content = "By time played";
                    AllGames = AllGames.OrderByDescending(g => g.TimePlayed).ToList();
                    break;
                case SortMode.ByFavorites:
                    SortDescriptionLabel.Content = "By favorites";
                    AllGames = AllGames.OrderByDescending(g => g.IsFavorite).ToList();
                    break;
            }

            InitializeGamePanel();
        }
    }
}
