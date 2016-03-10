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

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace HW3 {
  /// <summary>
  /// 可用于自身或导航至 Frame 内部的空白页。
  /// </summary>
  public sealed partial class MainPage : Page {
    public MainPage() {
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
    }

    private void AddTodoButton_Click(object sender, RoutedEventArgs e) {
      Frame.Navigate(typeof(AddTodoPage), "");
    }

    private void TodoCheckBox_Checked(object sender, RoutedEventArgs e) {
      CheckBox cb = sender as CheckBox;
      if (cb.Name == "TodoCheckBox1") {
        TodoCheckLine1.Visibility = Visibility.Visible;
      } else if (cb.Name == "TodoCheckBox2") {
        TodoCheckLine2.Visibility = Visibility.Visible;
      }
    }

    private void TodoCheckBox_Unchecked(object sender, RoutedEventArgs e) {
      CheckBox cb = sender as CheckBox;
      if (cb.Name == "TodoCheckBox1") {
        TodoCheckLine1.Visibility = Visibility.Collapsed;
      } else if (cb.Name == "TodoCheckBox2") {
        TodoCheckLine2.Visibility = Visibility.Collapsed;
      }
    }
  }
}
