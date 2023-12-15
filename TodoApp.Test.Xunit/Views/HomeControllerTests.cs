using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;
using Todo.Areas.Customer.Controllers;
using Todo.DataAccess.data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Todo.Models;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Http;
using Todo.Models.ViewModels;

namespace TodoApp.Test.Xunit.Views
{
    public class HomeControllerTests
    {
        //[Fact]
        //public void Test_Todo_Action_Returns_View()
        //{
        //    // Arrange
        //    var loggerMock = new Mock<ILogger<HomeController>>();
        //    var contextMock = new Mock<ApplicationDbContext>();
        //    var controller = new HomeController(loggerMock.Object, contextMock.Object);

        //    // Act
        //    var result = controller.Todo();

        //    // Assert
        //    var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
        //    Assert.Equal(404, statusCodeResult.StatusCode);
        //}
    }
}