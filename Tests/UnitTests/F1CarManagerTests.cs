using BusinessLayer;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Tests.IntegrationTests.Config;
using UI.Controllers;

namespace Tests.UnitTests;

public class F1CarManagerTests : IClassFixture<ExtendedWebApplicationFactoryWithMockAuth<Program>>
{
    
    private readonly ExtendedWebApplicationFactoryWithMockAuth<Program> _factory;

    public F1CarManagerTests(ExtendedWebApplicationFactoryWithMockAuth<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public void Details_Get_AsAuthorizedUser_ReturnsEditView_GivenValidF1CarId()
    {
        // Arrange
        int validCarId = 1;

        var f1CarMgrMock = new Mock<IManager>();
        f1CarMgrMock.Setup(mgr => mgr.GetF1Car(validCarId))
            .Returns(new F1Car()
            {
                Id = validCarId, Chasis = "Testing chasis", ConstructorsPosition = 1, Tyres = TyreType.Soft, Team = F1Team.Mercedes,
            });

        var f1CarController = new F1CarController(f1CarMgrMock.Object);

        // Act
        var iActionResult = f1CarController.Details(validCarId);

        // Assert
        var view = Assert.IsType<ViewResult>(iActionResult);
        Assert.Equal("Details", view.ViewName ?? nameof(f1CarController.Details));
    }

}