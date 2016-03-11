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
      this.ViewModel = new ViewModels.TodoItemViewModel();
    }
    
    public ViewModels.TodoItemViewModel ViewModel { get; set; }

    protected override void OnNavigatedTo(NavigationEventArgs e) {
      if (e.Parameter.GetType() == typeof(ViewModels.TodoItemViewModel)) {
        this.ViewModel = e.Parameter as ViewModels.TodoItemViewModel;
      }
    }

    private void AddTodoButton_Click(object sender, RoutedEventArgs e) {
      Frame.Navigate(typeof(AddTodoPage), ViewModel);
    }

    private void TodoItem_ItemClicked(object sender, ItemClickEventArgs e) {
      ViewModel.SelectedItem = (e.ClickedItem as Models.TodoItem);
      Frame.Navigate(typeof(AddTodoPage), ViewModel);
    }

    private void Complete_Checked(object sender, RoutedEventArgs e) {
      
    }
  }
}
