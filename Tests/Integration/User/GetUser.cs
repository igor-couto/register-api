using System.Net;
using System.Text.Json;
using System.Text;
using NUnit.Framework;
using FluentAssertions;
using Bogus;
using RegisterAPI.Domain.Requests;

namespace IntegrationTests.User;

public class GetUserTests : IntegrationTestBase
{
    // [Test]
    // public async Task Should_CreateUser()
    // {
    //     var createUserRequest = new CreateUserRequest
    //     {
    //          UserName = Faker.Person.UserName,
    //          FirstName = Faker.Person.FirstName,
    //          LastName = Faker.Person.LastName,
    //          Email = Faker.Person.Email,
    //          PhoneNumber = Faker.Person.Phone,
    //          Password = Faker.Random.Word()
    //     };
        
    //     var httpResult = await RegisterApiHttpClient.PostAsync("users", new StringContent(JsonSerializer.Serialize(createUserRequest), Encoding.UTF8, "application/json"));
        
    //     httpResult.StatusCode.Should().Be(HttpStatusCode.Created);
    //     httpResult.Content.Headers.ContentType.CharSet.Should().Be("utf-8");
    //     httpResult.Content.Headers.ContentType.MediaType.Should().Be("application/json");

    //     var stream = await httpResult.Content.ReadAsStringAsync();
    //     var user = JsonSerializer.Deserialize<RegisterAPI.Domain.User>(stream, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

    //     httpResult.Headers.Location.Should().Be(user.Id);

    //     user.UserName.Should().Be(createUserRequest.UserName);
    //     user.FirstName.Should().Be(createUserRequest.FirstName);
    //     user.LastName.Should().Be(createUserRequest.LastName);
    //     user.Email.Should().Be(createUserRequest.Email);
    //     user.PhoneNumber.Should().Be(createUserRequest.PhoneNumber);
    //     user.IsLocked.Should().Be(false);
    //     user.IsEmailConfirmed.Should().Be(false);
    // }
}