﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using gameVaultClassLibrary;

namespace gameVaultProject
{
    public partial class LibraryUserControl : UserControl
    {
        #region Properties
        public List<Game> AllGames { get; set; }
        private SortMode currentSortMode;

        public enum SortMode
        {
            ByTitle,
            ByLastPlayed,
            ByTimePlayed,
            ByFavorites
        }
        #endregion

        #region Constructor
        public LibraryUserControl(User user)
        {
            InitializeComponent();

            ExtractAllGames(user);

            currentSortMode = SortMode.ByTitle;
            SortAllGamesList();
        }
        #endregion

        #region Methods
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
        #endregion

        #region Button click
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
        #endregion
    }
}
