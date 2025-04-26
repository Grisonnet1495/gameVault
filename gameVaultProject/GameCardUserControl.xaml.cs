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
using gameVaultClassLibrary;

namespace gameVaultProject
{
    /// <summary>
    /// Logique d'interaction pour GameCardUserControl.xaml
    /// </summary>
    public partial class GameCardUserControl : UserControl
    {
        public GameCardUserControl(String gameTitle, String gameImageFilename)
        {
            InitializeComponent();

            if (gameTitle == null) TitleTextBlock.Text = "Unkwown game";
            else TitleTextBlock.Text = gameTitle;

            if (gameImageFilename == null) GameImage.Source = new BitmapImage(new Uri("/Ressources/Images/default_game_image.png", UriKind.RelativeOrAbsolute));
            else GameImage.Source = new BitmapImage(new Uri(gameImageFilename, UriKind.RelativeOrAbsolute));

            this.MouseLeftButtonUp += (s, e) =>
            {
                // Note : Temporary
                MessageBox.Show($"Tu as cliqué sur {gameTitle} !");
            };
        }
    }
}
