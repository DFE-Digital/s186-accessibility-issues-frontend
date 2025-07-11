using System.ComponentModel.DataAnnotations;

namespace S186Statements.Web.Models
{
    public class ServiceUrl
    {
        public int Id { get; set; }

        public string? Url { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? PublishedAt { get; set; }

        // Navigation properties
        public int? ServiceId { get; set; }
        public Service? Service { get; set; }
    }
}