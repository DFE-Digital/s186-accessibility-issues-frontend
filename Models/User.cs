using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace S186Statements.Web.Models
{
    public class User
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("documentId")]
        public string? DocumentId { get; set; }

        [JsonPropertyName("username")]
        public string? Username { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("first_name")]
        public string? FirstName { get; set; }

        [JsonPropertyName("last_name")]
        public string? LastName { get; set; }

        [JsonIgnore]
        public string? DisplayName
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(FirstName) && !string.IsNullOrWhiteSpace(LastName))
                {
                    return $"{FirstName} {LastName}";
                }
                else if (!string.IsNullOrWhiteSpace(FirstName))
                {
                    return FirstName;
                }
                else if (!string.IsNullOrWhiteSpace(LastName))
                {
                    return LastName;
                }
                else if (!string.IsNullOrWhiteSpace(Email))
                {
                    // Fallback: Try to extract name from email (e.g., "andy.jones@education.gov.uk" -> "Andy Jones")
                    var emailParts = Email.Split('@')[0];
                    if (emailParts.Contains('.'))
                    {
                        var nameParts = emailParts.Split('.');
                        if (nameParts.Length >= 2)
                        {
                            return $"{char.ToUpper(nameParts[0][0]) + nameParts[0].Substring(1)} {char.ToUpper(nameParts[1][0]) + nameParts[1].Substring(1)}";
                        }
                    }
                    return char.ToUpper(emailParts[0]) + emailParts.Substring(1);
                }
                else if (!string.IsNullOrWhiteSpace(Username))
                {
                    return Username;
                }
                else
                {
                    return "Unknown User";
                }
            }
        }

        [JsonPropertyName("entra_id")]
        public string? EntraId { get; set; }

        [JsonPropertyName("provider")]
        public string? Provider { get; set; }

        [JsonPropertyName("confirmed")]
        public bool Confirmed { get; set; }

        [JsonPropertyName("blocked")]
        public bool Blocked { get; set; }

        [JsonPropertyName("role")]
        public object? Role { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public ICollection<Issue> AssignedIssues { get; set; } = new List<Issue>();
        public ICollection<IssueComment> AuthoredComments { get; set; } = new List<IssueComment>();
    }
}