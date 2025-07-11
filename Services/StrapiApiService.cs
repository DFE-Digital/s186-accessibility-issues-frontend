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