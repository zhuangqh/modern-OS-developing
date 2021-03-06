﻿using SQLitePCL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace HW6.Services {
  public class DBService {
    private void LoadDatabase() {
      conn = new SQLiteConnection("Todos.db");
      string sql = @"CREATE TABLE IF NOT EXISTS "
                    + "Todos (TableId    INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,"
                          + "ItemId      VARCHAR(140),"
                          + "Title       VARCHAR(140),"
                          + "Description VARCHAR(140),"
                          + "Date        VARCHAR(140)"
                          + ");";
      using (var statement = conn.Prepare(sql)) {
        statement.Step();
      }
    }

    private SQLiteConnection conn { get; set; }

    public DBService() {
      LoadDatabase();
    }

    /// <summary>
    /// Database API
    /// </summary>

    public void CreateItem(Models.TodoItem TodoToCreate) {
      try {
        string sql = "INSERT INTO Todos (ItemId, Title, Description, Date) VALUES (?, ?, ?, ?)";
        using (var statement = conn.Prepare(sql)) {
          statement.Bind(1, TodoToCreate.Id);
          statement.Bind(2, TodoToCreate.Title);
          statement.Bind(3, TodoToCreate.Discription);
          statement.Bind(4, TodoToCreate.DueDate.ToString());
          statement.Step();
        }
      } catch (Exception ex) {
        var i = new MessageDialog("Error in creating item" + ex).ShowAsync();
      }
    }

    public List<Models.DisplayItem> GetItemsByStr(string str) {
      string sql = "SELECT TableId, Title, Description, Date FROM Todos "
        + "WHERE Title LIKE ? OR Description LIKE ? OR Date LIKE ?";
      int i, searchKeyNum = 3;
      using (var statement = conn.Prepare(sql)) {
        for (i = 1; i <= searchKeyNum; ++i)
          statement.Bind(i, "%" + str + "%");

        List<Models.DisplayItem> res = new List<Models.DisplayItem>();
        Models.DisplayItem tmp;
        while (statement.Step() == SQLiteResult.ROW) { // get result of one row
          tmp = new Models.DisplayItem() {
            Id = (Int64)statement[0],
            Title = (string)statement[1],
            Description = (string)statement[2],
            Date = (string)statement[3]
          };
          res.Add(tmp);
        }

        return res;
      }
    }

    public void UpdateItem(Models.TodoItem TodoToUpdate) {
      string sql = "UPDATE Todos SET Title = ?, Description = ?, Date = ? WHERE ItemId = ?";
      using (var custstmt = conn.Prepare(sql)) {
        custstmt.Bind(1, TodoToUpdate.Title);
        custstmt.Bind(2, TodoToUpdate.Discription);
        custstmt.Bind(3, TodoToUpdate.DueDate.ToString());
        custstmt.Bind(4, TodoToUpdate.Id);
        custstmt.Step();
      }
    }

    public void DeleteById(string itemId) {
      string sql = "DELETE FROM Todos WHERE ItemId = ?";
      using (var statement = conn.Prepare(sql)) {
        statement.Bind(1, itemId);
        statement.Step();
      }
    }
  }
}
