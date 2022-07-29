using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using App.Public.DTO.v1.Identity;
using App.Public.DTO.v1.List;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Tests.WebApp.Helpers;
using Xunit;
using Xunit.Abstractions;

namespace Tests.WebApp.ApiControllersTest;

public class HeadListControllerTest: IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly ITestOutputHelper _testOutputHelper;

    public HeadListControllerTest(CustomWebApplicationFactory<Program> factory,
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
    [Fact]
    public async Task Api_Get_HeadList()
    {
        // ========== LOGIN ===========
        // ARRANGE
        var loginDto = new LoginModel()
        {
            Email = "admin@itcollege.ee",
            Password = "Password.1"
        };

        var jsonStr = JsonSerializer.Serialize(loginDto);
        var data = new StringContent(jsonStr, Encoding.UTF8, "application/json");
        
        // ACT
        var response = await _client.PostAsync("/api/v1/identity/Account/Login/", data);
        
        // ASSERT
        response.EnsureSuccessStatusCode();
        
        var requestContent = await response.Content.ReadAsStringAsync();

        var resultJwt = System.Text.Json.JsonSerializer.Deserialize<JwtResponse>(
            requestContent,
            new JsonSerializerOptions() {PropertyNamingPolicy = JsonNamingPolicy.CamelCase}
        );
        
        Assert.NotNull(resultJwt);

        // ========== Get HeadLists ===========
        
        // ARRANGE
        var url = "api/v1/list/HeadList/GetHeadLists";
        
        var apiRequest = new HttpRequestMessage();
        apiRequest.Method = HttpMethod.Get;
        apiRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        apiRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", resultJwt!.Token);
        apiRequest.RequestUri = new Uri(_client.BaseAddress + url);
        
        // ACT
        var apiResponse = await _client.SendAsync(apiRequest);
        
        // ASSERT
        apiResponse.EnsureSuccessStatusCode();
        var item = await apiResponse.Content.ReadAsStringAsync();
        _testOutputHelper.WriteLine(item);

        var headListsResult = System.Text.Json.JsonSerializer.Deserialize<HeadList[]>(item);
        

        Assert.NotNull(headListsResult);
        Assert.True(headListsResult!.Any());
        Assert.Matches(headListsResult!.FirstOrDefault()!.DefaultTitle, "test123");
    }

}