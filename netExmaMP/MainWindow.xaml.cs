﻿using Microsoft.Win32;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace netExmaMP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        public System.Windows.Media.Color ForeColor { get { return (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(Properties.Settings.Default.FgColor); } }
        public System.Windows.Media.Color BackColor { get { return (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(Properties.Settings.Default.BgColor); } }
            
        double[] speeds = new double[9];
        public double[] Speeds { get { return speeds; } }
        MediaPlayer player;
        VideoDrawing videoDrawing;
        DispatcherTimer timer = new();
        SettingsWindow settings;

        public MainWindow()
        {
            SpeedCBInit();
            this.DataContext = this;

            InitializeComponent();

            SpeedCB.SelectedValue = 1.0;

            player = new MediaPlayer();
            player.MediaOpened += Player_MediaOpened;
            player.MediaEnded += Player_MediaEnded;

            videoDrawing = new VideoDrawing();
            videoDrawing.Player = player;
            videoDrawing.Rect = DrawingRect.RenderedGeometry.Bounds;

            timer.Interval = TimeSpan.FromSeconds(0.5);
            timer.Tick += timer_tick;

            VolumeSlider.Value = player.Volume * 100;

            settings = new SettingsWindow();
        }

        private void SpeedCBInit()
        {
            for (int i = 0; i < 8; i++) speeds[i] = 0.5 + 0.25 * (8 - i);
        }

        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog dialog = new()
            {
                Filter = "Media files (*.mp3;*.mpg;*.mpeg)|*.mp3;*.mpg;*.mpeg|All files (*.*)|*.*"
            };
            if (dialog.ShowDialog() == true)
            {
                //!!!!
                OpenMediaFile(new Uri(dialog.FileName, UriKind.Absolute));
            }
        }

        private void Properties_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            settings.ShowDialog();
        }

        private void TogglePlayPause_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            switch (PlayPauseTBtn.IsChecked)
            {
                case true:
                    PlayPauseTBtn.IsChecked = false;
                    TimeTB.IsReadOnly = false;
                    Pause();
                    break;
                case false:
                    PlayPauseTBtn.IsChecked= true;
                    TimeTB.IsReadOnly = true;
                    Play();
                    break;
            }
        }

        private void Play_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            PlayPauseTBtn.IsChecked = true;
            TimeTB.IsReadOnly = true;
            Play();
        }

        private void Play_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = PlayPauseTBtn.IsChecked==false;
        }

        private void Pause_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            PlayPauseTBtn.IsChecked = false;
            TimeTB.IsReadOnly = false;
            Pause();
        }

        private void Pause_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = PlayPauseTBtn.IsChecked == true;
        }

        private void NextTrack_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void NextTrack_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {

        }

        private void PreviousTrack_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void PreviouseTrack_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {

        }

        private void IncreaseVolume_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            player.Volume += 0.1;
            VolumeSlider.Value = player.Volume*100;
        }

        private void DecreaseVolume_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            player.Volume -= 0.1;
            VolumeSlider.Value -= player.Volume*100;
        }

        private void MuteVolume_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (player.Volume > 0)
            {
                volumeData = player.Volume;
                player.Volume = 0;
                VolumeTBtn.IsChecked = false;
            }
            else
            {
                player.Volume = volumeData;
                VolumeTBtn.IsChecked = true;
            }
        }
    }
}