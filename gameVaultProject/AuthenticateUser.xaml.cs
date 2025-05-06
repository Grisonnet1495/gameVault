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
    public partial class AuthenticateUser : Window
    {
        public string? Pseudo { get; set; }
        public string? Password { get; set; }

        Authenticator authenticator = new Authenticator();

        public AuthenticateUser()
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

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void ThemeToggle_Checked(object sender, RoutedEventArgs e)
        {
            FadeBackground("pack://application:,,,/Ressources/Images/dark_login.png");
            btnMinimize.Background= new SolidColorBrush(Color.FromRgb(249, 90, 56));
            Title.Foreground = new SolidColorBrush(Color.FromRgb(160, 81, 11));
            btnClose.Background = new SolidColorBrush(Color.FromRgb(160, 81, 11));
            btnLogin.Background = new SolidColorBrush(Color.FromRgb(255, 100, 0));
            btnCreate.Background = new SolidColorBrush(Color.FromRgb(255, 100, 0));
        }

        private void ThemeToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            FadeBackground("pack://application:,,,/Ressources/Images/clear_login.png");
            btnMinimize.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7B4FFF"));
            Title.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0061FF")); ;
            btnClose.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0061FF"));
            btnLogin.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7B4FFF"));
            btnCreate.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7B4FFF"));
            ThemeToggle.Content = "Light Mode";
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
            pseudo = txtUser.Text;
            password = txtPass.Password;

            if (string.IsNullOrWhiteSpace(pseudo) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Format de la chaîne de caractères invalide, recommencez.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (!FieldValidation(out string pseudo, out string password)) return;

            if (authenticator.AuthenticateUser(pseudo, password))
            {
                this.DialogResult = true; // Close the window and return true
                this.Close();
            }
            else
            {
                MessageBox.Show("Authentification échouée, veuillez créer un compte !", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            if (!FieldValidation(out string pseudo, out string password)) return;

            if (authenticator.AddUser(pseudo, password))
            {
                MessageBox.Show("Compte créé, vous pouvez maintenant vous connecter.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Création échouée, compte déjà existant !", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
