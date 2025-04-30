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
    /// Logique d'interaction pour GameUserControl.xaml
    /// </summary>
    public partial class GameUserControl : UserControl
    {
        public Game Game {  get; set; }

        public event EventHandler LaunchGameButtonClicked;
        public event EventHandler EditGameButtonClicked;
        public event EventHandler ExportGameButtonClicked;
        public event EventHandler DeleteGameButtonClicked;

        // Calculated property
        public bool HasValidStoreUrl => !string.IsNullOrWhiteSpace(Game.StoreUrl) && Uri.IsWellFormedUriString(Game.StoreUrl, UriKind.Absolute);

        public GameUserControl(Game game)
        {
            InitializeComponent();

            Game = game;
            DataContext = Game;
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

        private void LaunchGameButton_Click(object sender, RoutedEventArgs e)
        {
            LaunchGameButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void EditGameButton_Click(object sender, RoutedEventArgs e)
        {
            EditGameButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void ExportGameButton_Click(object sender, RoutedEventArgs e)
        {
            ExportGameButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void DeleteGamesButton_Click(object sender, RoutedEventArgs e)
        {
            DeleteGameButtonClicked?.Invoke(this, EventArgs.Empty);
        }

    }
}
