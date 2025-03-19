using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace netExmaMP
{
    /// <summary>
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();

            this.Loaded += SettingsWindow_Loaded;
        }

        private void SettingsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            
            foreground.Text = Properties.Settings.Default.FgColor;
            background.Text = Properties.Settings.Default.BgColor;
        }

        private void Save()
        {

            if(Regex.IsMatch(foreground.Text, "^#(?:[0-9a-fA-F]{3,4}){1,2}$")) Properties.Settings.Default.FgColor = foreground.Text;
            if(Regex.IsMatch(foreground.Text, "^#(?:[0-9a-fA-F]{3,4}){1,2}$")) Properties.Settings.Default.BgColor = background.Text;
            Properties.Settings.Default.Save();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            Save();
        }

        private void ApplyBtn_Click(object sender, RoutedEventArgs e)
        {
            Save();
            Close();
        }
    }
}
