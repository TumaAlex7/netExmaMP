using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Styles
{
    /// <summary>
    /// Логика взаимодействия для SliderStyle.xaml
    /// </summary>
    public partial class SliderStyle : UserControl
    {
        public SliderStyle()
        {
            InitializeComponent();
        }
        public class SliderValueToWidthConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                double sliderValue = (double)value;
                
                double maxValue = 100;

                double trackWidth = 200; 
                                         
                double width = (sliderValue / maxValue) * trackWidth;

                return width;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        public class SliderValueToThumbPositionConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                double sliderValue = (double)value;
                
                double maxValue = 100;

                double trackWidth = 200;  
                double thumbWidth = 14; 
                
                double position = (sliderValue / maxValue) * (trackWidth - thumbWidth);

                return position;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }
    }
}
