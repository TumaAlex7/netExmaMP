using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace netExmaMP
{
    public partial class MainWindow
    {
        bool? IsLooped = false;
        bool IsShuffled = false;

        double volumeData = 0.5;

        private void OpenMediaFile(Uri uri)
        {
            player.Open(uri);
            PlayPauseTBtn.IsChecked = true;
            TimeTB.IsReadOnly = true;
        }

        private void Play()
        {
            player.Play();
            timer.Start();
        }

        private void Pause()
        {
            player.Pause();
            timer.Stop();
        }

        private void Player_MediaOpened(object? sender, EventArgs e)
        {
            TimeTB.Text = TimeSpan.FromSeconds(0).ToString(@"hh\:mm\:ss");
            TimelaneSlider.Value = 0;
            TimelaneSlider.Maximum = player.NaturalDuration.TimeSpan.TotalSeconds;
            MaxTimeLabel.Content = player.NaturalDuration.TimeSpan.ToString(@"hh\:mm\:ss");
            Play();
        }

        private void Player_MediaEnded(object? sender, EventArgs e)
        {
            switch (IsLooped)
            {
                case true:
                    player.Position = TimeSpan.FromSeconds(0);
                    break;
            }
        }

        private void timer_tick(object sender, EventArgs e)
        {
            TimelaneSlider.Value = player.Position.TotalSeconds;
            TimeTB.Text = player.Position.ToString(@"hh\:mm\:ss");
        }

        private void LoopTBtn_Click(object sender, RoutedEventArgs e)
        {
            IsLooped = LoopTBtn.IsChecked;
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PlayPauseTBtn_Click(object sender, RoutedEventArgs e)
        {
            switch (PlayPauseTBtn.IsChecked)
            {
                case true:
                    TimeTB.IsReadOnly = true;
                    Play();
                    break;
                case false:
                    TimeTB.IsReadOnly = false;
                    Pause();
                    break;
            }
        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ShuffleBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void VolumeTBtn_Click(object sender, RoutedEventArgs e)
        {
            switch (VolumeTBtn.IsChecked)
            {
                case true:
                    player.Volume = volumeData;
                    break;
                case false:
                    volumeData = player.Volume;
                    player.Volume = 0;
                    break;
            }
        }

        private void VolumeTBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            VolumePopup.IsOpen = true;
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            player.Volume = VolumeSlider.Value / 100;
        }

        private void TimeTB_TextInput(object sender, TextChangedEventArgs e)
        {
            try
            {
                player.Position = TimeSpan.Parse(TimeTB.Text);
                
            }
            catch (Exception ex)
            {
                TimeTB.Text = player.Position.ToString(@"hh\:mm\:ss");
            }
        }

        private void TimelaneSlider_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            Pause();
        }

        private void TimelaneSlider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            player.Position=TimeSpan.FromSeconds(TimelaneSlider.Value);
            Play();
        }
    }
}
