using Newtonsoft.Json;
using System;
using System.IO;
using System.Xml.Serialization;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml.Media.Imaging;

namespace HW4.Models {
  public class EditPanelData {
    public string Title { get; set; }

    public string Detail { get; set; }

    public DateTimeOffset DueDate { get; set; }

    public EditPanelData() {
      Title = string.Empty;
      Detail = string.Empty;
      DueDate = DateTime.Now;
    }

    #region Methods for handling the apps permanent data
    public void LoadData() {
      if (ApplicationData.Current.RoamingSettings.Values.ContainsKey("TheData")) {
        EditPanelData data = JsonConvert.DeserializeObject<EditPanelData>(
            (string)ApplicationData.Current.RoamingSettings.Values["TheData"]);

        Title = data.Title;
        Detail = data.Detail;
        DueDate = data.DueDate;
      }
    }

    public void SaveData() {
      EditPanelData data = new EditPanelData { Title = this.Title, Detail = this.Detail, DueDate = this.DueDate };
      ApplicationData.Current.RoamingSettings.Values["TheData"] =
          JsonConvert.SerializeObject(data);
    }
    #endregion
  }
}
