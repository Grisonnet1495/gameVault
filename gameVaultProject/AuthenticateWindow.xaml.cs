using gameVaultClassLibrary;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace gameVaultProject
{
    public partial class AuthenticateWindow : Window
    {
        public string? Pseudo { get; set; }
        public string? Password { get; set; }

        Authenticator authenticator = new Authenticator();

        public AuthenticateWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = null;
            this.Close();
        }

        private void ThemeToggle_Checked(object sender, RoutedEventArgs e)
        {
            FadeBackground("pack://application:,,,/Ressources/Images/dark_login_image.png");

            Application.Current.Resources["MinimizeButtonBrush"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F95A38"));
            Application.Current.Resources["CloseButtonBrush"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A0510B"));
            Application.Current.Resources["LoginButtonBrush"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF6400"));
            Application.Current.Resources["CreateButtonBrush"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF6400"));
            Application.Current.Resources["TitleBrush"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A0510B"));

            ThemeToggleButton.Content = "Dark Mode";
        }

        private void ThemeToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            FadeBackground("pack://application:,,,/Ressources/Images/clear_login_image.png");

            Application.Current.Resources["MinimizeButtonBrush"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7B4FFF"));
            Application.Current.Resources["CloseButtonBrush"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0061FF"));
            Application.Current.Resources["LoginButtonBrush"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7B4FFF"));
            Application.Current.Resources["CreateButtonBrush"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7B4FFF"));
            Application.Current.Resources["TitleBrush"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0061FF"));

            ThemeToggleButton.Content = "Light Mode";
        }

        private void FadeBackground(string imagePath)
        {
            var fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(300));
            fadeOut.Completed += (s, e) =>
            {
                BackgroundImage.Source = new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute));
                var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(300));
                BackgroundImage.BeginAnimation(UIElement.OpacityProperty, fadeIn);
            };
            BackgroundImage.BeginAnimation(UIElement.OpacityProperty, fadeOut);
        }

        private bool FieldValidation(out string pseudo, out string password)
        {
            pseudo = UsernameTextBox.Text;
            password = PasswordPasswordBox.Password;

            if (string.IsNullOrWhiteSpace(pseudo) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Pseudo and password cannot be empty", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (!FieldValidation(out string pseudo, out string password)) return;

            if (authenticator.AuthenticateUser(pseudo, password))
            {
                Pseudo = pseudo;
                Password = password;

                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("No account for \"" + pseudo + "\" pseudo", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void CreateAccountButton_Click(object sender, RoutedEventArgs e)
        {
            if (!FieldValidation(out string pseudo, out string password)) return;

            if (authenticator.AddUser(pseudo, password))
            {
                MessageBox.Show("Account created, you can now log in.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("This pseudo already exists!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
