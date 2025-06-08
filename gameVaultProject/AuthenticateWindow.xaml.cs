using gameVaultClassLibrary;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace gameVaultProject
{
    public partial class AuthenticateWindow : Window
    {
        #region Properties
        public string? Pseudo { get; set; }
        public string? Password { get; set; }
        private Authenticator authenticator = new Authenticator(); 
        #endregion

        #region Constructor
        public AuthenticateWindow()
        {
            InitializeComponent();
        } 
        #endregion

        #region Window actions methods
        // Minimize the window
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        // Close the window
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = null;
            this.Close();
        }

        // Drag the window
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
        #endregion

        #region Main window buttons click
        private void ThemeToggle_Checked(object sender, RoutedEventArgs e)
        {
            // Change to dark mode
            FadeBackground("pack://application:,,,/Ressources/Images/dark_login_image.png");

            Application.Current.Resources["MinimizeButtonBrush"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F95A38"));
            Application.Current.Resources["CloseButtonBrush"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A0510B"));
            Application.Current.Resources["LoginButtonBrush"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF6400"));
            Application.Current.Resources["CreateButtonBrush"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF6400"));
            Application.Current.Resources["TitleBrush"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A0510B"));

            ThemeToggleButton.Content = "Light Mode";
        }

        private void ThemeToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            // Change to light mode
            FadeBackground("pack://application:,,,/Ressources/Images/clear_login_image.png");

            Application.Current.Resources["MinimizeButtonBrush"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7B4FFF"));
            Application.Current.Resources["CloseButtonBrush"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0061FF"));
            Application.Current.Resources["LoginButtonBrush"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7B4FFF"));
            Application.Current.Resources["CreateButtonBrush"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7B4FFF"));
            Application.Current.Resources["TitleBrush"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0061FF"));

            ThemeToggleButton.Content = "Dark Mode";
        }

        private void FadeBackground(string imagePath)
        {
            // Make a fade animation between 2 background images
            var fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(300));
            fadeOut.Completed += (s, e) =>
            {
                BackgroundImage.Source = new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute));
                var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(300));
                BackgroundImage.BeginAnimation(UIElement.OpacityProperty, fadeIn);
            };
            BackgroundImage.BeginAnimation(UIElement.OpacityProperty, fadeOut);
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (!IsFieldValidated(out string pseudo, out string password)) return;

            // Check if the user exists
            if (!authenticator.UserExists(pseudo))
            {
                MessageBox.Show("No account for the pseudo : " + pseudo, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Try to authenticate the user
            if (authenticator.AuthenticateUser(pseudo, password))
            {
                Pseudo = pseudo;
                Password = password;

                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Incorrect password", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CreateAccountButton_Click(object sender, RoutedEventArgs e)
        {
            if (!IsFieldValidated(out string pseudo, out string password)) return;

            // Try to add the user
            if (authenticator.AddUser(pseudo, password))
            {
                MessageBox.Show("Account created, you can now log in.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("This pseudo already exists!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Other methods
        private bool IsFieldValidated(out string pseudo, out string password)
        {
            pseudo = UsernameTextBox.Text;
            password = PasswordPasswordBox.Password;

            // Verify if the field is valid
            if (string.IsNullOrWhiteSpace(pseudo) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Pseudo and password cannot be empty", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        } 
        #endregion
    }
}
