using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using ATL;
using ATL.AudioData;
using Microsoft.Win32;
using static netExmaMP.QueueViewer;

namespace netExmaMP
{

    public partial class QueueViewer: UserControl
    {
        public bool? isLooped = false;

        ObservableCollection<Track> album = [];
        ObservableCollection<Track> queue = [];
         
        public ObservableCollection<Track> Album { get { return album; } }
        public ObservableCollection<Track> Queue { get { return queue; } }

        private object currentSelection;
        private static readonly Random rng = new Random();

        public QueueViewer()
        {
            this.DataContext = this;

            LoadSettings();
            InitializeComponent();

            QueueTbtn.IsChecked = true;
            AlbumTBtn.IsEnabled = false;
        }

        public void LoadSettings()
        {
            this.Resources["BackgroundColor"] = new SolidColorBrush(Properties.Settings.Default.BgColor);
            this.Resources["ForegroundColor"] = new SolidColorBrush(Properties.Settings.Default.FgColor);
            this.Resources["TextColor"] = new SolidColorBrush(Properties.Settings.Default.TextColor);
            this.Resources["SubColor"] = new SolidColorBrush((Properties.Settings.Default.BgColor.B & Properties.Settings.Default.BgColor.G & Properties.Settings.Default.BgColor.R) < 128 ?
                Color.FromArgb(Properties.Settings.Default.BgColor.A, (byte)(Properties.Settings.Default.BgColor.R + 8), (byte)(Properties.Settings.Default.BgColor.G + 8), (byte)(Properties.Settings.Default.BgColor.B + 8)) :
                Color.FromArgb(Properties.Settings.Default.BgColor.A, (byte)(Properties.Settings.Default.BgColor.R - 8), (byte)(Properties.Settings.Default.BgColor.G - 8), (byte)(Properties.Settings.Default.BgColor.B - 8)));
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new()
            {
                Filter = "Media files (*.mp3;*.mpg;*.mpeg)|*.mp3;*.mpg;*.mpeg|All files (*.*)|*.*",
                Multiselect = true
            };
            if (dialog.ShowDialog() == true)
            {
                AddTracks(dialog.FileNames);
            }
        }

        public void AddTrack(string path)
        {
            Track track = new(path);
            if (Queue.Count > 0)
            {
                Queue.Add(track);
                if ((Window.GetWindow(this) as MainWindow).PlayPauseTBtn.IsChecked == false)
                {
                    Viewer.SelectedItem = track;
                    (Window.GetWindow(this) as MainWindow).OpenMediaFile(new(track.Path));
                }
                return;
            }
            if (track.Album != "")
            {
                Album.Add(track);
                foreach (string p in Directory.EnumerateFiles(new FileInfo(path).DirectoryName).Where(file => file.ToLower().EndsWith(".mp3") || file.ToLower().EndsWith(".mpg") || file.ToLower().EndsWith(".mpeg")))
                {
                    Track t = new(p);
                    if(t.Album == track.Album && t.Title!=track.Title) Album.Add(t);
                }
                if (Album.Count > 1)
                {
                    AlbumTBtn.IsEnabled = true;
                    AlbumTBtn.IsChecked = true;
                    Viewer.SelectedItem = track;
                    (Window.GetWindow(this) as MainWindow).OpenMediaFile(new(track.Path));
                    return;
                }
                Album.Clear();
            }
            Queue.Add(track);
            Viewer.SelectedItem = track;
            (Window.GetWindow(this) as MainWindow).OpenMediaFile(new(track.Path));
        }

        public void AddTracks(string[] paths)
        {
            foreach (string path in paths) AddTrack(path);
        }

        private void AlbumTBtn_Checked(object sender, RoutedEventArgs e)
        {
            if(QueueTbtn.IsChecked == true) {QueueTbtn.IsChecked = false;}
            Viewer.ItemsSource = Album;
        }

