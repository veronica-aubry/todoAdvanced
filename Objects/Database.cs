using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System;

namespace ToDoList
{
  public class DB
  {
    public static SqlConnection Connection()
    {
      SqlConnection conn = new SqlConnection(DBConfiguration.ConnectionString);
      return conn;
    }
  }
}
