using System;
using System.Collections.Generic;
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
    /// Logique d'interaction pour HomeUserControl.xaml
    /// </summary>
    public partial class HomeUserControl : UserControl
    {
        public HomeUserControl()
        {
            InitializeComponent();

            InitializeHomePanel();
        }

        public void InitializeHomePanel()
        {
            // Get the 3 most recent games
            // Create and add a GameCardUserControl for each game
            // Create and add a GameCardUserControl to redirect to the game library
        }
    }
}
