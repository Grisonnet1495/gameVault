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
    public partial class FavoritesUserControl : UserControl
    {
        public List<Game> FavoritesGames { get; set; }

        public FavoritesUserControl(User user)
        {
            InitializeComponent();

            ExtractRecentGames(user);

            InitializeFavoritesPanel();
        }

        public void ExtractRecentGames(User user)
        {
            FavoritesGames = new List<Game>();

            foreach (Game game in user.Library.GameList)
            {
                if (game.IsFavorite)
                {
                    FavoritesGames.Add(game);
                }
            }
        }

        public void InitializeFavoritesPanel()
        {
            FavoritesGamesWrapPanel.Children.Clear();

            if (FavoritesGames.Count == 0)
            {
                TextBlock noFavoritesGamesTextBlock = new TextBlock();
                noFavoritesGamesTextBlock.Text = "No favorites games";
                noFavoritesGamesTextBlock.Foreground = new SolidColorBrush(Colors.White);
                noFavoritesGamesTextBlock.FontSize = 16;
                noFavoritesGamesTextBlock.Margin = new Thickness(20, 20, 20, 20);

                FavoritesGamesWrapPanel.Children.Add(noFavoritesGamesTextBlock);
            }
            else
            {
                foreach (var game in FavoritesGames)
                {
                    var card = new GameCardUserControl(game);
                    card.DataContext = game;

                    card.GameClicked += (s, g) =>
                    {
                        ((MainWindow)Application.Current.MainWindow).SelectedGame = game;
                        ((MainWindow)Application.Current.MainWindow).ShowGame();
                    };

                    FavoritesGamesWrapPanel.Children.Add(card);
                }
            }
        }
    }
}
