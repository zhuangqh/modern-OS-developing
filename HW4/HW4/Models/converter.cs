using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace HW4.Models {
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
