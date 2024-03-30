using System;
using JetBrains.Annotations;
using LegacyApp;
using Xunit;

namespace LegacyApp.Tests;

[TestSubject(typeof(UserService))]
public class UserServiceTest
{

    [Fact]
    public void AddUser_Should_Return_False_When_Name_Is_Misssing()
    {
        
        var userService = new UserService();
        var addResult = userService.AddUser("", "Doe", "johndoe@gmail.com", DateTime.Parse("1982-03-21"), 1);
        Assert.Equal(false,addResult);
        Assert.False(addResult);
    }
    [Fact]
    public void AddUser_Should_Return_False_When_Email_Is_Incorrect()
    {
        
        var userService = new UserService();
        var addResult = userService.AddUser("", "Doe", "johndoegmail.com", DateTime.Parse("1982-03-21"), 1);
     
        Assert.False(addResult);
    }
    [Fact]
    public void AddUser_Should_Throw_Exception_When_User_Doesnt_Exist()
    {
        
        var userService = new UserService();
        Assert.Throws<ArgumentException>(() =>
            {
            userService.AddUser("Joe", "Doe", "johndoe@gmail.com", DateTime.Parse("1982-03-21"), 7);
            });
        
    }
    [Fact]
    public void AddUser_Should_Return_False_When_User_Age_Is_Less_Than_21()
    {
        var userService = new UserService();
        var result = userService.AddUser("John", "Doe", "johndoe@gmail.com", DateTime.Now.AddYears(-20), 1);
        Assert.False(result);
    }
    [Fact]
         public void AddUser_Should_Return_False_When_User_Has_CreditLimit_Less_Than_500()
         {
             var userService = new UserService();
             
             var user = new User
             {
                 // FirstName = "John",
                 // LastName = "Doe",
                 // EmailAddress = "johndoe@example.com",
                 // DateOfBirth = DateTime.Now.AddYears(-30),
                 // HasCreditLimit = true,
                 // CreditLimit = 400
             };

             var result = userService.AddUser(user.FirstName, user.LastName, user.EmailAddress, user.DateOfBirth, 1);

          
             Assert.False(result);
     
         }
  
    
}
    
   
    
