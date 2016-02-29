using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace ToDoList
{
  public class CategoryTest : IDisposable
  {

    public void Dispose()
    {
      Task.DeleteAll();
      Category.DeleteAll();
    }

    public CategoryTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=to_do_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_GetTasks_RetrievesAllTasksWithCategory()
    {
      //Arrange
      Category testCategory = new Category("Household chores");
      testCategory.Save();

      Task firstTask = new Task("Mow the lawn");
      firstTask.Save();
      firstTask.AddCategory(testCategory);
      Task secondTask = new Task("Do the dishes");
      secondTask.Save();
      secondTask.AddCategory(testCategory);

      //Act
      List<Task> testTaskList = new List<Task> {firstTask, secondTask};
      List<Task> resultTaskList = testCategory.GetTasks();

      //Assert
      Assert.Equal(testTaskList, resultTaskList);
    }

    [Fact]
    public void Test_Delete_DeletesCategoryFromDatabase()
    {
      //Arrange
      string name1 = "Home stuff";
      Category testCategory1 = new Category(name1);
      testCategory1.Save();

      string name2 = "Work stuff";
      Category testCategory2 = new Category(name2);
      testCategory2.Save();

      //Act
      testCategory1.Delete();
      List<Category> resultCategories = Category.GetAll();
      List<Category> testCategoryList = new List<Category> {testCategory2};

      Assert.Equal(testCategoryList, resultCategories);
    }

        [Fact]
    public void Test_Delete_DeletesCategoryAssociationsFromDatabase()
    {
      //Arrange
      Task testTask = new Task("Mow the lawn");
      testTask.Save();

      string testName = "Home stuff";
      Category testCategory = new Category(testName);
      testCategory.Save();

      //Act
      testCategory.AddTask(testTask);
      testCategory.Delete();

      List<Category> resultTaskCategories = testTask.GetCategories();
      List<Category> testTaskCategories = new List<Category> {};

      //Assert
      Assert.Equal(testTaskCategories, resultTaskCategories);
    }

    [Fact]
    public void Test_CategoriesEmptyAtFirst()
    {
      //Arrange, Act
      int result = Category.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueForSameName()
    {
      //Arrange, Act
      Category firstCategory = new Category("Household chores");
      Category secondCategory = new Category("Household chores");

      //Assert
      Assert.Equal(firstCategory, secondCategory);
    }

    [Fact]
    public void Test_Save_SavesCategoryToDatabase()
    {
      //Arrange
      Category testCategory = new Category("Household chores");
      testCategory.Save();

      //Act
      List<Category> result = Category.GetAll();
      List<Category> testList = new List<Category>{testCategory};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Save_AssignsIdToCategoryObject()
    {
      //Arrange
      Category testCategory = new Category("Household chores");
      testCategory.Save();

      //Act
      Category savedCategory = Category.GetAll()[0];

      int result = savedCategory.GetId();
      int testId = testCategory.GetId();

      //Assert
      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_Find_FindsCategoryInDatabase()
    {
      //Arrange
      Category testCategory = new Category("Household chores");
      testCategory.Save();

      //Act
      Category foundCategory = Category.Find(testCategory.GetId());

      //Assert
      Assert.Equal(testCategory, foundCategory);
    }

    [Fact]
    public void Test_EqualOverrideTrueForSameDescription()
    {
      //Arrange, Act
      Task firstTask = new Task("Mow the lawn", 1);
      Task secondTask = new Task("Mow the lawn", 1);

      //Assert
      Assert.Equal(firstTask, secondTask);
    }

    [Fact]
    public void Test_Save()
    {
      //Arrange
      Task testTask = new Task("Mow the lawn", 1);
      testTask.Save();

      //Act
      List<Task> result = Task.GetAll();
      List<Task> testList = new List<Task>{testTask};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_SaveAssignsIdToObject()
    {
      //Arrange
      Task testTask = new Task("Mow the lawn", 1);
      testTask.Save();

      //Act
      Task savedTask = Task.GetAll()[0];

      int result = savedTask.GetId();
      int testId = testTask.GetId();

      //Assert
      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_FindFindsTaskInDatabase()
    {
      //Arrange
      Task testTask = new Task("Mow the lawn", 1);
      testTask.Save();

      //Act
      Task foundTask = Task.Find(testTask.GetId());

      //Assert
      Assert.Equal(testTask, foundTask);
    }

    [Fact]
    public void Test_AddTask_AddsTaskToCategory()
    {
      //Arrange
      Category testCategory = new Category("Household chores");
      testCategory.Save();

      Task testTask = new Task("Mow the lawn");
      testTask.Save();

      Task testTask2 = new Task("Water the garden");
      testTask2.Save();

      //Act
      testCategory.AddTask(testTask);
      testCategory.AddTask(testTask2);

      List<Task> result = testCategory.GetTasks();
      List<Task> testList = new List<Task>{testTask, testTask2};

      //Assert
      Assert.Equal(testList, result);
    }

      [Fact]
     public void Test_GetTasks_ReturnsAllCategoryTasks()
     {
       //Arrange
       Category testCategory = new Category("Household chores");
       testCategory.Save();

       Task testTask1 = new Task("Mow the lawn");
       testTask1.Save();

       Task testTask2 = new Task("Buy plane ticket");
       testTask2.Save();

       //Act
       testCategory.AddTask(testTask1);
       List<Task> savedTasks = testCategory.GetTasks();
       List<Task> testList = new List<Task> {testTask1};

       //Assert
       Assert.Equal(testList, savedTasks);
     }
  }
}
