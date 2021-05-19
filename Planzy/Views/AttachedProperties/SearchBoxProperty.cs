

using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Planzy
{
    public class SearchBoxProperty
    {
        public static readonly DependencyProperty MonitorStringProperty =
            DependencyProperty.RegisterAttached("MonitorString", typeof(bool), typeof(SearchBoxProperty), new PropertyMetadata(false, OnMonitorStringChanged));

        private static void OnMonitorStringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var searchBox = d as TextBox;

            if (searchBox == null) return;
            searchBox.TextChanged -= SearchBox_TextChanged;

            if ((bool)e.NewValue)
            {
                SetHasText(searchBox);
                searchBox.TextChanged += SearchBox_TextChanged;
            }
        }
        private static void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetHasText((TextBox)sender);
            
            TextBox textBox = sender as TextBox;
            string specialChar = @"\|!#$%&/()=?»«@£§€{}.-;'<>_,";
            foreach (var item in specialChar)
            {
                if (textBox.Text.Contains(item))
                {
                    textBox.Background = Brushes.Red;
                    return;
                }
            }
            textBox.Background = Brushes.White;
        }
        public static void SetMonitorString(TextBox element, bool value)
        {
            element.SetValue(MonitorStringProperty, value);
        }
        public static bool GetMonitorString(TextBox element)
        {
            return (bool)element.GetValue(MonitorStringProperty);
        }
        public static readonly DependencyProperty HasTextProperty =
             DependencyProperty.RegisterAttached("HasText", typeof(bool), typeof(SearchBoxProperty), new PropertyMetadata(false));
        private static void SetHasText(TextBox element)
        {
            
            element.SetValue(HasTextProperty, element.Text.Length > 0);
        }
        public static bool GetHasText(TextBox element)
        {
            return (bool)element.GetValue(HasTextProperty);
        }

    }
}
