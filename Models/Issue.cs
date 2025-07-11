using System.ComponentModel.DataAnnotations;

namespace S186Statements.Web.Models
{
    public enum IssueState
    {
        Open,
        Closed
    }
    
    public enum IssueSource
    {
        ManualTesting,
        AutomatedTesting,
        Audit,
        Complaint,
        Feedback
    }
    
    public enum IssueHowAdded
    {
        API,
        Import,
        Manual
    }
    
    public class Issue
    {
        public int Id { get; set; }
        
        [Required]
        public string Title { get; set; } = string.Empty;
        
        public string? Description { get; set; }
        
        [Required]
        public IssueState State { get; set; } = IssueState.Open;
        
        public IssueSource? Source { get; set; }
        
        public DateTime? DateIdentified { get; set; }
        
        public bool? PlanToFix { get; set; }
        
        public DateTime? PlanToFixDate { get; set; }
        
        public string? ReasonForNotFixing { get; set; }
        
        public IssueHowAdded? HowAdded { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? PublishedAt { get; set; }
        
        // Navigation properties
        public int? ServiceId { get; set; }
        public Service? Service { get; set; }
        
        public int? AssignedToId { get; set; }
        public User? AssignedTo { get; set; }
        
        public ICollection<IssueComment> IssueComments { get; set; } = new List<IssueComment>();
    }
} 