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

    public override string ToString() {
      return Title + ", " + Description + ", " + Date;
    }
  }
}
