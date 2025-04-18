﻿using System;
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
using ATL;

namespace netExmaMP
{
    public partial class MainWindow
    {
        double volumeData = 0.5;
        bool isDynamicPaused = false;

        public void OpenMediaFile(Uri uri)
        {
            player.Close();
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
            player.Stop();
            timer.Stop();
            string? path = QViewer.GetNextTrack();
            PlayPauseTBtn.IsChecked = false;
            if (path != null) OpenMediaFile(new(path));
        }

        private void timer_tick(object sender, EventArgs e)
        {
            TimelaneSlider.Value = player.Position.TotalSeconds;
            TimeTB.Text = player.Position.ToString(@"hh\:mm\:ss");
        }

        private void LoopTBtn_Click(object sender, RoutedEventArgs e)
        {
            QViewer.isLooped = LoopTBtn.IsChecked;
            if (LoopTBtn.IsChecked != false) NextBtn.IsEnabled = true;
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            if(!player.HasAudio) return;
            if(player.Position.TotalSeconds > 0) player.Position = TimeSpan.FromSeconds(0);
            else
            {
                string p = QViewer.GetPreviouseTrack();
                if(p != null) OpenMediaFile(new(p));
            }
        }

        private void PlayPauseTBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!player.HasAudio)
            {
                PlayPauseTBtn.IsChecked = false;
                return;
            }
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
            if(!player.HasAudio) return;
            string p = QViewer.GetNextTrack();
            if (p != null) OpenMediaFile(new(p));
        }

        private void ShuffleBtn_Click(object sender, RoutedEventArgs e)
        {
            QViewer.Shuffle();
        }

        private void VolumeTBtn_Click(object sender, RoutedEventArgs e)
        {
            switch (VolumeTBtn.IsChecked)
            {
                case true:
                    player.Volume = volumeData;
                    VolumeSlider.Value = volumeData*100;
                    break;
                case false:
                    volumeData = player.Volume;
                    VolumeSlider.Value = 0;
                    player.Volume = 0;
                    break;
            }
            VolumePopup.IsOpen = true;
        }

        private void VolumeTBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            VolumePopup.IsOpen = true;
            if(VolumeSlider.Value != 0) volumeData = VolumeSlider.Value/100;
        }

        private void VolumeTBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            Thread.Sleep(50);
            if(!(VolumePopup.IsMouseOver || VolumeTBtn.IsMouseOver)) VolumePopup.IsOpen = false;
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            player.Volume = VolumeSlider.Value / 100;
            if(VolumeSlider.Value == 0) VolumeTBtn.IsChecked = false;
            else VolumeTBtn.IsChecked = true;
        }

        private void TimeTB_TextInput(object sender, InputEventArgs e)
        {
            try
            {
                player.Position = TimeSpan.Parse(TimeTB.Text);
                TimelaneSlider.Value = player.Position.TotalSeconds;
            }
            catch (Exception ex)
            {
                TimeTB.Text = player.Position.ToString(@"hh\:mm\:ss");
            }
        }

        private void TimelaneSlider_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            if (PlayPauseTBtn.IsChecked == true)
            {
                Pause();
                PlayPauseTBtn.IsChecked = false;
                isDynamicPaused = true;
            }
            else isDynamicPaused = false;
        }

        private void TimelaneSlider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            player.Position=TimeSpan.FromSeconds(TimelaneSlider.Value);
            TimeTB.Text=player.Position.ToString(@"hh\:mm\:ss");
            if (isDynamicPaused)
            {
                Play();
                PlayPauseTBtn.IsChecked = true;
            }
        }

        private void SpeedCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            player.SpeedRatio = (double)SpeedCB.SelectedValue;
        }
    }
}
