using Microsoft.Win32;
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
    public partial class MainWindow : Window
    {
        double[] speeds = new double[9];
        public double[] Speeds { get { return speeds; } }
        MediaPlayer player;
        VideoDrawing videoDrawing;
        DispatcherTimer timer = new();

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
        }

        private void SpeedCBInit()
        {
            for (int i = 0; i < 8; i++) speeds[i] = 0.5 + 0.25 * (8 - i);
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new()
            {
                Filter= "Media files (*.mp3;*.mpg;*.mpeg)|*.mp3;*.mpg;*.mpeg|All files (*.*)|*.*"
            };
            if (dialog.ShowDialog() == true)
            {
                //!!!!
                OpenMediaFile(new Uri(dialog.FileName, UriKind.Absolute));
            }
        }
    }
}