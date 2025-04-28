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

namespace gameVaultProject
{
    /// <summary>
    /// Logique d'interaction pour GameUserControl.xaml
    /// </summary>
    public partial class GameUserControl : UserControl
    {
        public GameUserControl()
        {
            InitializeComponent();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
            e.Handled = true;
        }

        private void DeleteGamesButton_Click(object sender, RoutedEventArgs e)
        {
            // Delete current game
        }

        private void ExportGameButton_Click(object sender, RoutedEventArgs e)
        {
            // Export current game
        }
    }
}
