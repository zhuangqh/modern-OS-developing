using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW6.Models {
  public class DisplayItem {
    public string Title { get; set; }
    public string Description { get; set; }
    public string Date { get; set; }
    public Int64 Id { get; set; }

    public override string ToString() {
      StringBuilder sb = new StringBuilder();
      sb.AppendFormat("Id: {0}\tTitle: {1}\tContext: {2}\tDate: {3}",
        Id, Title, Description, Date);
      return sb.ToString();
    }
  }
}
