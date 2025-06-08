using System.Windows;
using System.Windows.Input;
using gameVaultClassLibrary;

namespace gameVaultProject
{
    public partial class MainWindow : Window
    {
        #region Properties
        public Game? SelectedGame { get; set; }
        private User currentUser;
        //private Backup backup;
        #endregion

        #region Constructor
        public MainWindow()
        {
            InitializeComponent();

            // Open a authentication dialog
            AuthenticateWindow authenticateWindow = new AuthenticateWindow();
            bool? result = authenticateWindow.ShowDialog();

            if (result == true) // If the user is authenticated
            {
                currentUser = new User(authenticateWindow.Pseudo, authenticateWindow.Password, new Library($"{authenticateWindow.Pseudo}_Library"));

                // Retrieve user data
                Backup.SetUpBackup(currentUser);

                //currentUser.Library.AddGame(new Game("Minecraft", "Sandbox", true, true, false, true, "Windows, MacOS, Linux", "https://www.minecraft.net", new DateTime(2009, 5, 17), true, "A sandbox game", null, DateTime.Now, "C:/XboxGames/Minecraft Launcher/Content/Minecraft.exe", TimeSpan.Zero, 0));
                //currentUser.Library.AddGame(new Game("Portal 2", "Puzzle", true, true, true, true, "Windows, MacOS, Linux", "https://store.steampowered.com/app/620/Portal_2/", new DateTime(2011, 4, 19), false, "A brilliant physics-based puzzle game", null, DateTime.Now, null, new TimeSpan(2, 30, 0), 1));
                //currentUser.Library.AddGame(new Game("Celeste", "Platformer", true, false, false, true, "Windows, MacOS, Linux, Switch, PS4, Xbox", "http://www.celestegame.com/", new DateTime(2018, 1, 25), true, "A challenging platformer about mental resilience", null, DateTime.Now, null, new TimeSpan(2, 30, 0), 2));
                //currentUser.Library.AddGame(new Game());
                //currentUser.Library.AddGame(new Game());
                //currentUser.Library.AddGame(new Game());

                SelectedGame = null;

                DataContext = this;

                UpdateMainInfoPanel();

                ShowHome();
            }
            else
            {
                this.Close();
            }
        }
        #endregion

        #region Show user control methods
        public void ShowHome()
        {
            MainContentControl.Content = new HomeUserControl(currentUser);

            UpdateMainInfoPanel();
        }

        public void ShowFavorites()
        {
            MainContentControl.Content = new FavoritesUserControl(currentUser);

            UpdateMainInfoPanel();
        }

        public void ShowLibrary()
        {
            MainContentControl.Content = new LibraryUserControl(currentUser);

            UpdateMainInfoPanel();
        }

