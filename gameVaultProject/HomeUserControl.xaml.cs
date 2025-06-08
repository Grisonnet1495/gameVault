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
    public partial class HomeUserControl : UserControl
    {
        #region Properties
        public List<Game> RecentGames { get; set; }
        private const double durationForRecentGame = 10;
        #endregion

        #region Constructor
        public HomeUserControl(User user)
        {
            InitializeComponent();

            UpdateWelcomeTitle(user);

            ExtractRecentGames(user);

            InitializeHomePanel();
        }
        #endregion

        #region Methods
        public void UpdateWelcomeTitle(User user)
        {
            DateTime currentTime = DateTime.Now;
            int currentHour = currentTime.Hour;

            if (currentHour > 7 && currentHour < 12)
            {
                WelcomeUserTitleLabel.Content = "Good morning " + user.Pseudo + " !";
                WelcomeUserTextLabel.Content = "Are you ready for a new game ?";
            }
            else if (currentHour < 22)
            {
                WelcomeUserTitleLabel.Content = "Good evening " + user.Pseudo + " !";
                WelcomeUserTextLabel.Content = "What would you like to play today ?";
            }
            else
            {
                WelcomeUserTitleLabel.Content = "Aren't you sleeping " + user.Pseudo + " ?";
                WelcomeUserTextLabel.Content = "Ready for your night gaming session !";
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
                if (user.Library.GameList.Count > 0)
                {
                    SelectedGameLabel.Content = "All games";
                    RecentGames = user.Library.GameList;
                }
                else
                {
                    SelectedGameLabel.Content = "No game";
                }
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
                    ((MainWindow)Application.Current.MainWindow).ShowGame();
                };

                SelectedGameWrapPanel.Children.Add(card);
            }
        } 
        #endregion
    }
}
