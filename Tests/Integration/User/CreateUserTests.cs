using System.Net;
using System.Text.Json;
using System.Text;
using NUnit.Framework;
using FluentAssertions;
using Bogus;
using Scrypt;
using RegisterAPI.Domain.Requests;
using RegisterAPI.Domain;

namespace IntegrationTests.User;

public class CreateUserTests : IntegrationTestBase
{
    [Test]
    public async Task Should_CreateUser()
    {
        var createUserRequest = new CreateUserRequest
        {
             UserName = Faker.Person.UserName.ToLower(),
             FirstName = Faker.Person.FirstName,
             LastName = Faker.Person.LastName,
             Email = Faker.Person.Email.ToLower(),
             PhoneNumber = Faker.Phone.PhoneNumber("(##) ####-####"),
             Password = Faker.Random.Word()
        };
        
        var httpResult = await RegisterApiHttpClient.PostAsync("users", new StringContent(JsonSerializer.Serialize(createUserRequest), Encoding.UTF8, "application/json"));
        
        httpResult.StatusCode.Should().Be(HttpStatusCode.Created);
        httpResult.Content.Headers.ContentType.CharSet.Should().Be("utf-8");
        httpResult.Content.Headers.ContentType.MediaType.Should().Be("application/json");

        var stream = await httpResult.Content.ReadAsStringAsync();
        var user = JsonSerializer.Deserialize<RegisterAPI.Domain.User>(stream, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        httpResult.Headers.Location.Should().Be($"http://localhost/users/{user.Id}");

        user.UserName.Should().Be(createUserRequest.UserName);
        user.FirstName.Should().Be(createUserRequest.FirstName);
        user.LastName.Should().Be(createUserRequest.LastName);
        user.Email.Should().Be(createUserRequest.Email);
        user.PhoneNumber.Should().Be(createUserRequest.PhoneNumber);
        user.IsLocked.Should().Be(false);
        user.IsEmailConfirmed.Should().Be(false);
        user.Role.Should().Be(Role.User);
        new ScryptEncoder().Compare(user.PasswordSalt + createUserRequest.Password, user.PasswordHash);
    }
}