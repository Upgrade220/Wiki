using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
using System.Windows.Shapes;
using Wiki.ArticleLogic;

namespace Wiki
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(bool IsAdmin)
        {
            var articles = new List<Article>();
            var changes = new List<Article>();

            for (int i = 1; i < Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "Article?.txt").Length; i++)
                articles.Add(Article.ReadArticle("Article" + i + ".txt"));
            for (int i = 1; i < Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "Changes?.txt").Length; i++)
                changes.Add(Article.ReadArticle("Changes" + i + ".txt"));

            InitializeComponent();

            layoutGrid.Background = Brushes.White;
            layoutGrid.ShowGridLines = true;

            var leftCol = new ColumnDefinition();
            leftCol.Width = new GridLength(300, GridUnitType.Pixel);
            var rightCol = new ColumnDefinition();
            rightCol.Width = new GridLength(1,GridUnitType.Star);

            layoutGrid.ColumnDefinitions.Add(leftCol);
            layoutGrid.ColumnDefinitions.Add(rightCol);

            //Левая часть

            var leftScroll = new ScrollViewer();
            leftScroll.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            leftScroll.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
            leftScroll.Height = IsAdmin ? 960 : 1020;
            Grid.SetColumn(leftScroll, 0);
            Grid.SetRow(leftScroll, 0);

            var leftStackPanel = new StackPanel();
            leftStackPanel.HorizontalAlignment = HorizontalAlignment.Left;
            leftStackPanel.VerticalAlignment = VerticalAlignment.Top;
            Grid.SetColumn(leftStackPanel, 0);
            Grid.SetRow(leftStackPanel, 0);

            //var searchTextBox = new TextBox();
            //searchTextBox.Margin = new Thickness(0);
            //searchTextBox.Width = 300;
            //searchTextBox.FontSize = 18;
            //searchTextBox.HorizontalAlignment = HorizontalAlignment.Stretch;
            //searchTextBox.Text = "Search";
            //searchTextBox.Foreground = Brushes.Gray;
            //searchTextBox.GotKeyboardFocus += new KeyboardFocusChangedEventHandler(tb_GotKeyboardFocus);
            //searchTextBox.LostKeyboardFocus += new KeyboardFocusChangedEventHandler(tb_LostKeyboardFocus);

            var listElement = new DataTemplate();
            listElement.DataType = typeof(Article);
            var spFactory = new FrameworkElementFactory(typeof(StackPanel));
            spFactory.Name = "myComboFactory";
            spFactory.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);
            var Title = new FrameworkElementFactory(typeof(TextBlock));
            Title.SetBinding(TextBlock.TextProperty, new Binding(""));
            spFactory.AppendChild(Title);
            listElement.VisualTree = spFactory;
            
            var listBox = new ListBox();
            listBox.HorizontalAlignment = HorizontalAlignment.Stretch;
            listBox.Margin = new Thickness(0);
            listBox.Width = 300;
            listBox.ItemsSource = articles;
            listBox.ItemTemplate = listElement;
            listBox.SelectionMode = SelectionMode.Single;

            var articlesButton = new Button();
            articlesButton.VerticalAlignment = VerticalAlignment.Bottom;
            articlesButton.HorizontalAlignment = HorizontalAlignment.Center;
            articlesButton.Margin = new Thickness(0);
            articlesButton.Height = 30;
            articlesButton.Width = 300;
            articlesButton.Content = "Статьи";
            articlesButton.Click += (v, e) =>
            {
                listBox.ItemsSource = articles;
                InvalidateVisual();
            };

            var changesButton = new Button();
            changesButton.VerticalAlignment = VerticalAlignment.Bottom;
            changesButton.HorizontalAlignment = HorizontalAlignment.Center;
            changesButton.Margin = new Thickness(0);
            changesButton.Height = 30;
            changesButton.Width = 300;
            changesButton.Content = "Правки";
            changesButton.Click += (v, e) =>
            {
                listBox.ItemsSource = changes;
                InvalidateVisual();
            };

            //leftStackPanel.Children.Add(searchTextBox);
            leftScroll.Content = listBox;
            leftStackPanel.Children.Add(leftScroll);
            if (IsAdmin)
            {
                leftStackPanel.Children.Add(articlesButton);
                leftStackPanel.Children.Add(changesButton);
            }

            // Правая часть

            var rightStackPanel = new StackPanel();
            rightStackPanel.HorizontalAlignment = HorizontalAlignment.Left;
            rightStackPanel.VerticalAlignment = VerticalAlignment.Top;
            Grid.SetColumn(rightStackPanel, 1);
            Grid.SetRow(rightStackPanel, 0);

            var titleTextBlock = new TextBlock();
            titleTextBlock.FontSize = 25;
            titleTextBlock.Margin = new Thickness(10,0,10,0);
            titleTextBlock.FontWeight = FontWeights.Bold;
            titleTextBlock.HorizontalAlignment = HorizontalAlignment.Stretch;

            var rightScroll = new ScrollViewer();
            rightScroll.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            rightScroll.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
            rightScroll.Height = 945;
            Grid.SetColumn(rightScroll, 1);
            Grid.SetRow(rightScroll, 0);

            var contentTextBlock = new TextBlock();
            contentTextBlock.Margin = new Thickness(10, 10, 10, 0);
            contentTextBlock.FontSize = 15;
            contentTextBlock.FontWeight = FontWeights.Normal;
            contentTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
            titleTextBlock.Text = "Выберите конспект";

            listBox.SelectionChanged += (v, e) =>
            {
                if (listBox.SelectedIndex != -1)
                {
                    titleTextBlock.Text = articles.ElementAt(listBox.SelectedIndex).Header;
                    contentTextBlock.Text = articles.ElementAt(listBox.SelectedIndex).Content;
                }
            };

            var changingButton = new Button();
            changingButton.VerticalAlignment = VerticalAlignment.Bottom;
            changingButton.HorizontalAlignment = HorizontalAlignment.Center;
            changingButton.Margin = new Thickness(0);
            changingButton.Height = 40;
            changingButton.Width = 1620;
            changingButton.Content = "Предложить правку";

            var acceptChangesButton = new Button();
            acceptChangesButton.VerticalAlignment = VerticalAlignment.Bottom;
            acceptChangesButton.HorizontalAlignment = HorizontalAlignment.Center;
            acceptChangesButton.Margin = new Thickness(0);
            acceptChangesButton.Height = 40;
            acceptChangesButton.Width = 1620;
            acceptChangesButton.Content = "Принять правку";

            rightStackPanel.Children.Add(titleTextBlock);
            rightScroll.Content = contentTextBlock;
            rightStackPanel.Children.Add(rightScroll);
            rightStackPanel.Children.Add(listBox.ItemsSource == articles ? changingButton : acceptChangesButton);

            articlesButton.Click += (v, e) =>
            {
                rightStackPanel.Children.Clear();
                rightStackPanel.Children.Add(titleTextBlock);
                rightStackPanel.Children.Add(rightScroll);
                rightStackPanel.Children.Add(changingButton);
                InvalidateVisual();
            };
            changesButton.Click += (v, e) =>
            {
                rightStackPanel.Children.Clear();
                rightStackPanel.Children.Add(titleTextBlock);
                rightStackPanel.Children.Add(rightScroll);
                rightStackPanel.Children.Add(acceptChangesButton);
                InvalidateVisual();
            };

            layoutGrid.Children.Add(leftStackPanel);
            layoutGrid.Children.Add(rightStackPanel);
        }

        private void tb_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (sender is TextBox)
            {
                //If nothing has been entered yet.
                if (((TextBox) sender).Foreground == Brushes.Gray)
                {
                    ((TextBox) sender).Text = "";
                    ((TextBox) sender).Foreground = Brushes.Black;
                }
            }
        }

        private void tb_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (sender is TextBox)
            {
                if (((TextBox)sender).Text.Trim().Equals(""))
                {
                    ((TextBox)sender).Foreground = Brushes.Gray;
                    ((TextBox)sender).Text = "Search";
                }
            }
        }
    }
}
