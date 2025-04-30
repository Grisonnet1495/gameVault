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

namespace gameVaultProject
{
    /// <summary>
    /// Logique d'interaction pour EditGameUserControl.xaml
    /// </summary>
    public partial class EditGameUserControl : UserControl
    {
        public Game Game { get; set; }

        public event EventHandler ExitEditButtonClicked;
        public event EventHandler DeleteGameButtonClicked;


        // Calculated property
        public bool HasValidStoreUrl => !string.IsNullOrWhiteSpace(Game.StoreUrl) && Uri.IsWellFormedUriString(Game.StoreUrl, UriKind.Absolute);

        public EditGameUserControl(Game game)
        {
            InitializeComponent();

            Game = game;
            DataContext = Game;
        }

        private void ExitEditButton_Click(object sender, RoutedEventArgs e)
        {
            ExitEditButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void DeleteGameButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "Are you sure you want to delete this game ?",
                "Delete game",
                MessageBoxButton.OKCancel
);

            if (result == MessageBoxResult.OK)
            {
                DeleteGameButtonClicked?.Invoke(this, EventArgs.Empty);
            }
        }

        private void OpenCalendarButton_Click(object sender, RoutedEventArgs e)
        {
            // Rendre le DatePicker visible à la position du bouton
            releaseDatePicker.Visibility = Visibility.Visible;
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (releaseDatePicker.SelectedDate.HasValue)
            {
                var selectedDate = releaseDatePicker.SelectedDate.Value;

                Game.ReleaseDate = selectedDate;
            }
        }
    }
}
