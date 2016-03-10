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
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace HW3 {
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
    }

    private void CreateButton_Click(object sender, RoutedEventArgs e) {
      Todo todo = new Todo(TitleTextBox.Text, DetailTextBox.Text, DueDatePicker.Date.DateTime);
      todo.Check();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e) {
      Todo.ResetInfo(ref TitleTextBox, ref DetailTextBox, ref DueDatePicker);
    }

    private async void SelectPictureButton_Click(object sender, RoutedEventArgs e) {
      FileOpenPicker picker = new FileOpenPicker();
      // set format
      picker.ViewMode = PickerViewMode.Thumbnail;
      picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
      picker.FileTypeFilter.Add(".jpg");
      picker.FileTypeFilter.Add(".jpeg");
      picker.FileTypeFilter.Add(".png");

      // Open a stream for the selected file 
      StorageFile file = await picker.PickSingleFileAsync();
      // Ensure a file was selected 
      if (file != null) {
        using (IRandomAccessStream fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read)) {
          // Set the image source to the selected bitmap 
          BitmapImage bitmapImage = new BitmapImage();
          bitmapImage.DecodePixelWidth = 350; //match the target Image.Width, not shown
          await bitmapImage.SetSourceAsync(fileStream);
          TodoImage.Source = bitmapImage;
        }
      }
    }
  }

  /// <summary>
  /// 用于处理Todo项相关事务
  /// </summary>
  public class Todo {
    public string Title { get; set; }
    public string Detail { get; set; }
    public DateTime DueDate { get; set; }

    public Todo(string Title, string Detail, DateTime DueDate) {
      this.Title = Title;
      this.Detail = Detail;
      this.DueDate = DueDate;
    }

    public Todo() { }

    public void Check() {
      string MessageToNotify = string.Empty;

      if (Title.Length == 0) {
        MessageToNotify += "Title is empty!\n";
      }

      if (Detail.Length == 0) {
        MessageToNotify += "Details are empty!\n";
      }

      if (DueDate.Subtract(DateTime.Now).Days < 0) {
        MessageToNotify += "Due date should be later than now!\n";
      }

      if (MessageToNotify.Length != 0) {
        var Notify = new MessageDialog(MessageToNotify).ShowAsync();
      }
    }

    public static void ResetInfo(ref TextBox Title, ref TextBox Detail, ref DatePicker DueDate) {
      Title.Text = string.Empty;
      Detail.Text = string.Empty;
      DueDate.Date = DateTime.Now;
    }
  }
}
