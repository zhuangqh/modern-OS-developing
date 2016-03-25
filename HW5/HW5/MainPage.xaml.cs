using System;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Notifications;
using NotificationsExtensions.Tiles;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace HW5 {
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
      Frame.Navigate(typeof(NewPage), ViewModel);
    }

    private void TodoItem_ItemClicked(object sender, ItemClickEventArgs e) {
      ViewModel.SelectedItem = (e.ClickedItem as Models.TodoItem);
      Frame.Navigate(typeof(NewPage), ViewModel);
    }

    private void UpdateTileButton_Click(object sender, RoutedEventArgs e) {
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
  }
}
