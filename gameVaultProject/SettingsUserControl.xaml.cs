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
    /// Logique d'interaction pour SettingsUserControl.xaml
    /// </summary>
    public partial class SettingsUserControl : UserControl
    {
        private User currentUser;
        private Authenticator authenticator;
        public event EventHandler ExitSettingsButtonClicked;

        public SettingsUserControl(User user)
        {
            InitializeComponent();

            currentUser = user;

            PseudoTextBox.Text = currentUser.Pseudo;

            AppDataFilePathTextBox.Text = Config.LoadSetting(Config.appDataKey);

            ExportGameComboBox.ItemsSource = currentUser.Library.GameList;

            authenticator = new Authenticator();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Change the current user pseudo if needed
            string newPseudo = PseudoTextBox.Text;

            if (currentUser.Pseudo != newPseudo && !string.IsNullOrWhiteSpace(newPseudo) && !authenticator.UserExists(newPseudo))
            {
                authenticator.ChangeUserPseudo(currentUser.Pseudo, newPseudo);
                Backup.ChangeUserPseudo(currentUser.Pseudo, newPseudo);
                currentUser.Pseudo = newPseudo;
            }

            // Change the app data folder if needed
            string newAppDataFolder = AppDataFilePathTextBox.Text;

            if (newAppDataFolder != Config.LoadSetting(Config.appDataKey))
            {
                if (!Directory.Exists(newAppDataFolder))
                {
                    Directory.CreateDirectory(newAppDataFolder);
                }

                // Test if the app can write in this folder
                try
                {
                    string testFilePath = System.IO.Path.Combine(newAppDataFolder, "testFile.txt");
                    using (FileStream fs = File.Create(testFilePath, 1, FileOptions.DeleteOnClose)) { }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Invalid new app data directory : " + ex.Message);
                    return;
                }

                Backup.ChangeAppDataFolder(newAppDataFolder);
            }

            // Exit the settings
            ExitSettingsButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ExitSettingsButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void CreateBackupButton_Click(object sender, RoutedEventArgs e)
        {
            // Ask the user to select a location
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Save backup";
            saveFileDialog.Filter = "Backup file (*.json)|*.json";
            saveFileDialog.FileName = "backup";

            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName; // Retrieve backup file path

                // Export the backup to this location
                Backup.ExportLibraryToFile(filePath, currentUser.Library);

                MessageBox.Show("Backup file created to : " + filePath);
            }
        }

        private void RestoreBackupButton_Click(object sender, RoutedEventArgs e)
        {
            // Ask the user to select a backup file
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Choose a backup file";
            openFileDialog.Filter = "Backup file (*.json)|*.json";

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFile = openFileDialog.FileName;

                // Import the backup
                Library library = Backup.ImportLibraryFromFile(selectedFile);

                if (library != null)
                {
                    currentUser.Library = library;

                    MessageBox.Show("Backup imported successfully !");
                }
                else
                {
                    MessageBox.Show("Incorrect backup file", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ExportGameButton_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve the selected game
            Game? selectedGame = ExportGameComboBox.SelectedItem as Game;

            if (selectedGame == null)
            {
                MessageBox.Show("Please select a game to export");
                return;
            }

            // Ask the user to select a location
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Export game";
            saveFileDialog.Filter = "Game file (*.xml)|*.xml";
            saveFileDialog.FileName = selectedGame.Title;

            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName; // Retrieve export file path

                // Export the game to this location
                Backup.ExportGameToFile(filePath, selectedGame);

                MessageBox.Show("Game exported to : " + filePath);
            }
        }

        private void ImportGameButton_Click(object sender, RoutedEventArgs e)
        {
            // Ask the user to select a game file
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Choose a game file";
            openFileDialog.Filter = "Game file (*.xml)|*.xml";

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFile = openFileDialog.FileName;

                // Import the game
                Game game = Backup.ImportGameFromFile(selectedFile);

                if (game != null)
                {
                    currentUser.Library.AddGame(game);

                    MessageBox.Show("Game imported successfully !");
                }
                else
                {
                    MessageBox.Show("Incorrect Game file", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void DeleteAllUserDataButton_Click(object sender, RoutedEventArgs e)
        {
            if (AcceptConsequencesCheckBox.IsChecked == true)
            {
                DeleteAllUserDataButton.IsEnabled = false;
                AcceptConsequencesCheckBox.IsEnabled = false;

                // Delete all current user data
                authenticator.RemoveUser(currentUser.Pseudo);
                Backup.DeleteUserData(currentUser);

                MessageBox.Show("Data of the current account deleted successfully ! The app is going to restart.", "Success", MessageBoxButton.OK);

                // Reload the app
                Process.Start(Process.GetCurrentProcess().MainModule.FileName);

                Application.Current.Shutdown();
            }
            else
            {
                MessageBox.Show("You have to accept the risks first");
            }
        }

        private void PseudoTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string newPseudo = PseudoTextBox.Text;

            // Inform the user about the new pseudo
            if (currentUser.Pseudo == newPseudo)
            {
                PseudoConfirmationTextBlock.Text = "";
                PseudoConfirmationTextBlock.Foreground = Brushes.White;
                return;
            }

            if (string.IsNullOrWhiteSpace(newPseudo))
            {
                PseudoConfirmationTextBlock.Text = "Incorrect pseudo";
                PseudoConfirmationTextBlock.Foreground = Brushes.IndianRed;
                return;
            }

            if (authenticator.UserExists(newPseudo))
            {
                PseudoConfirmationTextBlock.Text = "Not disponible";
                PseudoConfirmationTextBlock.Foreground = Brushes.IndianRed;
                return;
            }

            PseudoConfirmationTextBlock.Text = "Disponible";
            PseudoConfirmationTextBlock.Foreground = Brushes.LightGreen;
        }
    }
}
