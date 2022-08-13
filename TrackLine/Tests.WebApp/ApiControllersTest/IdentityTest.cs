using System.Text;
using System.Text.Json;
using App.Public.DTO.v1.Identity;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Manage.Internal;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Build.Framework;
using NuGet.Protocol;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Tests.WebApp.ApiControllersTest;

public class IdentityTest : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly ITestOutputHelper _testOutputHelper;

    public IdentityTest(CustomWebApplicationFactory<Program> factory,
        ITestOutputHelper testOutputHelper)
    {
        _factory = factory;
        _testOutputHelper = testOutputHelper;
        _client = _factory.CreateClient(
            new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            }
        );
    }
    
    private readonly RegistrationModel _registrationInfo = new RegistrationModel
    {
        Email = "pipiund@kaki.land",
        Username = "Rei_is_Cool",
        Password = "FeetArEcooL1."
    };

    private readonly LoginModel _loginInfoEmail = new LoginModel()
    {
        EmailOrUsername = "pipiund@kaki.land",
        Password = "FeetArEcooL1.",
        RememberMe = true
    };

    private readonly LoginModel _loginIngoUsername = new LoginModel
    {
        EmailOrUsername = "Rei_is_Cool",
        Password = "FeetArEcooL1.",
        RememberMe = true
    };
    
    private RegistrationModel RegistrationModelBuilder(string email, string username, string password)
    {
        return new RegistrationModel
        {
            Email = email,
            Username = username,
            Password = password
        };
    }

    private async Task<bool> AccountRegister(RegistrationModel registrationData)
    {   
        var logToJson = JsonSerializer.Serialize(registrationData);
        var jsonData = new StringContent(logToJson, Encoding.UTF8, "application/json");

        var request = await _client.PostAsync("/api/v1/identity/Account/Register/", jsonData);

        return request.IsSuccessStatusCode;
    }

    [Fact]
    public async Task HappyAccountRegister()
    {
        // ARRANGE
        var registraionInfo = new RegistrationModel
        {
            Email = "test34@test34.test",
            Username = "test123",
            Password = "Password.1"
        };
        var jsonInfo = JsonSerializer.Serialize(registraionInfo);
        var jsonData = new StringContent(jsonInfo, Encoding.UTF8, "application/json");

        // ACT
        var request = await _client.PostAsync("/api/v1/identity/Account/Register/", jsonData);
        _testOutputHelper.WriteLine(request.ToJson());
        
        // ASSERT
        request.EnsureSuccessStatusCode();

        var requestContent = await request.Content.ReadAsStringAsync();
        var resultJwt = System.Text.Json.JsonSerializer.Deserialize<JwtResponse>(requestContent,
            new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        _testOutputHelper.WriteLine(request.ToJson());
        Assert.NotNull(resultJwt);
        Assert.NotEqual("", resultJwt!.JWT);
        Assert.NotEqual("", resultJwt!.RefreshToken);
        Assert.NotEqual("", resultJwt!.Username);
        Assert.Equal(registraionInfo.Username, resultJwt!.Username);
    }

    [Fact]
    public async void HappyUserAuthName()
    {
        // ARRANGE
        var usernameLoginModel = new LoginModel
        {
            EmailOrUsername = "test1123123",
            Password = "Password.1",
            RememberMe = false
        };

        var registerInfo = RegistrationModelBuilder("test1333333@test.com", usernameLoginModel.EmailOrUsername,
            usernameLoginModel.Password);
        
        // Try register
        Assert.True(await AccountRegister(registerInfo));
        
        var logToJson = JsonSerializer.Serialize(usernameLoginModel);
        var jsonData = new StringContent(logToJson, Encoding.UTF8, "application/json");
    
        // ACT
        var request = await _client.PostAsync("/api/v1/identity/Account/LogIn", jsonData);
        _testOutputHelper.WriteLine(request.Content.ToJson());
        // ASSERT
        request.EnsureSuccessStatusCode();
        var requestContent = await request.Content.ReadAsStringAsync();
        var resultJwt = System.Text.Json.JsonSerializer.Deserialize<JwtResponse>(requestContent,
            new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        Assert.NotNull(resultJwt);
        Assert.NotEqual("", resultJwt!.JWT);
        Assert.NotEqual("", resultJwt!.RefreshToken);
        Assert.NotEqual("", resultJwt!.Username);
        Assert.Equal(usernameLoginModel.EmailOrUsername, resultJwt!.Username);
    }

    [Fact]
    public async void HappyUserAuthEmail()
    {
        // ARRANGE
        var usernameLoginModel = new LoginModel
        {
            EmailOrUsername = "test2@test2.test",
            Password = "Password.1",
            RememberMe = false
        };

        var registerInfo = RegistrationModelBuilder(usernameLoginModel.EmailOrUsername, "test2",
            usernameLoginModel.Password);
        
        // Try register
        Assert.True(await AccountRegister(registerInfo)); 
        
        var logToJson = JsonSerializer.Serialize(usernameLoginModel);
        var jsonData = new StringContent(logToJson, Encoding.UTF8, "application/json");
        
        // ACT
        var request = await _client.PostAsync("/api/v1/identity/Account/LogIn", jsonData);

        // ASSERT
        request.EnsureSuccessStatusCode();

        var requestContent = await request.Content.ReadAsStringAsync();
        var resultJwt = System.Text.Json.JsonSerializer.Deserialize<JwtResponse>(requestContent,
            new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        Assert.NotNull(resultJwt);
        Assert.NotEqual("", resultJwt!.JWT);
        Assert.NotEqual("", resultJwt!.RefreshToken);
        Assert.NotEqual("", resultJwt!.Username);
        Assert.NotEqual(usernameLoginModel.EmailOrUsername, resultJwt!.Username);
    }


    [Fact]
    public async void RefreshJwtToken()// needs to be heavily optimised
    {
        var logToJson = JsonSerializer.Serialize(_registrationInfo); 
        var jsonData = new StringContent(logToJson, Encoding.UTF8, "application/json");
        var request = await _client.PostAsync("/api/v1/identity/Account/Register/", jsonData);

        request.EnsureSuccessStatusCode();

        var requestContent = await request.Content.ReadAsStringAsync();
        var resultJwt = System.Text.Json.JsonSerializer.Deserialize<JwtResponse>(requestContent,
            new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        var refreshTokenModel = new RefreshTokenModel
        {
            Jwt = resultJwt!.JWT,
            RefreshToken = resultJwt!.RefreshToken
        };

        var tokensToJson = JsonSerializer.Serialize(refreshTokenModel); 
        var tokensJsonData = new StringContent(tokensToJson, Encoding.UTF8, "application/json");
        var refreshRequest = await _client.PostAsync("/api/v1/identity/Account/RefreshToken", tokensJsonData);
        var refreshRequestContent = await refreshRequest.Content.ReadAsStringAsync();
        var refreshedJwt = System.Text.Json.JsonSerializer.Deserialize<JwtResponse>(refreshRequestContent,
            new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        
        refreshRequest.EnsureSuccessStatusCode();
        
        Assert.NotEqual(resultJwt!.JWT,refreshedJwt!.JWT);
        
    }
}