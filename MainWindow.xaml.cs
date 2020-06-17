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

namespace Wiki
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            var articles = new List<string>() {"123", "12", "1"};

            InitializeComponent();

            layoutGrid.Background = Brushes.White;
            layoutGrid.ShowGridLines = true;

            var leftCol = new ColumnDefinition();
            leftCol.Width = new GridLength(300, GridUnitType.Pixel);
            var rightCol = new ColumnDefinition();
            rightCol.Width = new GridLength(1,GridUnitType.Star);

            layoutGrid.ColumnDefinitions.Add(leftCol);
            layoutGrid.ColumnDefinitions.Add(rightCol);

            var leftStackPanel = new StackPanel();
            leftStackPanel.HorizontalAlignment = HorizontalAlignment.Left;
            leftStackPanel.VerticalAlignment = VerticalAlignment.Top;
            Grid.SetColumn(leftStackPanel, 0);
            Grid.SetRow(leftStackPanel, 0);

            var searchTextBox = new TextBox();
            searchTextBox.Margin = new Thickness(0, 0, 0, 990);
            searchTextBox.FontSize = 18;
            searchTextBox.HorizontalAlignment = HorizontalAlignment.Stretch;
            searchTextBox.Text = "Search";
            searchTextBox.Foreground = Brushes.Gray;
            searchTextBox.GotKeyboardFocus += new KeyboardFocusChangedEventHandler(tb_GotKeyboardFocus);
            searchTextBox.LostKeyboardFocus += new KeyboardFocusChangedEventHandler(tb_LostKeyboardFocus);

            var listElement = new DataTemplate();
            listElement.DataType = typeof(string);
            FrameworkElementFactory spFactory = new FrameworkElementFactory(typeof(StackPanel));
            spFactory.Name = "myComboFactory";
            spFactory.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);
            FrameworkElementFactory cardHolder = new FrameworkElementFactory(typeof(TextBlock));
            cardHolder.SetBinding(TextBlock.TextProperty, new Binding("Title"));
            cardHolder.SetValue(TextBlock.ToolTipProperty, "Title");
            spFactory.AppendChild(cardHolder);
            listElement.VisualTree = spFactory;
            
            var listBox = new ListBox();
            listBox.HorizontalAlignment = HorizontalAlignment.Stretch;
            listBox.Margin = new Thickness(0);
            listBox.ItemsSource = articles;
            listBox.ItemTemplate = listElement;

            leftStackPanel.Children.Add(searchTextBox);
            leftStackPanel.Children.Add(listBox);

            layoutGrid.Children.Add(leftStackPanel);
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
                    ((TextBox)sender).Text = "Text";
                }
            }
        }
    }
}
