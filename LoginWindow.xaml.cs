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
        private TextBlock SRTextBlock = new TextBlock();

        public LoginWindow()
        {
            InitializeComponent();
            this.Title = "Logging";
            DrawSignInWindow();
        }

        private void DrawSignInWindow()
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(@"C:\Users\User\source\repos\Wiki\Icon.png");
            bitmap.EndInit();

            var image = new Image
            {
                Source = bitmap,
                Width = 70,
                Height = 70,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0, 20, 0, 0)
            };

            var loginTextBlock = new TextBlock
            {
                Margin = new Thickness(100, 100, 100, 0), FontSize = 16,
                HorizontalAlignment = HorizontalAlignment.Center, Text = "Login"
            };

            var loginTextBox = new TextBox {Margin = new Thickness(90, 140, 90, 300), MaxLength = 20};

            var passwordTextBlock = new TextBlock
            {
                Margin = new Thickness(100, 180, 100, 0), FontSize = 16,
                HorizontalAlignment = HorizontalAlignment.Center, Text = "Password"
            };

            var passwordTextBox = new PasswordBox
            {
                Margin = new Thickness(90, 220, 90, 220), MaxLength = 20, PasswordChar = '*'
            };

            var signInButton = new Button {Margin = new Thickness(125, 300, 125, 130), Content = "Sign In"};
            signInButton.Click += (e, v) => signInButton.Tag = loginTextBox.Text + " " + passwordTextBox.Password;
            signInButton.Click += signInButton_click;
            signInButton.IsEnabled = false;

            var signUpButton = new Button {Margin = new Thickness(125, 350, 125, 80), Content = "Sign Up"};
            signUpButton.Click += signUpButton1_click;

            SRTextBlock.Margin = new Thickness(10);
            SRTextBlock.FontSize = 16;
            SRTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
            SRTextBlock.VerticalAlignment = VerticalAlignment.Bottom;

            loginTextBox.TextChanged += (v, e) =>
            {
                signInButton.IsEnabled = passwordTextBox.Password != "" && loginTextBox.Text != "";
            };
            passwordTextBox.PasswordChanged += (v, e) =>
            {
                signInButton.IsEnabled = passwordTextBox.Password != "" && loginTextBox.Text != "";
            };

            layoutGrid.Children.Add(image);
            layoutGrid.Children.Add(loginTextBlock);
            layoutGrid.Children.Add(loginTextBox);
            layoutGrid.Children.Add(passwordTextBlock);
            layoutGrid.Children.Add(passwordTextBox);
            layoutGrid.Children.Add(signInButton);
            layoutGrid.Children.Add(signUpButton);
            layoutGrid.Children.Add(SRTextBlock);
        }


        private void signInButton_click(object sender, RoutedEventArgs e)
        {
            var login = ((Control) sender).Tag.ToString().Split(' ').First();
            var password = ((Control) sender).Tag.ToString().Split(' ').Last();
            if (User.LoginUser(login, password))
            {
                MainWindow mainWindow;
                if (User.IsAdmin(login))
                    mainWindow = new MainWindow(true);
                else
                    mainWindow = new MainWindow(false);
                this.Hide();
                mainWindow.ShowDialog();
                this.Close();
            }
            else
            {
                SRTextBlock.Foreground = Brushes.Red;
                SRTextBlock.Text = "Incorrect password or login";
            }
        }

        private void signUpButton1_click(object sender, RoutedEventArgs e)
        {
            layoutGrid.Children.Clear();
            DrawSignUpWindow();
        }

        private void DrawSignUpWindow()
        {
            this.Title = "Registration";

            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(@"C:\Users\User\source\repos\Wiki\Icon.png");
            bitmap.EndInit();

            var image = new Image
            {
                Source = bitmap,
                Width = 70,
                Height = 70,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0, 20, 0, 0)
            };

            var loginTextBlock = new TextBlock
            {
                Margin = new Thickness(100, 100, 100, 0), FontSize = 16,
                HorizontalAlignment = HorizontalAlignment.Center, Text = "Login"
            };

            var loginTextBox = new TextBox {Margin = new Thickness(90, 140, 90, 300), MaxLength = 20};

            var passwordTextBlock1 = new TextBlock
            {
                Margin = new Thickness(100, 180, 100, 0), FontSize = 16,
                HorizontalAlignment = HorizontalAlignment.Center, Text = "Password"
            };

            var passwordTextBox1 = new PasswordBox
            {
                Margin = new Thickness(90, 220, 90, 220), MaxLength = 20, PasswordChar = '*'
            };

            var passwordTextBlock2 = new TextBlock
            {
                Margin = new Thickness(100, 260, 100, 0), FontSize = 16,
                HorizontalAlignment = HorizontalAlignment.Center, Text = "Repeat Password"
            };

            var passwordTextBox2 = new PasswordBox
            {
                Margin = new Thickness(90, 300, 90, 140), MaxLength = 20, PasswordChar = '*'
            };

            var signUpButton = new Button
            {
                Margin = new Thickness(125, 350, 125, 80), Content = "Sign Up", IsEnabled = false
            };
            signUpButton.Click += (e, v) =>
                signUpButton.Tag = loginTextBox.Text + " " + passwordTextBox1.Password + " " +
                                   passwordTextBox2.Password;

            signUpButton.Click += signUpButton2_click;

            loginTextBox.TextChanged += (v, e) =>
            {
                signUpButton.IsEnabled = passwordTextBox1.Password != "" && loginTextBox.Text != "" && passwordTextBox2.Password != "";
            };
            passwordTextBox1.PasswordChanged += (v, e) =>
            {
                signUpButton.IsEnabled = passwordTextBox1.Password != "" && loginTextBox.Text != "" && passwordTextBox2.Password != "";
            };
            passwordTextBox2.PasswordChanged += (v, e) =>
            {
                signUpButton.IsEnabled = passwordTextBox1.Password != "" && loginTextBox.Text != "" && passwordTextBox2.Password != "";
            };

            layoutGrid.Children.Add(image);
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
            var passwordRepeat = ((Control) sender).Tag.ToString().Split(' ').Last();


            if (User.RegisterUser(login, password, passwordRepeat))
            {
                layoutGrid.Children.Clear();
                DrawSignInWindow();

                SRTextBlock.Foreground = Brushes.Green;
                SRTextBlock.Text = "Registration successful";
            }
            else
            {
                SRTextBlock.Foreground = Brushes.Red;
                SRTextBlock.Text = "Passwords didn't match or" + Environment.NewLine + "      Login already exists";
            }
        }
    }
}
