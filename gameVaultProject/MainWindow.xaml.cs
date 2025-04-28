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
        #region Constructor
        public MainWindow()
        {
            InitializeComponent();

            showHome();
        }
        #endregion

        #region Show user control methods
        public void showHome()
        {
            MainContentControl.Content = new HomeUserControl();
        }

        public void showFavorites()
        {
            MainContentControl.Content = new FavoritesUserControl();
        }

        public void showLibrary()
        {
            MainContentControl.Content = new LibraryUserControl(new Library());
        }

        public void showGame()
        {
            MainContentControl.Content = new GameUserControl();
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

        #region Button clicks
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
        } 
        #endregion
    }
}
