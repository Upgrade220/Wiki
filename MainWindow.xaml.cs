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
using System.Windows.Shapes;
using Wiki.ArticleLogic;

namespace Wiki
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            var articles = new List<Article>();

            for (int i = 1; i < 4; i++)
                articles.Add(Article.CreateArticle("Article" + i + ".txt"));

            InitializeComponent();

            layoutGrid.Background = Brushes.White;
            layoutGrid.ShowGridLines = true;

            var leftCol = new ColumnDefinition();
            leftCol.Width = new GridLength(300, GridUnitType.Pixel);
            var rightCol = new ColumnDefinition();
            rightCol.Width = new GridLength(1,GridUnitType.Star);

            layoutGrid.ColumnDefinitions.Add(leftCol);
            layoutGrid.ColumnDefinitions.Add(rightCol);

            var leftScroll = new ScrollViewer();
            leftScroll.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            leftScroll.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
            Grid.SetColumn(leftScroll, 0);
            Grid.SetRow(leftScroll, 0);

            var leftStackPanel = new StackPanel();
            leftStackPanel.HorizontalAlignment = HorizontalAlignment.Left;
            leftStackPanel.VerticalAlignment = VerticalAlignment.Top;
            Grid.SetColumn(leftStackPanel, 0);
            Grid.SetRow(leftStackPanel, 0);

            var searchTextBox = new TextBox();
            searchTextBox.Margin = new Thickness(0);
            searchTextBox.Width = 300;
            searchTextBox.FontSize = 18;
            searchTextBox.HorizontalAlignment = HorizontalAlignment.Stretch;
            searchTextBox.Text = "Search";
            searchTextBox.Foreground = Brushes.Gray;
            searchTextBox.GotKeyboardFocus += new KeyboardFocusChangedEventHandler(tb_GotKeyboardFocus);
            searchTextBox.LostKeyboardFocus += new KeyboardFocusChangedEventHandler(tb_LostKeyboardFocus);

            var listElement = new DataTemplate();
            listElement.DataType = typeof(Article);
            FrameworkElementFactory spFactory = new FrameworkElementFactory(typeof(StackPanel));
            spFactory.Name = "myComboFactory";
            spFactory.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);
            FrameworkElementFactory Title = new FrameworkElementFactory(typeof(TextBlock));
            Title.SetBinding(TextBlock.TextProperty, new Binding(""));
            spFactory.AppendChild(Title);
            listElement.VisualTree = spFactory;
            
            var listBox = new ListBox();
            listBox.HorizontalAlignment = HorizontalAlignment.Stretch;
            listBox.Margin = new Thickness(0);
            listBox.ItemsSource = articles;
            listBox.ItemTemplate = listElement;
            listBox.SelectionMode = SelectionMode.Single;

            leftStackPanel.Children.Add(searchTextBox);
            leftScroll.Content = listBox;
            leftStackPanel.Children.Add(leftScroll);

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

            rightStackPanel.Children.Add(titleTextBlock);
            rightStackPanel.Children.Add(contentTextBlock);

            rightScroll.Content = rightStackPanel;

            layoutGrid.Children.Add(leftStackPanel);
            layoutGrid.Children.Add(rightScroll);
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
