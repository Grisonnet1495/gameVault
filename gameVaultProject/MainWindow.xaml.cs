using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using gameVaultClassLibrary;

namespace gameVaultProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Properties
        public User currentUser {  get; set; }

        public Game SelectedGame { get; set; }
        private Backup backup;
        #endregion

        #region Constructor
        public MainWindow()
        {
            InitializeComponent();

            // Authenticate
            AuthenticateUser authenticateUser = new AuthenticateUser();
            bool? result = authenticateUser.ShowDialog();//only open one window and wait for it to close
            if(result == true)//if the user is authenticated and the window (of the login) is closed correctly
            {
                currentUser = new User("MyPseudo", "myPassword", new Library());
                // Retrieve user data
                backup = new Backup(currentUser);

                //currentUser.Library.AddGame(new Game("Minecraft", "Sandbox", true, true, false, true, "Windows, MacOS, Linux", "https://www.minecraft.net", new DateTime(2009, 5, 17), true, "A sandbox game", "Ressources/Images/minecraft_game_image.jpg", DateTime.Now, "C:/XboxGames/Minecraft Launcher/Content/Minecraft.exe", TimeSpan.Zero, 0));
                //currentUser.Library.AddGame(new Game("Portal 2", "Puzzle", true, true, true, true, "Windows, MacOS, Linux", "https://store.steampowered.com/app/620/Portal_2/", new DateTime(2011, 4, 19), false, "A brilliant physics-based puzzle game", "Ressources/Images/default_game_image.png", DateTime.Now, "", new TimeSpan(2, 30, 0), 1));
                //currentUser.Library.AddGame(new Game("Celeste", "Platformer", true, false, false, true, "Windows, MacOS, Linux, Switch, PS4, Xbox", "http://www.celestegame.com/", new DateTime(2018, 1, 25), true, "A challenging platformer about mental resilience", "Ressources/Images/default_game_image.png", DateTime.Now, "", new TimeSpan(2, 30, 0), 2));
                //currentUser.Library.AddGame(new Game());
                //currentUser.Library.AddGame(new Game());
                //currentUser.Library.AddGame(new Game());

                SelectedGame = null;

                DataContext = this;

                CalculateGameTime();
                UpdateInfoPanel();

                ShowHome();
            }
            else if(result == false || result == null)//If the user is not authenticated or the window (of the login) is closed without being authenticated
            {
                this.Close();
            }
        }
        #endregion

        #region Show user control methods
        public void ShowHome()
        {
            MainContentControl.Content = new HomeUserControl(currentUser);
        }

        public void ShowFavorites()
        {
            MainContentControl.Content = new FavoritesUserControl(currentUser);
        }

        public void ShowLibrary()
        {
            MainContentControl.Content = new LibraryUserControl(currentUser);
        }

        public void ShowGame()
        {
            if (SelectedGame != null)
            {
                var gameUserControl = new GameUserControl(SelectedGame);
                MainContentControl.Content = gameUserControl;

                gameUserControl.EditGameButtonClicked += EditCurrentGame;
                gameUserControl.ExportGameButtonClicked += ExportCurrentGame;
            }
            else
            {
                MessageBox.Show("Please select a game", "Error", MessageBoxButton.OK);
            }
        }

        public void ShowEditGame()
        {
            if (SelectedGame != null)
            {
                var editGameUserControl = new EditGameUserControl(SelectedGame);
                MainContentControl.Content = editGameUserControl;

                editGameUserControl.ExitEditButtonClicked += ExitGameEdit;
                editGameUserControl.DeleteGameButtonClicked += DeleteCurrentGame;
            }
            else
            {
                MessageBox.Show("Please select a game", "Error", MessageBoxButton.OK);
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
            backup.SaveDataToFile(currentUser);

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

        #region Main window button clicks
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

        private void ExportCurrentGame(object sender, EventArgs e)
        {
            // Note : To do
            MessageBox.Show("Export game !");
        }

        // Note : Move the launch game method
        #endregion

        #region EditGameUserControl buttons clicks
        private void ExitGameEdit(object sender, EventArgs e)
        {
            UpdateInfoPanel();
            ShowGame();
        }

        private void DeleteCurrentGame(object sender, EventArgs e)
        {
            currentUser.Library.RemoveGame(SelectedGame);
            SelectedGame = currentUser.Library.GameList.First();
            UpdateInfoPanel();
            ShowHome();
        }
        #endregion

        #region SettingsUserControl button clicks
        private void ExitSettings(object sender, EventArgs e)
        {
            UpdateInfoPanel();
            ShowHome();
        } 
        #endregion

        #region Other methods
        public void CalculateGameTime()
        {
            TimeSpan totalGameTime = currentUser.Library.CalculateGameTime();

            hoursPlayedLabel.Content = $"{(int)totalGameTime.Hours} hours";
            minutesPlayedLabel.Content = $"{totalGameTime.Minutes} minutes";
            minutesPlayedLabel.Content = $"{totalGameTime.Seconds} seconds";
        }

        public void UpdateInfoPanel()
        {
            TimeSpan gameTime = TimeSpan.Zero;
            double gameNbPlayed = 0;
            DateTime lastTimePlayed = new DateTime();
            Game bestGame = null;

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

            if (bestGame != null)
            {
                string title = bestGame.Title;
                if (title.Length > 21)
                {
                    title = title.Substring(0, 18) + "...";
                }
                BestGameTitleLabel.Content = title;
                BestGameHoursPlayedLabel.Content = bestGame.TimePlayed.Hours + " hours";
                BestGameNbTimePlayedLabel.Content = bestGame.NbTimePlayed + " times in total";
            }
            else
            {
                BestGameTitleLabel.Content = "No best game yet";
                BestGameHoursPlayedLabel.Content = "";
                BestGameNbTimePlayedLabel.Content = "";
            }
        }
        #endregion
    }
}
