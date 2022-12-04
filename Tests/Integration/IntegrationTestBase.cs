using NUnit.Framework;
using Bogus;

namespace IntegrationTests;

[TestFixture]
public abstract class IntegrationTestBase
{
    protected HttpClient RegisterApiHttpClient {get; set;}
    protected Faker Faker {get; set;}

    [OneTimeSetUp]
    public void SetUp()
    {
        var factory = new RegisterApiWebApplicationFactory();
        RegisterApiHttpClient = factory.CreateClient();
        Faker = new Faker();
    }
}