        private void QueueTbtn_Checked(object sender, RoutedEventArgs e)
        {
            if(AlbumTBtn.IsChecked == true) {AlbumTBtn.IsChecked = false;}
            Viewer.ItemsSource = Queue;
        }

        private void AlbumTBtn_Unchecked(object sender, RoutedEventArgs e)
        {
            if(QueueTbtn.IsChecked!=true) AlbumTBtn.IsChecked = true;
        }

        private void QueueTbtn_Unchecked(object sender, RoutedEventArgs e)
        {
            if(AlbumTBtn.IsChecked!=true) QueueTbtn.IsChecked = true;
        }

        private void Viewer_Drop(object sender, DragEventArgs e)
        {
            ListBox parent = sender as ListBox;
            Track data = e.Data.GetData(typeof(Track)) as Track;
            Track objectToPlaceBefore = GetObjectDataFromPoint(parent, e.GetPosition(parent)) as Track;
            var Items = QueueTbtn.IsChecked == true ? Queue : Album;
            if (data != null && objectToPlaceBefore != null)
            {
                int index = Items.IndexOf(objectToPlaceBefore);
                Items.Remove(data);
                Items.Insert(index, data);
                Viewer.SelectedItem = currentSelection;
            }
        }

        private void Viewer_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        { 
            ListBox parent = sender as ListBox;
            currentSelection = parent.SelectedItem;
            Track data = GetObjectDataFromPoint(parent, e.GetPosition(parent)) as Track;
            if (data != null)
            {
                DragDrop.DoDragDrop(parent, data, DragDropEffects.Move);
            }
        }

        private static object GetObjectDataFromPoint(ListBox source, Point point)
        {
            UIElement element = source.InputHitTest(point) as UIElement;
            if (element != null)
            {
                object data = DependencyProperty.UnsetValue;
                while (data == DependencyProperty.UnsetValue)
                {
                    data = source.ItemContainerGenerator.ItemFromContainer(element);
                    if (data == DependencyProperty.UnsetValue)
                        element = VisualTreeHelper.GetParent(element) as UIElement;
                    if (element == source)
                        return null;
                }
                if (data != DependencyProperty.UnsetValue)
                    return data;
            }

            return null;
        }

        public string GetPreviouseTrack()
        {
            var Items = QueueTbtn.IsChecked == true ? Queue : Album;
            Viewer.SelectedIndex--;
            if(Viewer.SelectedIndex < 0) Viewer.SelectedIndex = 0;
            return Items[Viewer.SelectedIndex].Path;
        }

        public string GetNextTrack()
        {
            var Items = QueueTbtn.IsChecked == true ? Queue : Album;
            if (isLooped == null);
            else
            {
                if (Viewer.SelectedIndex + 1 == Items.Count) switch (isLooped)
                    {
                        case true:
                            Viewer.SelectedIndex = 0;
                            break;
                        case false:
                            return null;
                    }
                else Viewer.SelectedIndex++;
            }
            return Items[Viewer.SelectedIndex].Path;
        }

        private void Viewer_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListBox parent = sender as ListBox;
            Track data = GetObjectDataFromPoint(parent, e.GetPosition(parent)) as Track;
            parent.SelectedItem = data;
        }

        public void Shuffle()
        {
            var Items = QueueTbtn.IsChecked == true ? Queue : Album;
            if (Items.Count <= 1) return;

            List<Track> list = new(Items);

            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                (list[n], list[k]) = (list[k], list[n]);
            }

            for (int i = 0; i < list.Count; i++)
            {
                int oldIndex = Items.IndexOf(list[i]);
                if (oldIndex != i)
                {
                    Items.Move(oldIndex, i);
                }
            }
        }
    }

    public class ToTimeSpanConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return TimeSpan.FromSeconds((int)value).ToString(@"mm\:ss");
        }

        public object ConvertBack(object value, Type targetType, object parametr,  System.Globalization.CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
