using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace HW2 {
  /// <summary>
  /// 可用于自身或导航至 Frame 内部的空白页。
  /// </summary>
  public sealed partial class AddTodoPage : Page {
    public AddTodoPage() {
      this.InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e) {
      Frame rootFrame = Window.Current.Content as Frame;

      if (rootFrame.CanGoBack) {
        // Show UI in title bar if opted-in and in-app backstack is not empty.
        SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
            AppViewBackButtonVisibility.Visible;
      } else {
        // Remove the UI from the title bar if in-app back stack is empty.
        SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
            AppViewBackButtonVisibility.Collapsed;
      }
      var i = new MessageDialog("Welcome!").ShowAsync();
    }
  }
}
