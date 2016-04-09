using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Text;
using System.Xml;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace HW7 {
  /// <summary>
  /// 可用于自身或导航至 Frame 内部的空白页。
  /// </summary>
  public sealed partial class MainPage : Page {
    public MainPage() {
      this.InitializeComponent();
    }

    private async void GetAttribution_json (string tel) {
      try {
        // 创建一个HTTP client实例对象
        HttpClient httpClient = new HttpClient();

        // Add a user-agent header to the GET request. 
        var headers = httpClient.DefaultRequestHeaders;

        // The safe way to add a header value is to use the TryParseAdd method and verify the return value is true,
        // especially if the header value is coming from user input.
        string header = "ie Mozilla/5.0 (Windows NT 6.2; WOW64; rv:25.0) Gecko/20100101 Firefox/25.0";
        if (!headers.UserAgent.TryParseAdd(header)) {
          throw new Exception("Invalid header value: " + header);
        }
        // 添加apikey， 为了使用百度的api
        headers.Add("apikey", "82b464f3e6e940432862aaabeac7e8c9");

        string getPhoneNumCode_json = "http://apis.baidu.com/apistore/mobilephoneservice/mobilephone?tel=" + tel;

        //发送GET请求
        HttpResponseMessage response = await httpClient.GetAsync(getPhoneNumCode_json);

        // 确保返回值为成功状态
        response.EnsureSuccessStatusCode();

        // 因为返回的字节流中含有中文，传输过程中，所以需要编码后才可以正常显示
        // “\u5e7f\u5dde”表示“广州”，\u表示Unicode
        Byte[] getByte = await response.Content.ReadAsByteArrayAsync();

        // 可以用来测试返回的结果
        //string returnContent = await response.Content.ReadAsStringAsync();

        // UTF-8是Unicode的实现方式之一。这里采用UTF-8进行编码
        Encoding code = Encoding.GetEncoding("UTF-8");
        string result = code.GetString(getByte, 0, getByte.Length);

        // 反序列化结果字符串
        JObject res = (JObject)JsonConvert.DeserializeObject(result);
        if (res["errNum"].ToString() != "0")
          throw (new Exception("手机号码有误"));

        if (res["retData"] != null) {
          ProvinceName.Text = res["retData"]["province"].ToString();
          CATName.Text = res["retData"]["carrier"].ToString();
        }

      } catch (Exception e) {
        infor.Text = e.ToString();
      }
    }

    private async void GetAttribution_xml(string tel) {
      try {
        // 创建一个HTTP client实例对象
        HttpClient httpClient = new HttpClient();

        // Add a user-agent header to the GET request. 
        var headers = httpClient.DefaultRequestHeaders;

        // The safe way to add a header value is to use the TryParseAdd method and verify the return value is true,
        // especially if the header value is coming from user input.
        string header = "ie Mozilla/5.0 (Windows NT 6.2; WOW64; rv:25.0) Gecko/20100101 Firefox/25.0";
        if (!headers.UserAgent.TryParseAdd(header)) {
          throw new Exception("Invalid header value: " + header);
        }

        string getPhoneNumCode_json = "http://life.tenpay.com/cgi-bin/mobile/MobileQueryAttribution.cgi?chgmobile=" + tel;

        //发送GET请求
        HttpResponseMessage response = await httpClient.GetAsync(getPhoneNumCode_json);

        // 确保返回值为成功状态
        response.EnsureSuccessStatusCode();

        // 因为返回的字节流中含有中文，传输过程中，所以需要编码后才可以正常显示
        // “\u5e7f\u5dde”表示“广州”，\u表示Unicode
        Byte[] getByte = await response.Content.ReadAsByteArrayAsync();

        // UTF-8是Unicode的实现方式之一。这里采用UTF-8进行编码
        Encoding code = Encoding.GetEncoding("UTF-8");
        string result = code.GetString(getByte, 0, getByte.Length);

        XmlDocument doc = new XmlDocument();
        doc.LoadXml(result);
        
        // 检查是否出现错误
        XmlNodeList infos = doc.GetElementsByTagName("retmsg");
        foreach (XmlNode node in infos) {
          if (node.InnerXml == "error")
            throw (new Exception("手机号码有误"));
        }

        // 获取归属地及运营商
        infos = doc.GetElementsByTagName("province");
        if (infos != null)
          ProvinceName.Text = infos[0].InnerXml;

        infos = doc.GetElementsByTagName("supplier");
        if (infos != null)
          CATName.Text = infos[0].InnerXml;
      } catch (Exception e) {
        infor.Text = e.ToString();
      }
    }

    private void Submit_json(object sender, RoutedEventArgs e) {
      ProvinceName.Text = "";
      CATName.Text = "";
      infor.Text = ""; // 清除出错信息
      GetAttribution_json(PhoneNum.Text);
    }

    private void Submit_xml(object sender, RoutedEventArgs e) {
      ProvinceName.Text = "";
      CATName.Text = "";
      infor.Text = ""; // 清除出错信息
      GetAttribution_xml(PhoneNum.Text);
    }
  }
}
