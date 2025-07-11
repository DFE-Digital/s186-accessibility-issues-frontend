using System.ComponentModel.DataAnnotations;

namespace S186Statements.Web.Models
{
    public class StatementSetting
    {
        public int Id { get; set; }

        public string? Setting { get; set; }

        public string? Value { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? PublishedAt { get; set; }

        // Navigation properties
        public int? ServiceId { get; set; }
        public Service? Service { get; set; }
    }
}