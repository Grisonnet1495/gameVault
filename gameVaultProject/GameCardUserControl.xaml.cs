using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class GameCardUserControl : UserControl
    {
        private bool _isPressed = false;

        public event EventHandler<Game> GameClicked; // Handler for click

        public GameCardUserControl(Game game)
        {
            InitializeComponent();

            string title = game.Title;
            // If the title is too long
            if (title.Length > 18)
            {
                title = title.Substring(0, 15) + "...";
            }

            TitleTextBlock.Text = title;
        }

        // When the click is released
        private void UserControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is Game game)
            {
                GameClicked?.Invoke(this, game); // Invoke event with handler
            }
        }
    }
}
