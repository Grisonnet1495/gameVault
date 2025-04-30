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

        // Note : Temporary
        public List<Game> Games { get; set; }
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
            currentUser.Library.AddGame(new Game(0, "Minecraft", "Sandbox", true, true, false, true, "Windows, MacOS, Linux", "www.minecraft.net", new DateTime(2009, 5, 17), false, "A sandbox game", "Ressources/Images/minecraft_game_image.jpg", DateTime.Now));
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

                gameUserControl.LaunchGameButtonClicked += GameUserControl_LaunchButtonClicked;
                gameUserControl.EditGameButtonClicked += GameUserControl_EditButtonClicked;
                gameUserControl.ExportGameButtonClicked += GameUserControl_ExportButtonClicked;
                gameUserControl.DeleteGameButtonClicked += GameUserControl_DeleteButtonClicked;
            }
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

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            var settingsDialog = new Settings();
            bool? result = settingsDialog.ShowDialog();
            // Note : Note finished
        }
        #endregion

        #region GameUserControl button clicks
        private void GameUserControl_LaunchButtonClicked(object sender, EventArgs e)
        {
            MessageBox.Show("Launch game !");
        }

        private void GameUserControl_EditButtonClicked(object sender, EventArgs e)
        {
            MessageBox.Show("Modify game !");
        }

        private void GameUserControl_ExportButtonClicked(object sender, EventArgs e)
        {
            MessageBox.Show("Export game !");
        }

        private void GameUserControl_DeleteButtonClicked(object sender, EventArgs e)
        {
            MessageBox.Show("Delete game !");
        } 
        #endregion

        #region Other methods
        private void OnGameSelected(Game selectedGame)
        {
            SelectedGame = selectedGame;
            showGame();
        } 
        #endregion
    }
}
