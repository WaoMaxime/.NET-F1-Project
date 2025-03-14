using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using BusinessLayer;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Tests.IntegrationTests.Config;
using UI.Controllers;

namespace Tests.UnitTests;

public class F1CarControllerTests 
{
    [Fact]
    public void Details_Get_AsAuthorizedUser_ReturnsDetailsView_GivenValidF1CarId()
    {
        // Arrange
        int validF1CarId = 3;

        var f1MgrMock = new Mock<IManager>();
        
        f1MgrMock.Setup(mgr => mgr.GetF1CarWithDetails(validF1CarId))
            .Returns(new F1Car() { Id = validF1CarId })
            .Verifiable();

        var f1Controller = new F1CarController(f1MgrMock.Object);
        
        var authorizedUser = new ClaimsPrincipal(new ClaimsIdentity([
            new Claim(ClaimTypes.Name, "AuthorizedUser"),
            new Claim(ClaimTypes.Role, "User")
        ], "mock"));

        f1Controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = authorizedUser }
        };

        // Act
        var iActionResult = f1Controller.Details(validF1CarId);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(iActionResult);
    
        Assert.True(viewResult.ViewName is null or "Details");
        
        f1MgrMock.Verify(mgr => mgr.GetF1CarWithDetails(validF1CarId), Times.Once);
    }

    [Fact]
    public void Details_Get_AsAuthorizedUser_ReturnsEmptyDetailsView_GivenInvalidF1CarId()
    {
        // Arrange
        int validF1CarId = -3;
    
        var f1MgrMock = new Mock<IManager>();
        f1MgrMock.Setup(mgr => mgr.GetF1CarWithDetails(validF1CarId))
            .Returns(new F1Car() { Id = validF1CarId })
            .Verifiable(); 

        var f1Controller = new F1CarController(f1MgrMock.Object);
        
        var authorizedUser = new ClaimsPrincipal(new ClaimsIdentity([
            new Claim(ClaimTypes.Name, "AuthorizedUser"),
            new Claim(ClaimTypes.Role, "User")
        ], "mock"));

        f1Controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = authorizedUser }
        };

        // Act
        var iActionResult = f1Controller.Details(validF1CarId);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(iActionResult);
        Assert.True(viewResult.ViewName is null);
        f1MgrMock.Verify(mgr => mgr.GetF1Car(validF1CarId), Times.Never);
    }

    [Fact]
    public void Add_Post_AsAdmin_WithValidModel_RedirectsToDetails()
    {
        // Arrange
        var managerMock = new Mock<IManager>();

        var newCar = new F1Car
        {
            Id = 1,
            Chasis = "Ferrari Chasis",
            ConstructorsPosition = 2,
            DriversPositions = 1, 
            ManufactureDate = new DateTime(2024, 3, 1),
            Tyres = TyreType.Medium,
            Team = F1Team.Ferrari,
            User = new IdentityUser(),
            EnginePower = 1000 
        };

        var controller = new F1CarController(managerMock.Object);
        
        var adminUser = new ClaimsPrincipal(new ClaimsIdentity([
            new Claim(ClaimTypes.Role, "Admin")
        ], "mock"));

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = adminUser }
        };

        // Act
        var result = controller.Add(newCar);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Details", redirectResult.ActionName);
        Assert.Equal(newCar.Id, redirectResult.RouteValues?["Id"]);
        
        managerMock.Verify(mgr => mgr.AddF1Car(
            newCar.Team,
            newCar.Chasis,
            newCar.ConstructorsPosition,
            newCar.DriversPositions, 
            newCar.ManufactureDate,
            newCar.Tyres,
            newCar.User, 
            newCar.EnginePower 
        ), Times.Once);
    }
    
    [Fact]
    public void Add_Post_AsAdmin_WithInvalidModel_ReturnsView()
    {
        // Arrange
        var managerMock = new Mock<IManager>();

        var newCar = new F1Car
        {
            Id = -5,
            Chasis = null, 
            ConstructorsPosition = 2,
            DriversPositions = -5, 
            ManufactureDate = new DateTime(2024, 3, 1),
            Tyres = TyreType.Medium,
            Team = F1Team.Ferrari,
            User = null, 
            EnginePower = 0 
        };

        var controller = new F1CarController(managerMock.Object);

        var adminUser = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Role, "Admin")
        }, "mock"));

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = adminUser }
        };
        
        var validationContext = new ValidationContext(newCar);
        var validationResults = new List<ValidationResult>();
        Validator.TryValidateObject(newCar, validationContext, validationResults, true);

        foreach (var validationResult in validationResults)
        {
            controller.ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage);
        }

        // Act
        var result = controller.Add(newCar);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(newCar, viewResult.Model); 
        
        managerMock.Verify(mgr => mgr.AddF1Car(
            It.IsAny<F1Team>(),
            It.IsAny<string>(),
            It.IsAny<int>(),
            It.IsAny<int>(), 
            It.IsAny<DateTime>(),
            It.IsAny<TyreType>(),
            It.IsAny<IdentityUser>(),
            It.IsAny<double?>()
        ), Times.Never);
    }

}