        public void ShowGame()
        {
            if (SelectedGame != null) // If there is a game selected
            {
                var gameUserControl = new GameUserControl(SelectedGame);
                MainContentControl.Content = gameUserControl;

                gameUserControl.EditGameButtonClicked += EditCurrentGame;

                UpdateGameInfoPanel();
            }
            else
            {
                MessageBox.Show("Please select a game", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void ShowEditGame()
        {
            if (SelectedGame != null) // If there is a game selected
            {
                var editGameUserControl = new EditGameUserControl(SelectedGame);
                MainContentControl.Content = editGameUserControl;

                editGameUserControl.ExitEditButtonClicked += ExitGameEdit;
                editGameUserControl.DeleteGameButtonClicked += DeleteCurrentGame;

                UpdateGameInfoPanel();
            }
            else
            {
                MessageBox.Show("Please select a game", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void ShowSettings()
        {
            var settingsUserControl = new SettingsUserControl(currentUser);
            MainContentControl.Content = settingsUserControl;

            settingsUserControl.ExitSettingsButtonClicked += ExitSettings;
        }

        #endregion

        #region Window actions methods
        // Minimize the window
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        // Maximize or restore the window
        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = (WindowState == WindowState.Maximized) ? WindowState.Normal : WindowState.Maximized;
        }

        // Close the window
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Backup.SaveDataToFile(currentUser);

            this.Close();
        }

        // Drag the window
        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        } 
        #endregion

        #region Main window buttons clicks
        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            ShowHome();
        }

        private void FavoritesButton_Click(object sender, RoutedEventArgs e)
        {
            ShowFavorites();
        }

        private void LibraryButton_Click(object sender, RoutedEventArgs e)
        {
            ShowLibrary();
        }

        private void GameButton_Click(object sender, RoutedEventArgs e)
        {
            ShowGame();
        }

        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            Game newGame = new Game();
            currentUser.Library.GameList.Add(newGame);
            SelectedGame = newGame;
            ShowEditGame();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            ShowSettings();
        }
        #endregion

        #region GameUserControl buttons click
        private void EditCurrentGame(object sender, EventArgs e)
        {
            ShowEditGame();
        }
        #endregion

        #region EditGameUserControl buttons click
        private void ExitGameEdit(object sender, EventArgs e)
        {
            Backup.SaveDataToFile(currentUser);

            ShowGame();
        }

        private void DeleteCurrentGame(object sender, EventArgs e)
        {
            currentUser.Library.RemoveGame(SelectedGame);
            SelectedGame = currentUser.Library.GameList.First();
            Backup.SaveDataToFile(currentUser);

            ShowHome();
        }
        #endregion

        #region SettingsUserControl button click
        private void ExitSettings(object sender, EventArgs e)
        {
            Backup.SaveDataToFile(currentUser);

            UpdateMainInfoPanel();
            ShowHome();
        } 
        #endregion

        #region Other methods
        // Update the main information panel
        public void UpdateMainInfoPanel()
        {
            TimeSpan gameTime = TimeSpan.Zero;
            DateTime lastTimePlayed = new DateTime();
            Game bestGame = null;

            // Initialize the base UI
            InfoPanelMainTitleLabel.Content = "Total hours played";
            InfoPanelSecondaryTitleLabel.Content = "Best game";

            // Calculate the total game time
            TimeSpan totalGameTime = currentUser.Library.CalculateGameTime();

            HoursPlayedLabel.Content = $"{(int)totalGameTime.TotalHours} hours";
            MinutesPlayedLabel.Content = $"{totalGameTime.Minutes} minutes";
            SecondsPlayedLabel.Content = $"{totalGameTime.Seconds} seconds";

            // Find the best game
            foreach (Game game in currentUser.Library.GameList)
            {
                if (game.TimePlayed > gameTime)
                {
                    bestGame = game;
                    gameTime = game.TimePlayed;
                    lastTimePlayed = game.LastPlayedDate;
                }
                else if (game.TimePlayed == gameTime)
                {
                    if (game.LastPlayedDate > lastTimePlayed)
                    {
                        bestGame = game;
                        gameTime = game.TimePlayed;
                        lastTimePlayed = game.LastPlayedDate;
                    }
                }
            }

            // If a best game has been found
            if (bestGame != null)
            {
                string title = bestGame.Title;
                // If the game title is too long
                if (title.Length > 21)
                {
                    title = title.Substring(0, 18) + "...";
                }
                InfoPanelSecondaryContentLabel.Content = title;
                BestGameHoursPlayedLabel.Content = bestGame.TimePlayed.Hours + " hours";
                BestGameNbTimePlayedLabel.Content = bestGame.NbTimePlayed + " times in total";
            }
            else
            {
                InfoPanelSecondaryContentLabel.Content = "No best game yet";
                BestGameHoursPlayedLabel.Content = "";
                BestGameNbTimePlayedLabel.Content = "";
            }
        }

        // Update the game information panel
        public void UpdateGameInfoPanel()
        {
            if (SelectedGame != null)
            {
                // Initialize the base UI
                InfoPanelMainTitleLabel.Content = "Hours played";
                InfoPanelSecondaryTitleLabel.Content = "Game rank";

                // Calculate current game time
                HoursPlayedLabel.Content = $"{(int)SelectedGame.TimePlayed.TotalHours} hours";
                MinutesPlayedLabel.Content = $"{SelectedGame.TimePlayed.Minutes} minutes";
                SecondsPlayedLabel.Content = $"{SelectedGame.TimePlayed.Seconds} seconds";

                // Find the current game rank
                List<Game> sortedGames = new List<Game>(currentUser.Library.GameList);
                sortedGames = sortedGames
                    .OrderByDescending(g => g.TimePlayed)
                    .ThenByDescending(g => g.LastPlayedDate)
                    .ToList();
                int gameRank = sortedGames.IndexOf(SelectedGame) + 1;

                // Display the current game rank using an specific notation
                string ordinal;
                int rem100 = gameRank % 100;
                if (rem100 >= 11 && rem100 <= 13)
                    ordinal = gameRank + "th";
                else
                {
                    switch (gameRank % 10)
                    {
                        case 1:
                            ordinal = gameRank + "st";
                            break;
                        case 2:
                            ordinal = gameRank + "nd";
                            break;
                        case 3:
                            ordinal = gameRank + "rd";
                            break;
                        default:
                            ordinal = gameRank + "th";
                            break;
                    }
                }
                InfoPanelSecondaryContentLabel.Content = ordinal + " place";
                BestGameHoursPlayedLabel.Content = "";
                BestGameNbTimePlayedLabel.Content = "";
            }
            else
            {
                UpdateMainInfoPanel();
            }
        }
        #endregion
    }
}
