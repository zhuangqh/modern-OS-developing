using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace HW4 {
  /// <summary>
  /// 可用于自身或导航至 Frame 内部的空白页。
  /// </summary>
  public sealed partial class MainPage : Page {
    public MainPage() {
      this.InitializeComponent();
    }

    private void AddAppBarButton_Click(object sender, RoutedEventArgs e) {
      Frame.Navigate(typeof(NewPage), "");
    }

    protected override void OnNavigatedTo(NavigationEventArgs e) {
      if (e.NavigationMode == NavigationMode.New) {
        // If this is a new navigation, this is a fresh launch so we can
        // discard any saved state
        ApplicationData.Current.LocalSettings.Values.Remove("TheWorkInProgress");
      } else {
        // Try to restore state if any, in case we were terminated
        if (ApplicationData.Current.LocalSettings.Values.ContainsKey("TheWorkInProgress")) {
          var composite = ApplicationData.Current.LocalSettings.Values["TheWorkInProgress"] as ApplicationDataCompositeValue;
          CheckBox1.IsChecked = (bool?)composite["CheckStatus1"];
          CheckBox2.IsChecked = (bool?)composite["CheckStatus2"];
          // We're done with it, so remove it
          ApplicationData.Current.LocalSettings.Values.Remove("TheWorkInProgress");
        }
      }
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e) {
      bool suspending = ((App)App.Current).IsSuspending;
      if (suspending) {
        // Save volatile state in case we get terminated later on, then
        // we can restore as if we'd never been gone :)
        var composite = new ApplicationDataCompositeValue();
        composite["CheckStatus1"] = CheckBox1.IsChecked;
        composite["CheckStatus2"] = CheckBox2.IsChecked;

        ApplicationData.Current.LocalSettings.Values["TheWorkInProgress"] = composite;
      }
    }
  }
}
