using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW3.ViewModels {
  public class TodoItemViewModel {
    private ObservableCollection<Models.TodoItem> Items = new ObservableCollection<Models.TodoItem>();
    public ObservableCollection<Models.TodoItem> AllItems { get { return this.Items; } }

    private Models.TodoItem selectedItem = default(Models.TodoItem);
    public Models.TodoItem SelectedItem {
      get { return selectedItem; }
      set { this.selectedItem = value; }
    }

    public TodoItemViewModel() {
      // 用于测试
      this.Items.Add(new Models.TodoItem("haha", "xixixi", DateTimeOffset.Now));
      this.Items.Add(new Models.TodoItem("hhh", "xixixixixi", DateTimeOffset.Now));
    }

    public void AddTodoItem(string title, string discription, DateTimeOffset duetime) {
      this.Items.Add(new Models.TodoItem(title, discription, duetime));
    }

    public void AddTodoItem(Models.TodoItem todo) {
      this.Items.Add(todo);
    }

    public void DeleteTodoItem(string id) {
      this.Items.Remove(this.Items.SingleOrDefault(i => i.Id == id));
      this.selectedItem = null;
    }

    public void UpdateTodoItem(string id, Models.TodoItem UpdateInfo) {
      Models.TodoItem ItemToUpdate = this.Items.FirstOrDefault(i => i.Id == id);
      if (ItemToUpdate != null) {
        ItemToUpdate.Update(ref UpdateInfo);
      }
      this.selectedItem = null;
    }
  }
}
