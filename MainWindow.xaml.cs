using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Wiki.ArticleLogic;
using static System.String;

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
            var changes = new List<Change>();

            var dataFile = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "Data.txt");
            var data = dataFile.ReadToEnd().Split('c');
            dataFile.Close();
            var articleIndex = data[0].Split(',');
            var changesIndex = data[1].Split(',').Where(x => !IsNullOrWhiteSpace(x));

            foreach (var index in articleIndex)
                articles.Add(Article.ReadArticle("Article" + index + ".txt", int.Parse(index))); 
            articles.Sort((a1, a2) => Compare(a1.Header, a2.Header, StringComparison.Ordinal));

            foreach (var index in changesIndex)
                changes.Add(Change.ReadChange("Change" + index + ".txt", int.Parse(index)));
            changes.Sort((a1, a2) => Compare(a1.Header, a2.Header, StringComparison.Ordinal));

            InitializeComponent();

            layoutGrid.Background = Brushes.White;
            layoutGrid.ShowGridLines = true;

            var leftCol = new ColumnDefinition {Width = new GridLength(300, GridUnitType.Pixel)};
            var rightCol = new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)};

            layoutGrid.ColumnDefinitions.Add(leftCol);
            layoutGrid.ColumnDefinitions.Add(rightCol);
            
            #region Левая часть

            var leftScroll = new ScrollViewer
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
                Height = IsAdmin ? 960 : 1020
            };
            Grid.SetColumn(leftScroll, 0);
            Grid.SetRow(leftScroll, 0);

            var leftStackPanel = new StackPanel
            {
                HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top
            };
            Grid.SetColumn(leftStackPanel, 0);
            Grid.SetRow(leftStackPanel, 0);

            var listElement = new DataTemplate {DataType = typeof(Article)};
            var spFactory = new FrameworkElementFactory(typeof(StackPanel)) {Name = "myComboFactory"};
            spFactory.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);
            var title = new FrameworkElementFactory(typeof(TextBlock));
            title.SetBinding(TextBlock.TextProperty, new Binding(""));
            title.SetValue(TextBlock.FontSizeProperty, 20.0);
            title.SetValue(TextBlock.TextWrappingProperty, TextWrapping.Wrap);
            title.SetValue(TextBlock.WidthProperty, 280.0);
            spFactory.AppendChild(title);
            listElement.VisualTree = spFactory;

            var listBox = new ListBox
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(0), Width = 300,
                ItemsSource = articles, ItemTemplate = listElement,
                SelectionMode = SelectionMode.Single
            };

            var articlesButton = new Button
            {
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0),
                Height = 30, Width = 300,
                Content = "Конспекты", FontSize = 15
            };
            articlesButton.Click += (v, e) =>
            {
                listBox.ItemsSource = articles;
                InvalidateVisual();
            };

            var changesButton = new Button
            {
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0),
                Height = 30, Width = 300,
                Content = "Правки", FontSize = 15
            };
            changesButton.Click += (v, e) =>
            {
                listBox.ItemsSource = changes;
                InvalidateVisual();
            };

            leftScroll.Content = listBox;
            leftStackPanel.Children.Add(leftScroll);
            if (IsAdmin)
            {
                leftStackPanel.Children.Add(articlesButton);
                leftStackPanel.Children.Add(changesButton);
            }

            #endregion

            #region Правая часть

            var rightStackPanel = new StackPanel
            {
                HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top
            };
            Grid.SetColumn(rightStackPanel, 1);
            Grid.SetRow(rightStackPanel, 0);

            var titleTextBlock = new TextBlock
            {
                FontSize = 30, Margin = new Thickness(0),
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Center,
                Text = "Выберите конспект", TextWrapping = TextWrapping.Wrap
            };

            var rightScroll = new ScrollViewer
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
                Height = 945, Margin = new Thickness(0)
            };
            Grid.SetColumn(rightScroll, 1);
            Grid.SetRow(rightScroll, 0);

            var contentTextBlock = new TextBlock
            {
                Margin = new Thickness(10, 10, 10, 0),
                FontSize = 15, FontWeight = FontWeights.Normal,
                HorizontalAlignment = HorizontalAlignment.Center,
                TextWrapping = TextWrapping.Wrap
            };

            var contentTextBox = new TextBox
            {
                Margin = new Thickness(10, 10, 10, 0),
                FontSize = 15, Width = 1620,
                HorizontalAlignment = HorizontalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                TextWrapping = TextWrapping.Wrap, AcceptsReturn = true
            };

            var changingButton = new Button
            {
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0),
                Height = 40, Width = 1620, Content = "Предложить правку",
                FontSize = 18, IsEnabled = false
            };

            var sendChangesButton = new Button
            {
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0),
                Height = 40, Width = 1620, Content = "Отправить правку",
                FontSize = 18, IsEnabled = false
            };


            var acceptChangesButton = new Button
            {
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0),
                Height = 40, Width = 1620, Content = "Принять правку",
                IsEnabled = false, FontSize = 18
            };

            rightStackPanel.Children.Add(titleTextBlock);
            rightScroll.Content = contentTextBlock;
            rightStackPanel.Children.Add(rightScroll);
            rightStackPanel.Children.Add(listBox.ItemsSource == articles ? changingButton : acceptChangesButton);

            #endregion

            listBox.SelectionChanged += (v, e) =>
            {
                rightScroll.Content = contentTextBlock;
                rightStackPanel.Children.Clear();
                rightStackPanel.Children.Add(titleTextBlock);
                rightStackPanel.Children.Add(rightScroll);
                changingButton.IsEnabled = false;
                sendChangesButton.IsEnabled = false;
                acceptChangesButton.IsEnabled = false;
                if (listBox.SelectedIndex != -1)
                {
                    if (listBox.ItemsSource == articles)
                    {
                        titleTextBlock.Text = articles.ElementAt(listBox.SelectedIndex).Header;
                        contentTextBlock.Text = articles.ElementAt(listBox.SelectedIndex).Content;
                        changingButton.IsEnabled = true;
                        rightStackPanel.Children.Add(changingButton);
                    }
                    else
                    {
                        titleTextBlock.Text = changes.ElementAt(listBox.SelectedIndex).Header;
                        contentTextBlock.Text = changes.ElementAt(listBox.SelectedIndex).Content;
                        acceptChangesButton.IsEnabled = true;
                        rightStackPanel.Children.Add(acceptChangesButton);
                    }
                }
                InvalidateVisual();
            };

            changingButton.Click += (v, e) =>
            {
                contentTextBox.Text = articles.ElementAt(listBox.SelectedIndex).Content;
                rightScroll.Content = contentTextBox;
                rightStackPanel.Children.Clear();
                rightStackPanel.Children.Add(titleTextBlock);
                rightStackPanel.Children.Add(rightScroll);
                rightStackPanel.Children.Add(sendChangesButton);
                InvalidateVisual();
            };

            contentTextBox.GotKeyboardFocus += (v, e) =>
            {
                sendChangesButton.IsEnabled = true;
                InvalidateVisual();
            };

            sendChangesButton.Click += (v, e) =>
            {
                var warning = MessageBox.Show("Вы точно хотите предложить правку?", "Подтверждение",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                switch (warning)
                {
                    case MessageBoxResult.Yes:
                        rightScroll.Content = contentTextBlock;
                        rightStackPanel.Children.Clear();
                        rightStackPanel.Children.Add(titleTextBlock);
                        rightStackPanel.Children.Add(rightScroll);
                        rightStackPanel.Children.Add(changingButton);
                        InvalidateVisual();
                        var article = listBox.SelectedItem as Article;
                        Change.CreateChange(
                            titleTextBlock.Text + "'" + article.Index + "'" +
                            contentTextBox.Text,
                            changes.Count + 1);
                        changes.Add(new Change(article.Index,changes.Count + 1,titleTextBlock.Text, contentTextBox.Text));
                        changes.Sort((a1, a2) => Compare(a1.Header, a2.Header, StringComparison.Ordinal));
                        break;
                    case MessageBoxResult.No:
                        break;
                }
            };

            acceptChangesButton.Click += (v, e) =>
            {
                var warning = MessageBox.Show("Вы точно хотите принять правку?", "Подтверждение",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                switch (warning)
                {
                    case MessageBoxResult.Yes:
                        var change = listBox.SelectedItem as Change;
                        Change.AcceptChange(change.Index);
                        var changedArtcle = articles.Find(x => x.Index == (change.Index - 1));
                        changedArtcle.Header = titleTextBlock.Text;
                        changedArtcle.Content = contentTextBlock.Text;
                        changes.RemoveAt(changes.FindIndex(a => a.Header.StartsWith(titleTextBlock.Text)));
                        titleTextBlock.Text = "Выберите правку";
                        contentTextBlock.Text = "";
                        contentTextBox.Text = "";
                        break;
                    case MessageBoxResult.No:
                        break;
                }
                rightStackPanel.Children.Clear();
                titleTextBlock.Text = "Выберите правку";
                contentTextBlock.Text = "";
                contentTextBox.Text = "";
                rightStackPanel.Children.Add(titleTextBlock);
                rightStackPanel.Children.Add(rightScroll);
                rightStackPanel.Children.Add(acceptChangesButton);
                InvalidateVisual();
            };

            articlesButton.Click += (v, e) =>
            {
                rightStackPanel.Children.Clear();
                titleTextBlock.Text = "Выберите конспект";
                contentTextBlock.Text = "";
                contentTextBox.Text = "";
                rightStackPanel.Children.Add(titleTextBlock);
                rightStackPanel.Children.Add(rightScroll);
                rightStackPanel.Children.Add(changingButton);
                InvalidateVisual();
            };
            changesButton.Click += (v, e) =>
            {
                rightStackPanel.Children.Clear();
                titleTextBlock.Text = "Выберите правку";
                contentTextBlock.Text = "";
                contentTextBox.Text = "";
                rightStackPanel.Children.Add(titleTextBlock);
                rightStackPanel.Children.Add(rightScroll);
                rightStackPanel.Children.Add(acceptChangesButton);
                InvalidateVisual();
            };

            layoutGrid.Children.Add(leftStackPanel);
            layoutGrid.Children.Add(rightStackPanel);
        }
    }
}
