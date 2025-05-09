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
        public Game currentGame { get; set; }

        public event EventHandler ExitEditButtonClicked;
        public event EventHandler DeleteGameButtonClicked;


        // Calculated property
        public bool HasValidStoreUrl => !string.IsNullOrWhiteSpace(currentGame.StoreUrl) && Uri.IsWellFormedUriString(currentGame.StoreUrl, UriKind.Absolute);

        public EditGameUserControl(Game game)
        {
            InitializeComponent();

            currentGame = game;
            DataContext = currentGame;
        }

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
            // Note : To do
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
                }
                else
                {
                    MessageBox.Show("Invalid file selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (releaseDatePicker.SelectedDate.HasValue)
            {
                var selectedDate = releaseDatePicker.SelectedDate.Value;

                currentGame.ReleaseDate = selectedDate;
            }
        }
    }
}
