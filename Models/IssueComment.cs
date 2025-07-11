using System.ComponentModel.DataAnnotations;

namespace S186Statements.Web.Models
{
    public class IssueComment
    {
        public int Id { get; set; }

        public string? Content { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? PublishedAt { get; set; }

        // Navigation properties
        public int? IssueId { get; set; }
        public Issue? Issue { get; set; }

        public int? AuthorId { get; set; }
        public User? Author { get; set; }
    }
}