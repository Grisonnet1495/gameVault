using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using gameVaultClassLibrary;

namespace gameVaultProject
{
    public partial class GameCardUserControl : UserControl
    {
        #region Properties
        public event EventHandler<Game> GameClicked; // Handler for click
        #endregion

        #region Constructor
        public GameCardUserControl(Game game)
        {
            InitializeComponent();

            string title = game.Title;
            // If the game title is too long
            if (title.Length > 18)
            {
                title = title.Substring(0, 15) + "...";
            }

            TitleTextBlock.Text = title;

            // Set up the game image
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            if (game.ImageName != null)
            {
                string imagePath = System.IO.Path.Combine(System.IO.Path.Combine(Config.LoadSetting(Config.appDataKey), Config.LoadSetting(Config.imagesFolderKey)), game.ImageName);
                bitmap.UriSource = new Uri(imagePath, UriKind.Absolute);
            }
            else
            {
                bitmap.UriSource = new Uri("pack://application:,,,/Ressources/Images/default_game_image.png");
            }
            bitmap.CacheOption = BitmapCacheOption.OnLoad; // To release the image
            bitmap.EndInit();
            GameImage.Source = bitmap;
        }
        #endregion

        #region Methods
        // When the click is released
        private void UserControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is Game game)
            {
                GameClicked?.Invoke(this, game); // Invoke event with handler
            }
        } 
        #endregion
    }
}
