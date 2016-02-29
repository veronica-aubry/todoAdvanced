using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace ToDoList
{
  public class Task
  {
    private int _id;
    private string _description;

    public Task(string Description, int Id = 0)
    {
      _id = Id;
      _description = Description;
    }

    public override bool Equals(System.Object otherTask)
    {
        if (!(otherTask is Task))
        {
          return false;
        }
        else {
          Task newTask = (Task) otherTask;
          bool idEquality = this.GetId() == newTask.GetId();
          bool descriptionEquality = this.GetDescription() == newTask.GetDescription();
          return (idEquality && descriptionEquality);
        }
    }

    public int GetId()
    {
      return _id;
    }
    public string GetDescription()
    {
      return _description;
    }
    public void SetDescription(string newDescription)
    {
      _description = newDescription;
    }
    public static List<Task> GetAll()
    {
      List<Task> AllTasks = new List<Task>{};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM tasks", conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int taskId = rdr.GetInt32(0);
        string taskDescription = rdr.GetString(1);
        Task newTask = new Task(taskDescription, taskId);
        AllTasks.Add(newTask);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return AllTasks;
    }
    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO tasks (description) OUTPUT INSERTED.id VALUES (@TaskDescription)", conn);

      SqlParameter descriptionParam = new SqlParameter();
      descriptionParam.ParameterName = "@TaskDescription";
      descriptionParam.Value = this.GetDescription();

      cmd.Parameters.Add(descriptionParam);

      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM tasks;", conn);
      cmd.ExecuteNonQuery();
    }

    public static Task Find(int id)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM tasks WHERE id = @TaskId", conn);
      SqlParameter taskIdParameter = new SqlParameter();
      taskIdParameter.ParameterName = "@TaskId";
      taskIdParameter.Value = id.ToString();
      cmd.Parameters.Add(taskIdParameter);
      rdr = cmd.ExecuteReader();

      int foundTaskId = 0;
      string foundTaskDescription = null;

      while(rdr.Read())
      {
        foundTaskId = rdr.GetInt32(0);
        foundTaskDescription = rdr.GetString(1);
      }
      Task foundTask = new Task(foundTaskDescription, foundTaskId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundTask;
    }

    public void AddCategory(Category newCategory)
   {
     SqlConnection conn = DB.Connection();
     conn.Open();

     SqlCommand cmd = new SqlCommand("INSERT INTO categories_tasks (category_id, task_id) VALUES (@CategoryId, @TaskId);", conn);

     SqlParameter categoryIdParameter = new SqlParameter();
     categoryIdParameter.ParameterName = "@CategoryId";
     categoryIdParameter.Value = newCategory.GetId();
     cmd.Parameters.Add(categoryIdParameter);

     SqlParameter taskIdParameter = new SqlParameter();
     taskIdParameter.ParameterName = "@TaskId";
     taskIdParameter.Value = this.GetId();
     cmd.Parameters.Add(taskIdParameter);

     cmd.ExecuteNonQuery();

     if (conn != null)
     {
       conn.Close();
     }
   }

   public List<Category> GetCategories()
   {
     SqlConnection conn = DB.Connection();
     SqlDataReader rdr = null;
     conn.Open();

     SqlCommand cmd = new SqlCommand("SELECT category_id FROM categories_tasks WHERE task_id = @TaskId;", conn);

     SqlParameter taskIdParameter = new SqlParameter();
     taskIdParameter.ParameterName = "@TaskId";
     taskIdParameter.Value = this.GetId();
     cmd.Parameters.Add(taskIdParameter);

     rdr = cmd.ExecuteReader();

     List<int> categoryIds = new List<int> {};

     while (rdr.Read())
     {
       int categoryId = rdr.GetInt32(0);
       categoryIds.Add(categoryId);
     }
     if (rdr != null)
     {
       rdr.Close();
     }

     List<Category> categories = new List<Category> {};

     foreach (int categoryId in categoryIds)
     {
       SqlDataReader queryReader = null;
       SqlCommand categoryQuery = new SqlCommand("SELECT * FROM categories WHERE id = @CategoryId;", conn);

       SqlParameter categoryIdParameter = new SqlParameter();
       categoryIdParameter.ParameterName = "@CategoryId";
       categoryIdParameter.Value = categoryId;
       categoryQuery.Parameters.Add(categoryIdParameter);

       queryReader = categoryQuery.ExecuteReader();
       while (queryReader.Read())
       {
         int thisCategoryId = queryReader.GetInt32(0);
         string categoryName = queryReader.GetString(1);
         Category foundCategory = new Category(categoryName, thisCategoryId);
         categories.Add(foundCategory);
       }
       if (queryReader != null)
       {
         queryReader.Close();
       }
     }
     if (conn != null)
     {
       conn.Close();
     }
     return categories;
   }

    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM tasks WHERE id = @TaskId; DELETE FROM categories_tasks WHERE task_id = @TaskId;", conn);

      SqlParameter taskIdParameter = new SqlParameter();
      taskIdParameter.ParameterName = "@TaskId";
      taskIdParameter.Value = this.GetId();

      cmd.Parameters.Add(taskIdParameter);
      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }
  }
}
