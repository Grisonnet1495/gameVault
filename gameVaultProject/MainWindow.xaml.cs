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
        #endregion

        #region Constructor
        public MainWindow()
        {
            InitializeComponent();

            // Authenticate
            // Retrieve user data

            // Temporary
            currentUser = new User("MyPseudo", "François", "Caprasse", new Library());
            currentUser.Library.AddGame(new Game("Minecraft", "Sandbox", true, true, false, true, "Windows, MacOS, Linux", "https://www.minecraft.net", new DateTime(2009, 5, 17), true, "A sandbox game", "Ressources/Images/minecraft_game_image.jpg", DateTime.Now, "C:/XboxGames/Minecraft Launcher/Content/Minecraft.exe"));
            currentUser.Library.AddGame(new Game("Portal 2", "Puzzle", true, true, true, true, "Windows, MacOS, Linux", "https://store.steampowered.com/app/620/Portal_2/", new DateTime(2011, 4, 19), false, "A brilliant physics-based puzzle game", "Ressources/Images/default_game_image.png", DateTime.Now, ""));
            currentUser.Library.AddGame(new Game("Celeste", "Platformer", true, false, false, true, "Windows, MacOS, Linux, Switch, PS4, Xbox", "http://www.celestegame.com/", new DateTime(2018, 1, 25), true, "A challenging platformer about mental resilience", "Ressources/Images/default_game_image.png", DateTime.Now, ""));
            currentUser.Library.AddGame(new Game());
            currentUser.Library.AddGame(new Game());
            currentUser.Library.AddGame(new Game());

            DataContext = this;

            SelectedGame = currentUser.Library.GameList.First();

            showHome();
        }
        #endregion

        #region Show user control methods
        public void showHome()
        {
            MainContentControl.Content = new HomeUserControl(currentUser);
        }

        public void showFavorites()
        {
            MainContentControl.Content = new FavoritesUserControl(currentUser);
        }

        public void showLibrary()
        {
            MainContentControl.Content = new LibraryUserControl(currentUser);
        }

        public void showGame()
        {
            if (SelectedGame != null)
            {
                var gameUserControl = new GameUserControl(SelectedGame);
                MainContentControl.Content = gameUserControl;

                gameUserControl.EditGameButtonClicked += EditCurrentGame;
                gameUserControl.ExportGameButtonClicked += ExportCurrentGame;
            }
        }

        public void showEditGame()
        {
            var editGameUserControl = new EditGameUserControl(SelectedGame);
            MainContentControl.Content = editGameUserControl;

            editGameUserControl.ExitEditButtonClicked += ExitGameEdit;
            editGameUserControl.DeleteGameButtonClicked += DeleteCurrentGame;
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
            showHome();
        }

        private void FavoritesButton_Click(object sender, RoutedEventArgs e)
        {
            showFavorites();
        }

        private void LibraryButton_Click(object sender, RoutedEventArgs e)
        {
            showLibrary();
        }

        private void GameButton_Click(object sender, RoutedEventArgs e)
        {
            showGame();
        }

        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            Game newGame = new Game();
            currentUser.Library.GameList.Add(newGame);
            SelectedGame = newGame;
            showEditGame();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            var settingsDialog = new Settings();
            bool? result = settingsDialog.ShowDialog();
            // Note : Note finished
        }
        #endregion

        #region GameUserControl buttons click
        private void EditCurrentGame(object sender, EventArgs e)
        {
            showEditGame();
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
            showGame();
        }

        private void DeleteCurrentGame(object sender, EventArgs e)
        {
            currentUser.Library.RemoveGame(SelectedGame);
            SelectedGame = currentUser.Library.GameList.First();
            showHome();
        } 
        #endregion
    }
}
