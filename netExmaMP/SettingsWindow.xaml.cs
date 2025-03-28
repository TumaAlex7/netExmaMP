using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        System.Windows.Media.Color FgColor => Properties.Settings.Default.FgColor;
        System.Windows.Media.Color BgColor => Properties.Settings.Default.BgColor;
        System.Windows.Media.Color TextColor => Properties.Settings.Default.TextColor;

        public SettingsWindow(Window owner)
        {
            this.Owner = owner;

            InitializeComponent();
            LoadSettings();

            this.Loaded += SettingsWindow_Loaded;
        }

        private void SettingsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            
            foreground.Text = string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", FgColor.A, FgColor.R, FgColor.G, FgColor.B);
            background.Text = string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", BgColor.A, BgColor.R, BgColor.G, BgColor.B);
            autotext.IsChecked = Properties.Settings.Default.AutoText;
            textcolor.Text = string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", TextColor.A, TextColor.R, TextColor.G, TextColor.B);
        }

        private void LoadSettings()
        {
            this.Background = new SolidColorBrush(BgColor);
            this.Foreground = new SolidColorBrush(TextColor);
        }

        private void Save()
        {
            try { Properties.Settings.Default.FgColor = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(foreground.Text); }
            catch { foreground.Text = string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", FgColor.A, FgColor.R, FgColor.G, FgColor.B); }

            try { Properties.Settings.Default.BgColor = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(background.Text); }
            catch { background.Text = string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", FgColor.A, FgColor.R, FgColor.G, FgColor.B); }
            if (autotext.IsChecked != true) Properties.Settings.Default.TextColor = (Properties.Settings.Default.BgColor.B & Properties.Settings.Default.BgColor.G & Properties.Settings.Default.BgColor.R) < 128 ?
                    Colors.White : Colors.Black;
            else
            try { Properties.Settings.Default.TextColor = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(textcolor.Text); }
            catch { textcolor.Text = string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", FgColor.A, FgColor.R, FgColor.G, FgColor.B); }

            Properties.Settings.Default.Save();
            this.LoadSettings();
            (Owner as MainWindow).LoadSettings();
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
