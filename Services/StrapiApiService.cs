using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using S186Statements.Web.Models;

namespace S186Statements.Web.Services
{
    public class StrapiApiService : IStrapiApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly JsonSerializerOptions _jsonOptions;

        public StrapiApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            };

            // Configure the base address for Strapi API
            var strapiBaseUrl = _configuration["Strapi:BaseUrl"] ?? "http://localhost:1337";
            _httpClient.BaseAddress = new Uri(strapiBaseUrl);

            // Add API token if configured
            var apiToken = _configuration["Strapi:ApiToken"];
            if (!string.IsNullOrEmpty(apiToken))
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiToken}");
            }
        }

        #region Service Methods
        public async Task<IEnumerable<Service>> GetServicesAsync()
        {
            var response = await _httpClient.GetAsync("/api/services?populate=*");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<StrapiResponse<Service>>(content, _jsonOptions);

            return result?.Data?.Select(d => d.Attributes) ?? Enumerable.Empty<Service>();
        }

        public async Task<Service?> GetServiceAsync(int id)
        {
            var response = await _httpClient.GetAsync($"/api/services/{id}?populate=*");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<StrapiSingleResponse<Service>>(content, _jsonOptions);
                return result?.Data?.Attributes;
            }
            return null;
        }

        public async Task<Service> CreateServiceAsync(Service service)
        {
            var requestData = new { data = service };
            var json = JsonSerializer.Serialize(requestData, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/services", content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<StrapiSingleResponse<Service>>(responseContent, _jsonOptions);

            return result?.Data?.Attributes ?? service;
        }

        public async Task<Service> UpdateServiceAsync(int id, Service service)
        {
            var requestData = new { data = service };
            var json = JsonSerializer.Serialize(requestData, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"/api/services/{id}", content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<StrapiSingleResponse<Service>>(responseContent, _jsonOptions);

            return result?.Data?.Attributes ?? service;
        }

        public async Task DeleteServiceAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/services/{id}");
            response.EnsureSuccessStatusCode();
        }
        #endregion

        #region Issue Methods
        public async Task<IEnumerable<Issue>> GetIssuesAsync()
        {
            var response = await _httpClient.GetAsync("/api/issues?populate=*");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<StrapiResponse<Issue>>(content, _jsonOptions);

            return result?.Data?.Select(d => d.Attributes) ?? Enumerable.Empty<Issue>();
        }

        public async Task<Issue?> GetIssueAsync(int id)
        {
            var response = await _httpClient.GetAsync($"/api/issues/{id}?populate=*");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<StrapiSingleResponse<Issue>>(content, _jsonOptions);
                return result?.Data?.Attributes;
            }
            return null;
        }

        public async Task<Issue> CreateIssueAsync(Issue issue)
        {
            var requestData = new { data = issue };
            var json = JsonSerializer.Serialize(requestData, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/issues", content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<StrapiSingleResponse<Issue>>(responseContent, _jsonOptions);

            return result?.Data?.Attributes ?? issue;
        }

        public async Task<Issue> UpdateIssueAsync(int id, Issue issue)
        {
            var requestData = new { data = issue };
            var json = JsonSerializer.Serialize(requestData, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"/api/issues/{id}", content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<StrapiSingleResponse<Issue>>(responseContent, _jsonOptions);

            return result?.Data?.Attributes ?? issue;
        }

        public async Task DeleteIssueAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/issues/{id}");
            response.EnsureSuccessStatusCode();
        }
        #endregion

        #region Statement Template Methods
        public async Task<IEnumerable<StatementTemplate>> GetStatementTemplatesAsync()
        {
            var response = await _httpClient.GetAsync("/api/statement-templates?populate=*");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<StrapiResponse<StatementTemplate>>(content, _jsonOptions);

            return result?.Data?.Select(d => d.Attributes) ?? Enumerable.Empty<StatementTemplate>();
        }

        public async Task<StatementTemplate?> GetStatementTemplateAsync(int id)
        {
            var response = await _httpClient.GetAsync($"/api/statement-templates/{id}?populate=*");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<StrapiSingleResponse<StatementTemplate>>(content, _jsonOptions);
                return result?.Data?.Attributes;
            }
            return null;
        }

        public async Task<StatementTemplate> CreateStatementTemplateAsync(StatementTemplate template)
        {
            var requestData = new { data = template };
            var json = JsonSerializer.Serialize(requestData, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/statement-templates", content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<StrapiSingleResponse<StatementTemplate>>(responseContent, _jsonOptions);

            return result?.Data?.Attributes ?? template;
        }

        public async Task<StatementTemplate> UpdateStatementTemplateAsync(int id, StatementTemplate template)
        {
            var requestData = new { data = template };
            var json = JsonSerializer.Serialize(requestData, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"/api/statement-templates/{id}", content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<StrapiSingleResponse<StatementTemplate>>(responseContent, _jsonOptions);

            return result?.Data?.Attributes ?? template;
        }

        public async Task DeleteStatementTemplateAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/statement-templates/{id}");
            response.EnsureSuccessStatusCode();
        }
        #endregion

        #region Statement Setting Methods
        public async Task<IEnumerable<StatementSetting>> GetStatementSettingsAsync()
        {
            var response = await _httpClient.GetAsync("/api/statement-settings?populate=*");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<StrapiResponse<StatementSetting>>(content, _jsonOptions);

            return result?.Data?.Select(d => d.Attributes) ?? Enumerable.Empty<StatementSetting>();
        }

        public async Task<StatementSetting?> GetStatementSettingAsync(int id)
        {
            var response = await _httpClient.GetAsync($"/api/statement-settings/{id}?populate=*");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<StrapiSingleResponse<StatementSetting>>(content, _jsonOptions);
                return result?.Data?.Attributes;
            }
            return null;
        }

        public async Task<StatementSetting> CreateStatementSettingAsync(StatementSetting setting)
        {
            var requestData = new { data = setting };
            var json = JsonSerializer.Serialize(requestData, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/statement-settings", content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<StrapiSingleResponse<StatementSetting>>(responseContent, _jsonOptions);

            return result?.Data?.Attributes ?? setting;
        }

        public async Task<StatementSetting> UpdateStatementSettingAsync(int id, StatementSetting setting)
        {
            var requestData = new { data = setting };
            var json = JsonSerializer.Serialize(requestData, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"/api/statement-settings/{id}", content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<StrapiSingleResponse<StatementSetting>>(responseContent, _jsonOptions);

            return result?.Data?.Attributes ?? setting;
        }

        public async Task DeleteStatementSettingAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/statement-settings/{id}");
            response.EnsureSuccessStatusCode();
        }
        #endregion

        #region Service URL Methods
        public async Task<IEnumerable<ServiceUrl>> GetServiceUrlsAsync()
        {
            var response = await _httpClient.GetAsync("/api/service-urls?populate=*");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<StrapiResponse<ServiceUrl>>(content, _jsonOptions);

            return result?.Data?.Select(d => d.Attributes) ?? Enumerable.Empty<ServiceUrl>();
        }

        public async Task<ServiceUrl?> GetServiceUrlAsync(int id)
        {
            var response = await _httpClient.GetAsync($"/api/service-urls/{id}?populate=*");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<StrapiSingleResponse<ServiceUrl>>(content, _jsonOptions);
                return result?.Data?.Attributes;
            }
            return null;
        }

        public async Task<ServiceUrl> CreateServiceUrlAsync(ServiceUrl serviceUrl)
        {
            var requestData = new { data = serviceUrl };
            var json = JsonSerializer.Serialize(requestData, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/service-urls", content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<StrapiSingleResponse<ServiceUrl>>(responseContent, _jsonOptions);

            return result?.Data?.Attributes ?? serviceUrl;
        }

        public async Task<ServiceUrl> UpdateServiceUrlAsync(int id, ServiceUrl serviceUrl)
        {
            var requestData = new { data = serviceUrl };
            var json = JsonSerializer.Serialize(requestData, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"/api/service-urls/{id}", content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<StrapiSingleResponse<ServiceUrl>>(responseContent, _jsonOptions);

            return result?.Data?.Attributes ?? serviceUrl;
        }

        public async Task DeleteServiceUrlAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/service-urls/{id}");
            response.EnsureSuccessStatusCode();
        }
        #endregion

        #region Issue Comment Methods
        public async Task<IEnumerable<IssueComment>> GetIssueCommentsAsync()
        {
            var response = await _httpClient.GetAsync("/api/issue-comments?populate=*");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<StrapiResponse<IssueComment>>(content, _jsonOptions);

            return result?.Data?.Select(d => d.Attributes) ?? Enumerable.Empty<IssueComment>();
        }

        public async Task<IssueComment?> GetIssueCommentAsync(int id)
        {
            var response = await _httpClient.GetAsync($"/api/issue-comments/{id}?populate=*");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<StrapiSingleResponse<IssueComment>>(content, _jsonOptions);
                return result?.Data?.Attributes;
            }
            return null;
        }

        public async Task<IssueComment> CreateIssueCommentAsync(IssueComment comment)
        {
            var requestData = new { data = comment };
            var json = JsonSerializer.Serialize(requestData, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/issue-comments", content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<StrapiSingleResponse<IssueComment>>(responseContent, _jsonOptions);

            return result?.Data?.Attributes ?? comment;
        }

        public async Task<IssueComment> UpdateIssueCommentAsync(int id, IssueComment comment)
        {
            var requestData = new { data = comment };
            var json = JsonSerializer.Serialize(requestData, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"/api/issue-comments/{id}", content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<StrapiSingleResponse<IssueComment>>(responseContent, _jsonOptions);

            return result?.Data?.Attributes ?? comment;
        }

        public async Task DeleteIssueCommentAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/issue-comments/{id}");
            response.EnsureSuccessStatusCode();
        }
        #endregion

        #region User Methods
        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            var response = await _httpClient.GetAsync("/api/users?populate=*");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<StrapiResponse<User>>(content, _jsonOptions);

            return result?.Data?.Select(d => d.Attributes) ?? Enumerable.Empty<User>();
        }

        public async Task<User?> GetUserAsync(int id)
        {
            var response = await _httpClient.GetAsync($"/api/users/{id}?populate=*");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<StrapiSingleResponse<User>>(content, _jsonOptions);
                return result?.Data?.Attributes;
            }
            return null;
        }

        public async Task<(User? User, int? Id)> GetUserByEmailAsync(string email)
        {
            try
            {
                // First try exact match
                var url = $"/api/users?filters[email][$eq]={Uri.EscapeDataString(email)}&populate=*";
                Console.WriteLine($"Calling Strapi API: {url}");

                var response = await _httpClient.GetAsync(url);
                Console.WriteLine($"Strapi API response status: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Strapi API response content: {content}");

                    // Try to parse as users-permissions format first (direct array)
                    try
                    {
                        var users = JsonSerializer.Deserialize<List<User>>(content, _jsonOptions);
                        var user = users?.FirstOrDefault();
                        if (user != null)
                        {
                            return (user, user.Id);
                        }
                    }
                    catch
                    {
                        // Fallback to StrapiResponse format
                        var result = JsonSerializer.Deserialize<StrapiResponse<User>>(content, _jsonOptions);
                        var userData = result?.Data?.FirstOrDefault();
                        return (userData?.Attributes, userData?.Id);
                    }
                }

                // If exact match fails, try case-insensitive search by getting all users
                Console.WriteLine("Exact email match failed, trying case-insensitive search...");
                var allUsersUrl = "/api/users?populate=*";
                var allUsersResponse = await _httpClient.GetAsync(allUsersUrl);

                if (allUsersResponse.IsSuccessStatusCode)
                {
                    var allUsersContent = await allUsersResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"All users response: {allUsersContent}");

                    try
                    {
                        var allUsers = JsonSerializer.Deserialize<List<User>>(allUsersContent, _jsonOptions);
                        var matchingUser = allUsers?.FirstOrDefault(u =>
                            string.Equals(u.Email, email, StringComparison.OrdinalIgnoreCase));

                        if (matchingUser != null)
                        {
                            Console.WriteLine($"Found user with case-insensitive match: {matchingUser.Email}");
                            return (matchingUser, matchingUser.Id);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error parsing all users response: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DEBUG] GetUserByEmailAsync exception: {ex.Message}");
            }
            return (null, null);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            try
            {
                // Generate a random password for the user (required by users-permissions plugin)
                var randomPassword = Convert.ToBase64String(System.Security.Cryptography.RandomNumberGenerator.GetBytes(12));

                // Create user data using camelCase field names that Strapi expects
                var createData = new
                {
                    username = user.Username,
                    email = user.Email,
                    password = randomPassword,
                    firstName = user.FirstName,
                    lastName = user.LastName,
                    entraId = user.EntraId,
                    provider = "local",
                    confirmed = true,
                    blocked = false,
                    role = 1 // Role ID 1 is typically "Authenticated" in Strapi
                };

                var json = JsonSerializer.Serialize(createData, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = null, // Use field names as-is since they're already camelCase
                    PropertyNameCaseInsensitive = true
                });
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                Console.WriteLine($"Creating user with JSON: {json}");

                var response = await _httpClient.PostAsync("/api/users", content);
                Console.WriteLine($"Create user response status: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Create user response content: {responseContent}");

                    try
                    {
                        // Try to handle the double data wrapper like in the working app
                        var jsonDoc = JsonDocument.Parse(responseContent);
                        var root = jsonDoc.RootElement;

                        // Check if there's a double data wrapper
                        if (root.TryGetProperty("data", out var dataElement) &&
                            dataElement.TryGetProperty("data", out var innerDataElement))
                        {
                            // Double data wrapper: {"data": {"data": {...}}}
                            var result = JsonSerializer.Deserialize<User>(innerDataElement.GetRawText(), _jsonOptions);
                            if (result != null) return result;
                        }
                        else if (root.TryGetProperty("data", out var singleDataElement))
                        {
                            // Single data wrapper: {"data": {...}}
                            var result = JsonSerializer.Deserialize<User>(singleDataElement.GetRawText(), _jsonOptions);
                            if (result != null) return result;
                        }
                        else
                        {
                            // Direct response: {...}
                            var result = JsonSerializer.Deserialize<User>(responseContent, _jsonOptions);
                            if (result != null) return result;
                        }

                        return user;
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"[DEBUG] Failed to deserialize create user response. Content: {responseContent}");
                        return user;
                    }
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Create user error response: {errorContent}");
                    response.EnsureSuccessStatusCode(); // This will throw an exception
                    return user; // This line won't be reached
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DEBUG] CreateUserAsync exception: {ex.Message}");
                throw;
            }
        }

        public async Task<User> UpdateUserAsync(int id, User user)
        {
            try
            {
                // Create update data using camelCase field names that Strapi expects
                var updateData = new
                {
                    firstName = user.FirstName,
                    lastName = user.LastName,
                    entraId = user.EntraId,
                    email = user.Email
                };

                var json = JsonSerializer.Serialize(updateData, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = null, // Use field names as-is since they're already camelCase
                    PropertyNameCaseInsensitive = true
                });
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                Console.WriteLine($"Updating user {id} with JSON: {json}");

                var response = await _httpClient.PutAsync($"/api/users/{id}", content);
                Console.WriteLine($"Update user response status: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Update user response content: {responseContent}");

                    try
                    {
                        // Try to handle the double data wrapper like in the working app
                        var jsonDoc = JsonDocument.Parse(responseContent);
                        var root = jsonDoc.RootElement;

                        // Check if there's a double data wrapper
                        if (root.TryGetProperty("data", out var dataElement) &&
                            dataElement.TryGetProperty("data", out var innerDataElement))
                        {
                            // Double data wrapper: {"data": {"data": {...}}}
                            var result = JsonSerializer.Deserialize<User>(innerDataElement.GetRawText(), _jsonOptions);
                            if (result != null) return result;
                        }
                        else if (root.TryGetProperty("data", out var singleDataElement))
                        {
                            // Single data wrapper: {"data": {...}}
                            var result = JsonSerializer.Deserialize<User>(singleDataElement.GetRawText(), _jsonOptions);
                            if (result != null) return result;
                        }
                        else
                        {
                            // Direct response: {...}
                            var result = JsonSerializer.Deserialize<User>(responseContent, _jsonOptions);
                            if (result != null) return result;
                        }

                        return user;
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"[DEBUG] Failed to deserialize update user response. Content: {responseContent}");
                        return user;
                    }
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Update user error response: {errorContent}");
                    response.EnsureSuccessStatusCode(); // This will throw an exception
                    return user; // This line won't be reached
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DEBUG] UpdateUserAsync exception: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteUserAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/users/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            try
            {
                var url = "/api/users?populate=*";
                Console.WriteLine($"Calling Strapi API: {url}");

                var response = await _httpClient.GetAsync(url);
                Console.WriteLine($"GetAllUsersAsync response status: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"GetAllUsersAsync response content: {content}");

                    try
                    {
                        var users = JsonSerializer.Deserialize<List<User>>(content, _jsonOptions);
                        return users ?? new List<User>();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error parsing GetAllUsersAsync response: {ex.Message}");
                        return new List<User>();
                    }
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"GetAllUsersAsync error response: {errorContent}");
                    return new List<User>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DEBUG] GetAllUsersAsync exception: {ex.Message}");
                return new List<User>();
            }
        }
        #endregion


    }

    // Helper classes for Strapi API responses
    public class StrapiResponse<T>
    {
        public List<StrapiData<T>>? Data { get; set; }
        public StrapiMeta? Meta { get; set; }
    }

    public class StrapiSingleResponse<T>
    {
        public StrapiData<T>? Data { get; set; }
        public StrapiMeta? Meta { get; set; }
    }

    public class StrapiData<T>
    {
        public int Id { get; set; }
        public T? Attributes { get; set; }
    }

    public class StrapiMeta
    {
        public StrapiPagination? Pagination { get; set; }
    }

    public class StrapiPagination
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public int Total { get; set; }
    }
}