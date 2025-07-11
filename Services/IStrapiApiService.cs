using S186Statements.Web.Models;

namespace S186Statements.Web.Services
{
    public interface IStrapiApiService
    {
        // Service endpoints
        Task<IEnumerable<Service>> GetServicesAsync();
        Task<Service?> GetServiceAsync(int id);
        Task<Service> CreateServiceAsync(Service service);
        Task<Service> UpdateServiceAsync(int id, Service service);
        Task DeleteServiceAsync(int id);

        // Issue endpoints
        Task<IEnumerable<Issue>> GetIssuesAsync();
        Task<Issue?> GetIssueAsync(int id);
        Task<Issue> CreateIssueAsync(Issue issue);
        Task<Issue> UpdateIssueAsync(int id, Issue issue);
        Task DeleteIssueAsync(int id);

        // Statement Template endpoints
        Task<IEnumerable<StatementTemplate>> GetStatementTemplatesAsync();
        Task<StatementTemplate?> GetStatementTemplateAsync(int id);
        Task<StatementTemplate> CreateStatementTemplateAsync(StatementTemplate template);
        Task<StatementTemplate> UpdateStatementTemplateAsync(int id, StatementTemplate template);
        Task DeleteStatementTemplateAsync(int id);

        // Statement Setting endpoints
        Task<IEnumerable<StatementSetting>> GetStatementSettingsAsync();
        Task<StatementSetting?> GetStatementSettingAsync(int id);
        Task<StatementSetting> CreateStatementSettingAsync(StatementSetting setting);
        Task<StatementSetting> UpdateStatementSettingAsync(int id, StatementSetting setting);
        Task DeleteStatementSettingAsync(int id);

        // Service URL endpoints
        Task<IEnumerable<ServiceUrl>> GetServiceUrlsAsync();
        Task<ServiceUrl?> GetServiceUrlAsync(int id);
        Task<ServiceUrl> CreateServiceUrlAsync(ServiceUrl serviceUrl);
        Task<ServiceUrl> UpdateServiceUrlAsync(int id, ServiceUrl serviceUrl);
        Task DeleteServiceUrlAsync(int id);

        // Issue Comment endpoints
        Task<IEnumerable<IssueComment>> GetIssueCommentsAsync();
        Task<IssueComment?> GetIssueCommentAsync(int id);
        Task<IssueComment> CreateIssueCommentAsync(IssueComment comment);
        Task<IssueComment> UpdateIssueCommentAsync(int id, IssueComment comment);
        Task DeleteIssueCommentAsync(int id);

        // User endpoints
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User?> GetUserAsync(int id);
        Task<(User? User, int? Id)> GetUserByEmailAsync(string email);
        Task<User> CreateUserAsync(User user);
        Task<User> UpdateUserAsync(int id, User user);
        Task DeleteUserAsync(int id);
        Task<List<User>> GetAllUsersAsync();
    }
}