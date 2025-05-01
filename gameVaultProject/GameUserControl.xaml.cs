using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using Microsoft.Win32;

namespace gameVaultProject
{
    /// <summary>
    /// Logique d'interaction pour GameUserControl.xaml
    /// </summary>
    public partial class GameUserControl : UserControl
    {
        public Game Game {  get; set; }

        public event EventHandler LaunchGameButtonClicked;
        public event EventHandler EditGameButtonClicked;
        public event EventHandler ExportGameButtonClicked;

        // Calculated property
        public bool HasValidStoreUrl => !string.IsNullOrWhiteSpace(Game.StoreUrl) && Uri.IsWellFormedUriString(Game.StoreUrl, UriKind.Absolute);

        public GameUserControl(Game game)
        {
            InitializeComponent();

            Game = game;
            DataContext = Game;

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

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Hyperlink link)
            {
                string? url = (link.DataContext as Game)?.StoreUrl;

                if (!string.IsNullOrWhiteSpace(url) && Uri.IsWellFormedUriString(url, UriKind.Absolute))
                {
                    Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
                }
                else
                {
                    MessageBox.Show("Link not valid", "Cannot open link", MessageBoxButton.OK);
                }
            }
        }

        private async void LaunchGameButton_Click(object sender, RoutedEventArgs e)
        {
            string extension = System.IO.Path.GetExtension(Game.GamePath).ToLower();

            if (!File.Exists(Game.GamePath) || extension != ".exe")
            {
                MessageBox.Show("Specified path isn't a valid executable");
                return;
            }

            try
            {
                DateTime startTime = DateTime.Now;

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
                            MessageBox.Show("The game process was redirected to another executable. Time-tracking isn't going to work on this game.", "Process redirection", MessageBoxButton.OK);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while starting game : {ex.Message}");
            }
            

            Console.WriteLine("Game " + Game.GamePath + " started !");
        }

        private void EditGameButton_Click(object sender, RoutedEventArgs e)
        {
            EditGameButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void ExportGameButton_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Export game";
            saveFileDialog.Filter = "Game file (*.xml)|*.xml";
            saveFileDialog.FileName = Game.Title;

            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName; // Retrieve export file path

                Backup.ExportGameToFile(filePath, Game);

                MessageBox.Show("Game exported to : " + filePath);
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
    }
}
