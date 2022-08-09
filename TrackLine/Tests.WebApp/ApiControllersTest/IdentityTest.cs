using System.Text;
using System.Text.Json;
using App.Public.DTO.v1.Identity;
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

    public LoginModel loginfo = new LoginModel()
            {
                EmailOrUsername = "pipiund@kaki.land",
                Password = "FeetArEcooL1."
            };

    [Fact]
    public async Task HappyAccountRegister()
    {

        var logToJson = JsonSerializer.Serialize(loginfo); 
        var jsonData = new StringContent(logToJson, Encoding.UTF8, "application/json");

        var request = await _client.PostAsync("/api/v1/identity/Account/Register/", jsonData);

        request.EnsureSuccessStatusCode();

        var requestContent = await request.Content.ReadAsStringAsync();
        var resultJwt = System.Text.Json.JsonSerializer.Deserialize<JwtResponse>(requestContent,
            new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        Assert.NotNull(resultJwt);
    }

    [Fact]
    public async void HappyUserAuth()// needs to be optimised
    {
        await HappyAccountRegister();
        var logToJson = JsonSerializer.Serialize(loginfo);
        var jsonData = new StringContent(logToJson, Encoding.UTF8, "application/json");

        var request = await _client.PostAsync("/api/v1/identity/Account/LogIn", jsonData);

        request.EnsureSuccessStatusCode();

        var requestContent = await request.Content.ReadAsStringAsync();
        var resultJwt = System.Text.Json.JsonSerializer.Deserialize<JwtResponse>(requestContent,
            new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        Assert.NotNull(resultJwt);
    }

    [Fact]
    public async void RefreshJwtToken()// needs to be heavily optimised
    {
        var logToJson = JsonSerializer.Serialize(loginfo); 
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
        
        Assert.NotEqual(refreshedJwt!.JWT,resultJwt!.JWT);
        
    }
}