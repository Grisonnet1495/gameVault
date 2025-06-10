using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using gameVaultClassLibrary;
using Microsoft.Win32;

namespace gameVaultProject
{
    public partial class GameUserControl : UserControl
    {
        #region Properties
        public Game Game { get; set; }

        public event EventHandler EditGameButtonClicked;
        #endregion

        #region Constructor
        public GameUserControl(Game game)
        {
            InitializeComponent();

            Game = game;
            DataContext = Game;

            // Set up the game image
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            if (Game.ImageName != null)
            {
                string imagePath = System.IO.Path.Combine(System.IO.Path.Combine(Config.LoadSetting(Config.appDataKey), Config.LoadSetting(Config.imagesFolderKey)), Game.ImageName);
                bitmap.UriSource = new Uri(imagePath, UriKind.Absolute);
            }
            else
            {
                bitmap.UriSource = new Uri("pack://application:,,,/Ressources/Images/default_game_image.png");
            }
            bitmap.CacheOption = BitmapCacheOption.OnLoad; // To release the image
            bitmap.EndInit();
            GameImage.Source = bitmap;

            // Display the favorite button icon
            if (Game.IsFavorite)
            {
                ToggleFavoriteImage.Source = new BitmapImage(new Uri("pack://application:,,,/Ressources/Icons/favorite_game_icon.png"));
            }
            else
            {
                ToggleFavoriteImage.Source = new BitmapImage(new Uri("pack://application:,,,/Ressources/Icons/not_favorite_game_icon.png"));
            }
        }
        #endregion

        #region Buttons click
        private async void LaunchGameButton_Click(object sender, RoutedEventArgs e)
        {
            if (Game.GamePath == null)
            {
                MessageBox.Show("No executable linked to this game", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string extension = System.IO.Path.GetExtension(Game.GamePath).ToLower();

            // Check if the path is valid
            if (!File.Exists(Game.GamePath) || extension != ".exe")
            {
                MessageBox.Show("Specified path isn't a valid executable", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                DateTime startTime = DateTime.Now;

                // Launch the game
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = Game.GamePath,
                    UseShellExecute = true
                };

                using (Process gameProcess = Process.Start(startInfo))
                {
                    if (gameProcess != null)
                    {
                        await Task.Run(() => gameProcess.WaitForExit());
                        DateTime endTime = DateTime.Now;

                        // Update the game info
                        TimeSpan gameDuration = endTime - startTime;
                        Game.TimePlayed += gameDuration;
                        Game.NbTimePlayed++;
                        Game.LastPlayedDate = DateTime.Now;

                        if (gameDuration < TimeSpan.FromSeconds(10))
                        {
                            MessageBox.Show("The game process was redirected to another executable. Time-tracking isn't going to work on this game.", "Process redirection", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while starting game : {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditGameButton_Click(object sender, RoutedEventArgs e)
        {
            EditGameButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void ExportGameButton_Click(object sender, RoutedEventArgs e)
        {
            // Ask the user to select a location
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Export game";
            saveFileDialog.Filter = "Game file (*.xml)|*.xml";
            saveFileDialog.FileName = Game.Title;

            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName; // Retrieve export file path

                // Export the game to this location
                Backup.ExportGameToFile(filePath, Game);

                MessageBox.Show("Game exported to : " + filePath, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ToggleFavoriteGameButton_Click(object sender, RoutedEventArgs e)
        {
            if (Game.IsFavorite)
            {
                Game.IsFavorite = false;
                ToggleFavoriteImage.Source = new BitmapImage(new Uri("pack://application:,,,/Ressources/Icons/not_favorite_game_icon.png"));
            }
            else
            {
                Game.IsFavorite = true;
                ToggleFavoriteImage.Source = new BitmapImage(new Uri("pack://application:,,,/Ressources/Icons/favorite_game_icon.png"));
            }
        }
        #endregion

        #region Hyperlink click
        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Hyperlink link)
            {
                string? url = (link.DataContext as Game)?.StoreUrl;

                // Check if the string is a valid URL
                if (!string.IsNullOrWhiteSpace(url) && Uri.IsWellFormedUriString(url, UriKind.Absolute))
                {
                    // Open the link
                    Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
                }
                else
                {
                    MessageBox.Show("Link not valid", "Cannot open link", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        #endregion
    }
}
