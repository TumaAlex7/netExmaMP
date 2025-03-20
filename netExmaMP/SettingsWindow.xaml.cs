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
        System.Windows.Media.Color FgColor => Properties.Settings.Default.FgColor;
        System.Windows.Media.Color BgColor => Properties.Settings.Default.BgColor;

        public SettingsWindow(Window owner)
        {
            this.Owner = owner;

            InitializeComponent();

            this.Loaded += SettingsWindow_Loaded;
        }

        private void SettingsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            
            foreground.Text = string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", FgColor.A, FgColor.R, FgColor.G, FgColor.B);
            background.Text = string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", BgColor.A, BgColor.R, BgColor.G, BgColor.B);
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private void ApplyBtn_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Save();
            Close();
        }

        private void color_TextInput(object sender, TextCompositionEventArgs e)
        {
            try { Properties.Settings.Default.FgColor = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString((sender as TextBox).Text); }
            catch { (sender as TextBox).Text = string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", FgColor.A, FgColor.R, FgColor.G, FgColor.B); }
        }
    }
}
