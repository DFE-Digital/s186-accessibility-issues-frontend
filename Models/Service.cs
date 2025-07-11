using System.ComponentModel.DataAnnotations;

namespace S186Statements.Web.Models
{
    public class Service
    {
        public int Id { get; set; }

        [Required]
        public long ServiceId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string? FipsId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? PublishedAt { get; set; }

        // Navigation properties
        public ICollection<ServiceUrl> ServiceUrls { get; set; } = new List<ServiceUrl>();
        public ICollection<Issue> Issues { get; set; } = new List<Issue>();
        public ICollection<StatementSetting> StatementSettings { get; set; } = new List<StatementSetting>();
    }
}