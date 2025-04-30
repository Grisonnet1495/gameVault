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
    /// Logique d'interaction pour HomeUserControl.xaml
    /// </summary>
    public partial class HomeUserControl : UserControl
    {
        public List<Game> RecentGames { get; set; }
        private const double durationForRecentGame = 1;

        public HomeUserControl(User user)
        {
            InitializeComponent();

            UpdateWelcomeTitle(user);

            ExtractRecentGames(user);

            InitializeHomePanel();
        }

        public void UpdateWelcomeTitle(User user)
        {
            DateTime currentTime = DateTime.Now;
            int currentHour = currentTime.Hour;

            if (currentHour > 7 && currentHour < 12)
            {
                WelcomeUserTitleLabel.Content = "Goog morning " + user.Pseudo + " !";
            }
            else if (currentHour < 22)
            {
                WelcomeUserTitleLabel.Content = "Goog evening " + user.Pseudo + " !";
            }
            else
            {
                WelcomeUserTitleLabel.Content = "Not sleeping " + user.Pseudo + " ?";
            }
        }

        public void ExtractRecentGames(User user)
        {
            RecentGames = new List<Game>();

            foreach (Game game in user.Library.GameList)
            {
                if (game.LastPlayedDate > DateTime.Now.AddMinutes(-durationForRecentGame))
                {
                    RecentGames.Add(game);
                }
            }

            if (RecentGames.Count > 0)
            {
                SelectedGameLabel.Content = "Recent games";
            }
            else
            {
                SelectedGameLabel.Content = "All games";
                RecentGames = user.Library.GameList;
            }
        }

        public void InitializeHomePanel()
        {
            SelectedGameWrapPanel.Children.Clear();

            foreach (var game in RecentGames)
            {
                var card = new GameCardUserControl(game);
                card.DataContext = game;

                card.GameClicked += (s, g) =>
                {
                    ((MainWindow)Application.Current.MainWindow).SelectedGame = game;
                    ((MainWindow)Application.Current.MainWindow).showGame();
                };

                SelectedGameWrapPanel.Children.Add(card);
            }
        }
    }
}
