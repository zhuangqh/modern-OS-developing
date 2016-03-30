using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace HW6.Models {
  public class BoolToVisibilityConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, string language) {
      return (bool)value ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) {
      Visibility Vis = (Visibility)value;
      return Vis == Visibility.Collapsed ? false : true;
    }
  }
}
