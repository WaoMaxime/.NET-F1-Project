using System.ComponentModel.DataAnnotations;
using BusinessLayer;
using DataAccessLayer.EF;
using Microsoft.Extensions.DependencyInjection;
using Domain;
using Tests.IntegrationTests.Config;

namespace Tests.IntegrationTests;

public class F1CarManagerTests : IClassFixture<ExtendedWebApplicationFactoryWithMockAuth<Program>>
{
    private readonly ExtendedWebApplicationFactoryWithMockAuth<Program> _factory;

    public F1CarManagerTests(ExtendedWebApplicationFactoryWithMockAuth<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public void GetF1CarById_ReturnsF1CarObject_GivenValidF1CarId()
    {
        // Arrange
        using (var scope = _factory.Services.CreateScope())
        {
            var f1CarManager = scope.ServiceProvider.GetService<IManager>();

            var f1Carid = 1;

            // Act
            var f1Car = f1CarManager.GetF1Car(f1Carid);

            // Assert
            Assert.Equal(f1Carid, f1Car.Id);
        }
    }
    
    [Fact]
    public void GetF1CarById_ReturnsNull_GivenInvalidF1CarId()
    {
        // Arrange
        using (var scope = _factory.Services.CreateScope())
        {
            var f1CarManager = scope.ServiceProvider.GetRequiredService<IManager>(); //important!!

            var f1Carid = -1; // invalid

            // Act
            var f1Car = f1CarManager.GetF1Car(f1Carid);

            // Assert
            Assert.Null(f1Car);
        }
    }
    
    
    [Fact]
    public void AddF1Car_GivenValidData_ReturnsNewF1CarObject()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var f1CarManager = scope.ServiceProvider.GetRequiredService<IManager>(); //important!!

        F1Team Team = F1Team.RedBull;
        string Chasis = "RDBL20";
        int ConstructorsPosition = 2;
        int DriversPositions = 3;
        DateTime ManufactureDate = DateTime.Now.AddYears(-10);
        TyreType Tyres = TyreType.Soft;
        int EnginePower = 1020;
    
            
        // Act
        F1Car car = f1CarManager.AddF1Car(Team, Chasis, ConstructorsPosition,DriversPositions,ManufactureDate,Tyres,EnginePower);
        // Assert
        Assert.NotNull(car);
        Assert.Equal(Chasis, car.Chasis);
        
        //Assert.False(book.Id == default(int));
        // is book added to db
        var dbCtx = scope.ServiceProvider.GetService<F1CarDbContext>();
        Assert.NotNull(dbCtx?.F1Cars.Find(car.Id));
    }
    
    [Fact]
    public void AddF1Car_GivenInvalidData_ReturnsValidationException()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var f1CarManager = scope.ServiceProvider.GetRequiredService<IManager>();
        
        var validTeam = F1Team.RedBull;
        var emptyChasis = ""; 
        var validConstructorsPosition = 2; 
        var validDriversPositions = 3; 
        var futureManufactureDate = DateTime.Now.AddYears(5); 
        var validTyres = TyreType.Soft; 
        var negativeEnginePower = -500; 
    
            
        // Act & Assert 
        Assert.Throws<ValidationException>(() =>
            f1CarManager.AddF1Car(validTeam, emptyChasis, validConstructorsPosition, validDriversPositions,
                futureManufactureDate, validTyres, negativeEnginePower));
    }
}