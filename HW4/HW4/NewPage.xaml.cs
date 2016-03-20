using Newtonsoft.Json;
using System;
using System.IO;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace HW4 {
  /// <summary>
  /// 可用于自身或导航至 Frame 内部的空白页。
  /// </summary>
  public sealed partial class NewPage : Page {
    public NewPage() {
      this.InitializeComponent();
      this.EditPanel = new Models.EditPanelData();
      EditPanel.LoadData();
    }

    private Models.EditPanelData EditPanel { get; set; }

    private void Create_Item(object sender, RoutedEventArgs e) {
      Frame.Navigate(typeof(MainPage), "");
    }

    protected override void OnNavigatedTo(NavigationEventArgs e) {
      ((App)App.Current).BackRequested += SaveData_OnBackRequested;

      if (e.NavigationMode == NavigationMode.New) {
        // If this is a new navigation, this is a fresh launch so we can
        // discard any saved state
        ApplicationData.Current.LocalSettings.Values.Remove("TheWorkInProgress");
      } else {
        // Try to restore state if any, in case we were terminated
        if (ApplicationData.Current.LocalSettings.Values.ContainsKey("TheWorkInProgress")) {
          var composite = ApplicationData.Current.LocalSettings.Values["TheWorkInProgress"] as ApplicationDataCompositeValue;
          EditPanel.Title = (string)composite["Title"];
          EditPanel.Detail = (string)composite["Detail"];
          EditPanel.DueDate = (DateTimeOffset)composite["DueDate"];
          
          // We're done with it, so remove it
          ApplicationData.Current.LocalSettings.Values.Remove("TheWorkInProgress");
        }
      }
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e) {
      ((App)App.Current).BackRequested -= SaveData_OnBackRequested;

      bool suspending = ((App)App.Current).IsSuspending;
      if (suspending) {
        // Save volatile state in case we get terminated later on, then
        // we can restore as if we'd never been gone :)
        var composite = new ApplicationDataCompositeValue();
        composite["Title"] = EditPanel.Title;
        composite["Detail"] = EditPanel.Detail;
        composite["DueDate"] = EditPanel.DueDate;

        ApplicationData.Current.LocalSettings.Values["TheWorkInProgress"] = composite;
      }
    }

    private void SaveData_OnBackRequested(object sender, BackRequestedEventArgs e) {
      EditPanel.SaveData();
    }
  }
}
