using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Notifications;
using NotificationsExtensions.Tiles;
using Windows.UI.Popups;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.Streams;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace HW6 {
  /// <summary>
  /// 可用于自身或导航至 Frame 内部的空白页。
  /// </summary>
  public sealed partial class MainPage : Page {
    public MainPage() {
      this.InitializeComponent();
      this.ViewModel = new ViewModels.TodoItemViewModel();
    }

    public ViewModels.TodoItemViewModel ViewModel { get; set; }

    private string currentId { get; set; }

    #region View Controller
    protected override void OnNavigatedTo(NavigationEventArgs e) {
      if (e.Parameter.GetType() == typeof(ViewModels.TodoItemViewModel)) {
        this.ViewModel = e.Parameter as ViewModels.TodoItemViewModel;
      }
      DataTransferManager.GetForCurrentView().DataRequested += OnShareDataRequested;
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e) {
      DataTransferManager.GetForCurrentView().DataRequested -= OnShareDataRequested;
    }

    private void AddTodoButton_Click(object sender, RoutedEventArgs e) {
      Frame.Navigate(typeof(NewPage), ViewModel);
    }

    private void TodoItem_ItemClicked(object sender, ItemClickEventArgs e) {
      ViewModel.SelectedItem = (e.ClickedItem as Models.TodoItem);
      Frame.Navigate(typeof(NewPage), ViewModel);
    }

    private void UpdateTileButton_Click(object sender, RoutedEventArgs e) {
      if (ViewModel.NewestItem == null) {
        var i = new MessageDialog("There is no Todo Item!").ShowAsync();
        return;
      }
      // In a real app, these would be initialized with actual data
      string from = ViewModel.NewestItem.Title;
      string body = ViewModel.NewestItem.Discription;


      // Construct the tile content
      TileContent content = new TileContent() {
        Visual = new TileVisual() {
          Branding = TileBranding.NameAndLogo,
          DisplayName = "Todos",

          TileSmall = new TileBinding() {
            Content = new TileBindingContentAdaptive() {
              Children =
                      {
                    new TileText()
                    {
                        Text = from,
                        Style = TileTextStyle.Subtitle
                    }
                }
            }
          },

          TileMedium = new TileBinding() {
            Content = new TileBindingContentAdaptive() {
              Children =
                      {
                    new TileText()
                    {
                        Text = from,
                        Style = TileTextStyle.Subtitle
                    },
                    new TileText()
                    {
                        Text = body,
                        Style = TileTextStyle.CaptionSubtle
                    }
                }
            }
          },

          TileWide = new TileBinding() {
            Content = new TileBindingContentAdaptive() {
              Children =
                      {
                    new TileText()
                    {
                        Text = from,
                        Style = TileTextStyle.Subtitle
                    },
                    new TileText()
                    {
                        Text = body,
                        Style = TileTextStyle.CaptionSubtle
                    }
                }
            }
          }
        }
      };

      var notification = new TileNotification(content.GetXml());
      // send the notification
      TileUpdateManager.CreateTileUpdaterForApplication().Update(notification);
    }
    #endregion

    #region Share controller
    private void ShareButton_Click(object sender, RoutedEventArgs e) {
      // get the Id of current button
      FrameworkElement fe = e.OriginalSource as FrameworkElement;
      currentId = fe.DataContext.ToString();
      DataTransferManager.ShowShareUI();
    }

    // Handle DataRequested event and provide DataPackage
    private void OnShareDataRequested(DataTransferManager sender, DataRequestedEventArgs args) {
      Models.TodoItem sharedItem = ViewModel.GetItemById(currentId);
      var data = args.Request.Data;
      DataRequestDeferral GetFiles = args.Request.GetDeferral();

      try {
        data.Properties.Title = sharedItem.Title;
        data.Properties.Description = "A todo item from APP: Todos";
        var imageFile = sharedItem.ShareFile;
        if (imageFile != null) {
          data.Properties.Thumbnail = RandomAccessStreamReference.CreateFromFile(imageFile);
          data.SetBitmap(RandomAccessStreamReference.CreateFromFile(imageFile));
        } else {
          data.SetText(sharedItem.Discription);  // if no image, share text
        }
      } finally {
        GetFiles.Complete();
      }
    }
    #endregion
  }
}
