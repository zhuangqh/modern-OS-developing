using System;
using System.Collections.Generic;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.DataTransfer.ShareTarget;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace HW5 {
  /// <summary>
  /// 可用于自身或导航至 Frame 内部的空白页。
  /// </summary>
  public sealed partial class NewPage : Page {
    public NewPage() {
      this.InitializeComponent();
      ViewModel = new ViewModels.TodoItemViewModel();
    }

    private ViewModels.TodoItemViewModel ViewModel;

    private StorageFile ImageFile;

    private ShareOperation shareOp;

    protected async override void OnNavigatedTo(NavigationEventArgs e) {
      if (e.Parameter.GetType() == typeof(ViewModels.TodoItemViewModel)) {
        ViewModel = e.Parameter as ViewModels.TodoItemViewModel;
      } else if (e.Parameter.GetType() == typeof(ShareOperation)) {
        // handle event as a sharing target
        shareOp = (e.Parameter as ShareOperation);
        if (shareOp.Data.Contains(StandardDataFormats.Text)) {
          string text = await shareOp.Data.GetTextAsync();
          DetailTextBox.Text = text;
        }
      }
      SetButton();
      DataTransferManager.GetForCurrentView().DataRequested += OnShareDataRequested;
    }

    // determine what to be shown in NewPage
    private void SetButton() {
      if (ViewModel.SelectedItem == null) {
        CommandBar.Visibility = Visibility.Collapsed;
        UpdateButton.Visibility = Visibility.Collapsed;
      } else {
        CreateButton.Visibility = Visibility.Collapsed;
        DueDatePicker.Date = ViewModel.SelectedItem.DueDate;
      }
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e) {
      DataTransferManager.GetForCurrentView().DataRequested -= OnShareDataRequested;
    }

    #region TodoItem controller
    private void CreateButton_Click(object sender, RoutedEventArgs e) {
      Models.TodoItem TodoToCreate = new Models.TodoItem(TitleTextBox.Text, DetailTextBox.Text, DueDatePicker.Date, TodoImage.Source);
      if (shareOp == null) {
        if (TodoToCreate.TodoInfoValidator()) {
          ViewModel.AddTodoItem(TodoToCreate);
          ViewModel.NewestItem = TodoToCreate;
          Frame.Navigate(typeof(MainPage), ViewModel);
        }
      } else { // on sharing
        shareOp.ReportCompleted();
      }
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e) {
      ViewModel.SelectedItem = null;
      Frame.Navigate(typeof(MainPage), ViewModel);
    }

    private void DeleteTodoButton_Click(object sender, RoutedEventArgs e) {
      if (ViewModel.SelectedItem != null) {
        ViewModel.DeleteTodoItem(ViewModel.SelectedItem.Id);
        Frame.Navigate(typeof(MainPage), ViewModel);
      }
    }

    private void UpdateButton_Click(object sender, RoutedEventArgs e) {
      if (ViewModel.SelectedItem != null) {
        Models.TodoItem TodoToUpdate = new Models.TodoItem(TitleTextBox.Text, DetailTextBox.Text, DueDatePicker.Date, TodoImage.Source);
        TodoToUpdate.Id = ViewModel.SelectedItem.Id;
        if (TodoToUpdate.TodoInfoValidator()) {
          ViewModel.UpdateTodoItem(ViewModel.SelectedItem, TodoToUpdate);
          ViewModel.NewestItem = TodoToUpdate;
          Frame.Navigate(typeof(MainPage), ViewModel);
        }
      }
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
      ImageFile = await picker.PickSingleFileAsync();
      // Ensure a file was selected 
      if (ImageFile != null) {
        using (IRandomAccessStream fileStream = await ImageFile.OpenAsync(Windows.Storage.FileAccessMode.Read)) {
          // Set the image source to the selected bitmap 
          BitmapImage bitmapImage = new BitmapImage();
          bitmapImage.DecodePixelWidth = 350; //match the target Image.Width, not shown
          await bitmapImage.SetSourceAsync(fileStream);
          TodoImage.Source = bitmapImage;
        }
      }
    }
    #endregion

    #region Share controller
    private void ShareButton_Click(object sender, RoutedEventArgs e) {
      DataTransferManager.ShowShareUI();
    }

    // Handle DataRequested event and provide DataPackage
    private void OnShareDataRequested(DataTransferManager sender, DataRequestedEventArgs args) {
      var data = args.Request.Data;
      DataRequestDeferral GetFiles = args.Request.GetDeferral();

      try {
        data.Properties.Title = ViewModel.SelectedItem.Title;
        data.Properties.Description = "A todo item from APP: Todos";
        data.Properties.Thumbnail = RandomAccessStreamReference.CreateFromFile(ImageFile);
        data.SetBitmap(RandomAccessStreamReference.CreateFromFile(ImageFile));
        //data.SetText(ViewModel.SelectedItem.Discription);
      } finally {
        GetFiles.Complete();
      }
      
    }
    #endregion
  }
}
