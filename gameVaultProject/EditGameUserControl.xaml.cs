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
    /// Logique d'interaction pour EditGameUserControl.xaml
    /// </summary>
    public partial class EditGameUserControl : UserControl
    {
        #region Properties
        public Game currentGame { get; set; }

        public event EventHandler ExitEditButtonClicked;
        public event EventHandler DeleteGameButtonClicked; 
        #endregion

        #region Constructor
        public EditGameUserControl(Game game)
        {
            InitializeComponent();

            currentGame = game;
            DataContext = currentGame;

            if (currentGame.ImageName == null)
            {
                GameImageConfirmationTextBlock.Text = "No image";
                GameImageConfirmationTextBlock.Foreground = Brushes.White;
            }

            if (currentGame.GamePath == null)
            {
                GameExecutableConfirmationTextBlock.Text = "No executable";
                GameExecutableConfirmationTextBlock.Foreground = Brushes.White;
            }
        }
        #endregion

        #region Buttons click
        private void ExitEditButton_Click(object sender, RoutedEventArgs e)
        {
            ExitEditButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void DeleteGameButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this game ?", "Delete game", MessageBoxButton.OKCancel, MessageBoxImage.Warning);

            if (result == MessageBoxResult.OK)
            {
                DeleteGameButtonClicked?.Invoke(this, EventArgs.Empty);
            }
        }

        private void ChooseGameImageButton_Click(object sender, RoutedEventArgs e)
        {
            // Ask the user to select an image file
            var openFileDialog = new OpenFileDialog
            {
                Title = "Choose an executable file",
                Filter = "Image files (*.bmp;*.jpg;*.jpeg;*.png;*.gif;*.ico)|*.bmp;*.jpg;*.jpeg;*.png;*.gif;*.ico",
                CheckFileExists = true,
                CheckPathExists = true,
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    string selectedFile = openFileDialog.FileName;

                    string extension = System.IO.Path.GetExtension(selectedFile);
                    var allowedExtensions = new[] { ".bmp", ".jpg", ".jpeg", ".png", ".gif", ".ico" };

                    // Check if the selected file is valid
                    if (!string.IsNullOrWhiteSpace(selectedFile) && File.Exists(selectedFile) && allowedExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
                    {
                        // Delete the old image, if it exists
                        if (currentGame.ImageName != null)
                        {
                            string oldImagePath = System.IO.Path.Combine(System.IO.Path.Combine(Config.LoadSetting(Config.appDataKey), Config.LoadSetting(Config.imagesFolderKey)), currentGame.ImageName);

                            if (File.Exists(oldImagePath))
                            {
                                File.Delete(oldImagePath);
                            }
                        }

                        // Generate a new file name
                        string imageId = Guid.NewGuid().ToString();
                        currentGame.ImageName = imageId + extension;

                        // Copy the file to the new location
                        string newImagePath = System.IO.Path.Combine(System.IO.Path.Combine(Config.LoadSetting(Config.appDataKey), Config.LoadSetting(Config.imagesFolderKey)), currentGame.ImageName);
                        File.Copy(selectedFile, newImagePath);

                        // Inform the user
                        GameImageConfirmationTextBlock.Text = "Image accepted";
                        GameImageConfirmationTextBlock.Foreground = Brushes.LightGreen;
                    }
                    else
                    {
                        MessageBox.Show("Invalid file selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                        GameImageConfirmationTextBlock.Text = "No changes made";
                        GameImageConfirmationTextBlock.Foreground = Brushes.White;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while adding the new image : " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                    GameImageConfirmationTextBlock.Text = "No changes made";
                    GameImageConfirmationTextBlock.Foreground = Brushes.White;
                }
            }
        }

        private void ChooseGameExecutableButton_Click(object sender, RoutedEventArgs e)
        {
            // Ask the user to select an executable file
            var openFileDialog = new OpenFileDialog
            {
                Title = "Choose an executable file",
                Filter = "Executable files (*.exe)|*.exe",
                CheckFileExists = true,
                CheckPathExists = true,
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFile = openFileDialog.FileName;

                // Check if the selected file is correct
                if (!string.IsNullOrWhiteSpace(selectedFile) && File.Exists(selectedFile) && string.Equals(System.IO.Path.GetExtension(selectedFile), ".exe", StringComparison.OrdinalIgnoreCase))
                {
                    currentGame.GamePath = selectedFile;

                    // Inform the user
                    GameExecutableConfirmationTextBlock.Text = "Executable added";
                    GameExecutableConfirmationTextBlock.Foreground = Brushes.LightGreen;
                }
                else
                {
                    MessageBox.Show("Invalid file selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                    GameExecutableConfirmationTextBlock.Text = "No changes made";
                    GameExecutableConfirmationTextBlock.Foreground = Brushes.White;
                }
            }
        }
        #endregion
    }
}
