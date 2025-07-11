using System.ComponentModel.DataAnnotations;

namespace S186Statements.Web.Models
{
    public enum ConformanceLevel
    {
        AA,
        AAA
    }

    public enum WcagVersion
    {
        WCAG21,
        WCAG22,
        WCAG30
    }

    public class StatementTemplate
    {
        public int Id { get; set; }

        public string? Type { get; set; }

        public string? Content { get; set; }

        public ConformanceLevel? ConformanceLevel { get; set; }

        public WcagVersion? WcagVersion { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? PublishedAt { get; set; }
    }
}