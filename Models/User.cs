using System.ComponentModel.DataAnnotations;

namespace S186Statements.Web.Models
{
    public class User
    {
        public int Id { get; set; }

        public string? Username { get; set; }

        public string? Email { get; set; }

        public string? Provider { get; set; }

        public string? Password { get; set; }

        public string? ResetPasswordToken { get; set; }

        public string? ConfirmationToken { get; set; }

        public bool? Confirmed { get; set; }

        public bool? Blocked { get; set; }

        public string? Role { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public ICollection<Issue> AssignedIssues { get; set; } = new List<Issue>();
        public ICollection<IssueComment> AuthoredComments { get; set; } = new List<IssueComment>();
    }
}