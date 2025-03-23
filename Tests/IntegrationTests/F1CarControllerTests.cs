using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Testing;
using Tests.IntegrationTests.Config;
using UI.DTO;

namespace Tests.IntegrationTests;

public class F1CarControllerTests : IClassFixture<ExtendedWebApplicationFactoryWithMockAuth<Program>>
{
    private readonly ExtendedWebApplicationFactoryWithMockAuth<Program> _factory;

    public F1CarControllerTests(ExtendedWebApplicationFactoryWithMockAuth<Program> factory)
    {
        _factory = factory;
    }
    
        //MVC Tests
        //Protection
    
        [Fact]
        public void Details_ReturnsNotFound_WhenInvalidIdProvided()
        {
            // Arrange
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions{AllowAutoRedirect = false});
            var invalidF1CarId = -1; 

            // Act
            var response = client.GetAsync($"/F1Car/Details/{invalidF1CarId}").GetAwaiter().GetResult();

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        
        [Fact]
        public void Add_ThrowsUnauthorized_WhenUserIsNotAuthenticated()
        {
            // Arrange
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

            // Act
            var response = client.GetAsync($"/F1Car/Add").GetAwaiter().GetResult();

            // Assert
            Assert.True(response.StatusCode is HttpStatusCode.Found or HttpStatusCode.Unauthorized);

            if (response.StatusCode == HttpStatusCode.Found)
            {
                Assert.Contains("/Account/Login", response.Headers.Location?.ToString());
            }
        }
        
        //Successes
        [Fact]
        public void Details_ReturnsSuccessAndCorrectView_WhenValidIdProvided()
        {
            // Arrange
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions{AllowAutoRedirect = false});
            var validF1CarId = 1; 

            // Act
            var response = client.GetAsync($"/F1Car/Details/{validF1CarId}").GetAwaiter().GetResult();

            // Assert
            response.EnsureSuccessStatusCode(); 

            var responseString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains("F1 Car Details", responseString); 
        }
        
        [Fact]
        public void Add_ReturnsSuccessAndCorrectView_WhenUserIsAdmin()
        {
            // Arrange
            var client = _factory.AuthenticatedInstance(
                    new Claim(ClaimTypes.Role, "Admin"))
                .CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

            // Act
            var response =  client.GetAsync($"/F1Car/Add").GetAwaiter().GetResult();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            Assert.Contains("<h1>Add New F1 Car</h1>", responseString); 
            _factory.AuthenticatedInstance();
        }
        
        //API tests
    
        //Protection
        [Fact]
        public void UpdateF1CarHp_ReturnsUnauthorized_WhenUserIsNotAuthenticated()
        {
            // Arrange
            var client = _factory.CreateClient(); 

            var validF1CarId = 2;
            var updateDto = new UpdateF1CarHpDto { F1CarHp = 1100 };

            // Act
            var response = client.PutAsJsonAsync($"/api/F1Cars/{validF1CarId}", updateDto).GetAwaiter().GetResult();

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public void UpdateF1CarHp_ReturnsUnauthorized_WhenUserIsNotOwnerOrAdmin()
        {
            // Arrange
            var client = _factory.AuthenticatedInstance(
                    new Claim(ClaimTypes.NameIdentifier, "6226f377-77bf-4856-af3d-9bb7934f8e34")) 
                .CreateClient();

            var validF1CarId = 1;
            var updateDto = new UpdateF1CarHpDto { F1CarHp = 1100 };

            // Act
            var response = client.PutAsJsonAsync($"/api/F1Cars/{validF1CarId}", updateDto).GetAwaiter().GetResult();

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            _factory.AuthenticatedInstance();
        }

        [Fact]
        public void UpdateF1CarHp_ReturnsNotFound_WhenF1CarDoesNotExist()
        {
            // Arrange
            var client = _factory
                .CreateClient();

            var invalidF1CarId = -1; 
            var updateDto = new UpdateF1CarHpDto { F1CarHp = 950 };

            // Act
            var response = client.PutAsJsonAsync($"/api/F1Cars/{invalidF1CarId}", updateDto).GetAwaiter().GetResult();

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        //Successes
        [Fact]
        public void UpdateF1CarHp_ReturnsSuccess_WhenUserIsAdmin()
        {
            // Arrange
            var client = _factory.AuthenticatedInstance(new Claim(
                        ClaimTypes.NameIdentifier, "6226f377-77bf-4856-af3d-9bb7934f8e34"),
                            new Claim(ClaimTypes.Role, "Admin")) 
                .CreateClient();

            var validF1CarId = 2; 
            var updateDto = new UpdateF1CarHpDto { F1CarHp = 1200 };

            // Act
            var response = client.PutAsJsonAsync($"/api/F1Cars/{validF1CarId}", updateDto).GetAwaiter().GetResult();

            // Debugging output
            var responseContent = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            // Assert
            response.EnsureSuccessStatusCode(); 
            Assert.Contains("Horsepower updated successfully", responseContent);
            _factory.AuthenticatedInstance();
        }

}