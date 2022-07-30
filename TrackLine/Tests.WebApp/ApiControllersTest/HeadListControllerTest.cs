using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using App.Public.DTO.v1;
using App.Public.DTO.v1.Identity;
using App.Public.DTO.v1.List;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;
using Tests.WebApp.Helpers;
using Xunit;
using Xunit.Abstractions;
using JsonSerializer = System.Text.Json.JsonSerializer;

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
    public async Task Api_Get_HeadLists()
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

        var headListsResult =  JsonConvert.DeserializeObject<List<HeadList>>(item);
        
        Assert.NotNull(headListsResult);
        Assert.NotEmpty(headListsResult);
        Assert.NotNull(headListsResult!.FirstOrDefault()!.DefaultTitle);
        Assert.NotEqual(headListsResult!.FirstOrDefault()!.Id, Guid.Empty);
        Assert.Matches(headListsResult!.FirstOrDefault()!.DefaultTitle, "test123");
    }

    async Task Api_Post_HeadList()
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
        var url = "api/v1/list/HeadList/PostHeadList";

        var headList = new HeadList
        {
            DefaultTitle = "created title"
        };

        var apiRequest = new HttpRequestMessage();
        apiRequest.Method = HttpMethod.Post;
        apiRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        apiRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", resultJwt!.Token);
        apiRequest.RequestUri = new Uri(_client.BaseAddress + url);
        apiRequest.Content = JsonContent.Create(headList);

        
        // ACT
        var apiResponse = await _client.SendAsync(apiRequest);
        
        // ASSERT
        apiResponse.EnsureSuccessStatusCode();
        var item = await apiResponse.Content.ReadAsStringAsync();

        var headListsResult =  JsonConvert.DeserializeObject<HeadList>(item);
        
        Assert.NotNull(headListsResult);
        Assert.NotNull(headListsResult!.DefaultTitle);
        Assert.NotEqual(headListsResult!.Id, Guid.Empty);
        Assert.NotNull(headListsResult!.AppUserId);
        Assert.Matches(headListsResult!.DefaultTitle, "created title");
    }

}