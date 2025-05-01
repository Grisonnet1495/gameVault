using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public event EventHandler ExitSettingsButtonClicked;

        public SettingsUserControl(User user)
        {
            InitializeComponent();

            currentUser = user;

            PseudoTextBox.Text = currentUser.Pseudo;

            AppDataFilPathTextBox.Text = Config.LoadSetting(Config.appDataKey);

            ExportGameComboBox.ItemsSource = currentUser.Library.GameList;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            //currentUser.Pseudo = PseudoTextBox.Text;

            // Note : To continue
            // If the pseudo has changed :
                // Change library name ?
                // Change user library filename
                // Change user pseudo in libraries config file
                // Change user pseudo in user file
            // If the new app data folder has changer :
                // Check if the new location exists
                // Move all app data to the new location
                // It it worked :
                    // Inform the user
                // Else :
                    // Display a error message to the user

            ExitSettingsButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ExitSettingsButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void CreateBackupButton_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Save backup";
            saveFileDialog.Filter = "Backup file (*.json)|*.json";
            saveFileDialog.FileName = "backup";

            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName; // Retrieve backup file path

                Backup.ExportLibraryToFile(filePath, currentUser.Library);

                MessageBox.Show("Backup file created to : " + filePath);
            }
        }

        private void RestoreBackupButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Choose a backup file";
            openFileDialog.Filter = "Backup file (*.json)|*.json";

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFile = openFileDialog.FileName;

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
            Game selectedGame = ExportGameComboBox.SelectedItem as Game;

            if (selectedGame == null)
            {
                MessageBox.Show("Please select a game to export");
                return;
            }

            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Export game";
            saveFileDialog.Filter = "Game file (*.xml)|*.xml";
            saveFileDialog.FileName = selectedGame.Title;

            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName; // Retrieve export file path

                Backup.ExportGameToFile(filePath, selectedGame);

                MessageBox.Show("Game exported to : " + filePath);
            }
        }

        private void ImportGameButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Choose a game file";
            openFileDialog.Filter = "Game file (*.xml)|*.xml";

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFile = openFileDialog.FileName;

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
                // Delete user library file
                // Delete user from libraries file
                // Delete user from password file
                // Note : To continue

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
    }
}
