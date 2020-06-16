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
using Wiki.LoginLogic;

namespace Wiki
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {

        private string login;
        private string password;
        private string password_repeat;

        public LoginWindow()
        {
            InitializeComponent();
            DrawSignInWindow();
        }

        private void DrawSignInWindow()
        {
            TextBlock loginTextBlock = new TextBlock();
            loginTextBlock.Margin = new Thickness(100, 100, 100, 0);
            loginTextBlock.FontSize = 16;
            loginTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
            loginTextBlock.Text = "Login";

            TextBox loginTextBox = new TextBox();
            loginTextBox.Margin = new Thickness(90, 140, 90, 300);
            loginTextBox.MaxLength = 20;

            TextBlock passwordTextBlock = new TextBlock();
            passwordTextBlock.Margin = new Thickness(100, 180, 100, 0);
            passwordTextBlock.FontSize = 16;
            passwordTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
            passwordTextBlock.Text = "Password";

            PasswordBox passwordTextBox = new PasswordBox();
            passwordTextBox.Margin = new Thickness(90, 220, 90, 220);
            passwordTextBox.MaxLength = 20;
            passwordTextBox.PasswordChar = '*';

            Button signInButton = new Button();
            signInButton.Margin = new Thickness(125, 300, 125, 130);
            signInButton.Content = "Sign In";
            signInButton.Click += signInButton_click;
            signInButton.Tag = loginTextBox.Text + " " + passwordTextBox.Password;
            Button signUpButton = new Button();
            signUpButton.Margin = new Thickness(125, 350, 125, 80);
            signUpButton.Content = "Sign Up";
            signUpButton.Click += signUpButton1_click;

            layoutGrid.Children.Add(loginTextBlock);
            layoutGrid.Children.Add(loginTextBox);
            layoutGrid.Children.Add(passwordTextBlock);
            layoutGrid.Children.Add(passwordTextBox);
            layoutGrid.Children.Add(signInButton);
            layoutGrid.Children.Add(signUpButton);
        }


        private void signInButton_click(object sender, RoutedEventArgs e)
        {
            var login = ((Control) sender).Tag.ToString().Split(' ').First();
            var password = ((Control) sender).Tag.ToString().Split(' ').Last();
            if (User.LoginUser(login, password))
                MessageBox.Show("Ты уебан", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            MainWindow mainWindow = new MainWindow();
            this.Hide();
            mainWindow.ShowDialog();
            this.Close();
        }

        private void signUpButton1_click(object sender, RoutedEventArgs e)
        {
            layoutGrid.Children.Clear();
            DrawSignUpWindow();
        }

        private void DrawSignUpWindow()
        {
            TextBlock titleTextBlock = new TextBlock();
            titleTextBlock.Margin = new Thickness(10);
            titleTextBlock.FontSize = 24;
            titleTextBlock.FontWeight = FontWeights.Bold;
            titleTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
            titleTextBlock.Text = "Registration";

            TextBlock loginTextBlock = new TextBlock();
            loginTextBlock.Margin = new Thickness(100, 100, 100, 0);
            loginTextBlock.FontSize = 16;
            loginTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
            loginTextBlock.Text = "Login";

            TextBox loginTextBox = new TextBox();
            loginTextBox.Margin = new Thickness(90, 140, 90, 300);
            loginTextBox.MaxLength = 20;

            TextBlock passwordTextBlock1 = new TextBlock();
            passwordTextBlock1.Margin = new Thickness(100, 180, 100, 0);
            passwordTextBlock1.FontSize = 16;
            passwordTextBlock1.HorizontalAlignment = HorizontalAlignment.Center;
            passwordTextBlock1.Text = "Password";

            PasswordBox passwordTextBox1 = new PasswordBox();
            passwordTextBox1.Margin = new Thickness(90, 220, 90, 220);
            passwordTextBox1.MaxLength = 20;
            passwordTextBox1.PasswordChar = '*';

            TextBlock passwordTextBlock2 = new TextBlock();
            passwordTextBlock2.Margin = new Thickness(100, 260, 100, 0);
            passwordTextBlock2.FontSize = 16;
            passwordTextBlock2.HorizontalAlignment = HorizontalAlignment.Center;
            passwordTextBlock2.Text = "Repeat Password";

            PasswordBox passwordTextBox2 = new PasswordBox();
            passwordTextBox2.Margin = new Thickness(90, 300, 90, 140);
            passwordTextBox2.MaxLength = 20;
            passwordTextBox2.PasswordChar = '*';

            Button signUpButton = new Button();
            signUpButton.Margin = new Thickness(125, 350, 125, 80);
            signUpButton.Content = "Sign Up";
            signUpButton.Click += signUpButton2_click;
            signUpButton.Tag = loginTextBox.Text + " " + passwordTextBox1.Password + " " +passwordTextBox2.Password;

            layoutGrid.Children.Add(titleTextBlock);
            layoutGrid.Children.Add(loginTextBlock);
            layoutGrid.Children.Add(loginTextBox);
            layoutGrid.Children.Add(passwordTextBlock1);
            layoutGrid.Children.Add(passwordTextBox1);
            layoutGrid.Children.Add(passwordTextBlock2);
            layoutGrid.Children.Add(passwordTextBox2);
            layoutGrid.Children.Add(signUpButton);
        }

        private void signUpButton2_click(object sender, RoutedEventArgs e)
        {
            var login = ((Control)sender).Tag.ToString().Split(' ').First();
            var password = ((Control)sender).Tag.ToString().Split(' ')[1];
            var password_repeat = ((Control) sender).Tag.ToString().Split(' ').Last();

            if (User.RegisterUser(login, password, password_repeat))
            {
                layoutGrid.Children.Clear();
                DrawSignInWindow();

                TextBlock sRTextBlock = new TextBlock();
                sRTextBlock.Margin = new Thickness(10);
                sRTextBlock.FontSize = 16;
                sRTextBlock.Foreground = Brushes.Green;
                sRTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
                sRTextBlock.VerticalAlignment = VerticalAlignment.Bottom;
                sRTextBlock.Text = "Registration successful";

                layoutGrid.Children.Add(sRTextBlock);
            }
            else
            {
                TextBlock sRTextBlock = new TextBlock();
                sRTextBlock.Margin = new Thickness(10);
                sRTextBlock.FontSize = 16;
                sRTextBlock.Foreground = Brushes.Red;
                sRTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
                sRTextBlock.VerticalAlignment = VerticalAlignment.Bottom;
                sRTextBlock.Text = "Passwords didn't match or Login already exists";

                layoutGrid.Children.Add(sRTextBlock);
            }
        }
    }
}
