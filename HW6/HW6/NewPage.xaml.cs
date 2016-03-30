using System;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.DataTransfer.ShareTarget;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace HW6 {
  /// <summary>
  /// 可用于自身或导航至 Frame 内部的空白页。
  /// </summary>
  public sealed partial class NewPage : Page {
    public NewPage() {
      this.InitializeComponent();
      ViewModel = new ViewModels.TodoItemViewModel();
      DB = new Services.DBService();
    }

    private ViewModels.TodoItemViewModel ViewModel;

    private ShareOperation shareOp;

    private StorageFile ImageFile;

    private Services.DBService DB { get; set; }

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

    #region TodoItem controller
    private void CreateButton_Click(object sender, RoutedEventArgs e) {
      Models.TodoItem TodoToCreate = new Models.TodoItem(TitleTextBox.Text,
        DetailTextBox.Text, DueDatePicker.Date, TodoImage.Source, ImageFile);
      if (shareOp == null) {
        if (TodoToCreate.TodoInfoValidator()) {
          ViewModel.AddTodoItem(TodoToCreate);
          DB.CreateItem(TodoToCreate);  // create an item in database
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
        if (ViewModel.AllItems.Count > 0)
          ViewModel.NewestItem = ViewModel.AllItems[0];
        else
          ViewModel.NewestItem = null;
        DB.DeleteById(ViewModel.SelectedItem.Id);
        Frame.Navigate(typeof(MainPage), ViewModel);
      }
    }

    private void UpdateButton_Click(object sender, RoutedEventArgs e) {
      if (ViewModel.SelectedItem != null) {
        // if not update image set to origin
        if (ImageFile == null) ImageFile = ViewModel.SelectedItem.ShareFile;
        Models.TodoItem TodoToUpdate = new Models.TodoItem(TitleTextBox.Text,
          DetailTextBox.Text, DueDatePicker.Date, TodoImage.Source, ImageFile);

        TodoToUpdate.Id = ViewModel.SelectedItem.Id;
        if (TodoToUpdate.TodoInfoValidator()) {
          ViewModel.UpdateTodoItem(ViewModel.SelectedItem, TodoToUpdate);
          ViewModel.NewestItem = TodoToUpdate;
          DB.UpdateItem(TodoToUpdate);  // update in database
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

  }
}
