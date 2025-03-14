using System.ComponentModel.DataAnnotations;
using BusinessLayer;
using DataAccessLayer;
using Domain;
using Moq;

namespace Tests.UnitTests;

public class F1CarManagerTests
{

    [Fact]
    public void ChangeHpF1Car_WithValidData_UpdatesCarSuccessfully()
    {
        // Arrange
        int carId = 1;
        double newHp = 1000.5;
    
        var existingCar = new F1Car { Id = carId, EnginePower = 800 };
    
        var repoMock = new Mock<IRepository>();
        repoMock.Setup(repo => repo.ReadF1Car(carId)).Returns(existingCar);
        repoMock.Setup(repo => repo.UpdateHpF1Car(It.IsAny<F1Car>())).Returns((F1Car c) => c);
    
        var manager = new Manager(repoMock.Object);

        // Act
        var updatedCar = manager.ChangeHpF1Car(carId, newHp);

        // Assert
        Assert.Equal(newHp, updatedCar.EnginePower);
    
        repoMock.Verify(repo => repo.ReadF1Car(carId), Times.Once);
    
        repoMock.Verify(repo => repo.UpdateHpF1Car(It.Is<F1Car>(c => c.EnginePower != null && c.EnginePower == newHp)), Times.Once);
    }
    [Fact]
    public void ChangeHpF1Car_WithInvalidData_ThrowsException()
    {
        // Arrange
        int carId = 2;
        double newHp = -500;

        var existingCar = new F1Car { Id = carId, EnginePower = 800 };

        var repoMock = new Mock<IRepository>();
        repoMock.Setup(repo => repo.ReadF1Car(carId)).Returns(existingCar);

        var manager = new Manager(repoMock.Object);

        // Act & Assert
        var exception = Assert.Throws<ValidationException>(() => manager.ChangeHpF1Car(carId, newHp));

        Assert.Contains("validation", exception.Message, StringComparison.OrdinalIgnoreCase);

        repoMock.Verify(repo => repo.ReadF1Car(carId), Times.Once);

        repoMock.Verify(repo => repo.UpdateHpF1Car(It.IsAny<F1Car>()), Times.Never);
    }

    [Fact]
    public void AddTyreToCar_WithValidCar_AddsTyreSuccessfully()
    {
        // Arrange
        int carId = 1;
        var existingCar = new F1Car { Id = carId, Chasis = "Test Car" };

        var tyreType = TyreType.Soft;
        int tyrePressure = 30;
        int operationalTemperature = 90;

        var repoMock = new Mock<IRepository>();
        
        repoMock.Setup(repo => repo.ReadF1Car(carId)).Returns(existingCar);

        var manager = new Manager(repoMock.Object);

        // Act
        var result = manager.AddTyreToCar(carId, tyreType, tyrePressure, operationalTemperature);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(existingCar, result.Car);
        Assert.Equal(tyreType, result.Tyre);
        Assert.Equal(tyrePressure, result.TyrePressure);
        Assert.Equal(operationalTemperature, result.OperationalTemperature);
        
        repoMock.Verify(repo => repo.ReadF1Car(carId), Times.Once);
        
        repoMock.Verify(repo => repo.AddCarTyre(It.Is<CarTyre>(t =>
            t.Car == existingCar &&
            t.Tyre == tyreType &&
            t.TyrePressure == tyrePressure &&
            t.OperationalTemperature == operationalTemperature
        )), Times.Once);
    }

    
    [Fact]
    public void AddTyreToCar_WithInvalidCar_ThrowsException()
    {
        // Arrange
        int invalidCarId = -1; 

        var tyreType = TyreType.Medium;
        int tyrePressure = 28;
        int operationalTemperature = 85;

        var repoMock = new Mock<IRepository>();
        
        repoMock.Setup(repo => repo.ReadF1Car(invalidCarId)).Returns((F1Car)null);

        var manager = new Manager(repoMock.Object);

        // Act & Assert
        var exception = Assert.Throws<Exception>(() =>
            manager.AddTyreToCar(invalidCarId, tyreType, tyrePressure, operationalTemperature)
        );

        Assert.Equal("Car not found!", exception.Message);
        
        repoMock.Verify(repo => repo.ReadF1Car(invalidCarId), Times.Once);
        
        repoMock.Verify(repo => repo.AddCarTyre(It.IsAny<CarTyre>()), Times.Never);
    }

}

