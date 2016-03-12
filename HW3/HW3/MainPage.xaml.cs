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

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace HW3 {
  /// <summary>
  /// 可用于自身或导航至 Frame 内部的空白页。
  /// </summary>
  public sealed partial class MainPage : Page {
    public MainPage() {
      this.InitializeComponent();
      this.ViewModel = new ViewModels.TodoItemViewModel();
      this.SizeChanged += (s, e) => {
        SideGridShow = e.NewSize.Width > 800 ? true : false;
      };
     }

    public ViewModels.TodoItemViewModel ViewModel { get; set; }

    private bool SideGridShow { get; set; }  // 记录右边的编辑框显示与否

    protected override void OnNavigatedTo(NavigationEventArgs e) {
      if (e.Parameter.GetType() == typeof(ViewModels.TodoItemViewModel)) {
        this.ViewModel = e.Parameter as ViewModels.TodoItemViewModel;
        SideGrid_Set();
      }
    }

    // 设置右边编辑框的信息
    private void SideGrid_Set() {
      if (ViewModel.SelectedItem == null) {
        CreateButton.Visibility = Visibility.Visible;
        UpdateButton.Visibility = Visibility.Collapsed;
        TitleTextBox.Text = string.Empty;
        DetailTextBox.Text = string.Empty;
        DueDatePicker.Date = DateTime.Now;
      } else {
        CreateButton.Visibility = Visibility.Collapsed;
        UpdateButton.Visibility = Visibility.Visible;
        TitleTextBox.Text = ViewModel.SelectedItem.Title;
        DetailTextBox.Text = ViewModel.SelectedItem.Discription;
        DueDatePicker.Date = ViewModel.SelectedItem.DueDate;
      }
    }

    private void AddTodoButton_Click(object sender, RoutedEventArgs e) {
      Frame.Navigate(typeof(AddTodoPage), ViewModel);
    }

    private void TodoItem_ItemClicked(object sender, ItemClickEventArgs e) {
      ViewModel.SelectedItem = (e.ClickedItem as Models.TodoItem);
      if (!SideGridShow)
        Frame.Navigate(typeof(AddTodoPage), ViewModel);
      SideGrid_Set();
    }

    private void CreateButton_Click(object sender, RoutedEventArgs e) {
      Models.TodoItem TodoToCreate = new Models.TodoItem(TitleTextBox.Text, DetailTextBox.Text, DueDatePicker.Date);
      if (TodoToCreate.TodoInfoValidator()) {
        ViewModel.AddTodoItem(TodoToCreate);
      }
    }

    private void UpdateButton_Click(object sender, RoutedEventArgs e) {
      if (ViewModel.SelectedItem != null) {
        Models.TodoItem TodoToUpdate = new Models.TodoItem(TitleTextBox.Text, DetailTextBox.Text, DueDatePicker.Date);

        if (TodoToUpdate.TodoInfoValidator()) {
          ViewModel.UpdateTodoItem(ViewModel.SelectedItem.Id, TodoToUpdate);
        }
      }
    }


    private void CancelButton_Click(object sender, RoutedEventArgs e) {
      ViewModel.SelectedItem = null;
      SideGrid_Set();
    }
  }
}
