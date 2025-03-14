using Microsoft.AspNetCore.Mvc;
using Tests.IntegrationTests.Config;

namespace Tests.UnitTests;

public class F1CarControllerTests : IClassFixture<ExtendedWebApplicationFactoryWithMockAuth<Program>>
{
    private readonly ExtendedWebApplicationFactoryWithMockAuth<Program> _factory;

    public F1CarControllerTests(ExtendedWebApplicationFactoryWithMockAuth<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Test1()
    {
        
    }
}