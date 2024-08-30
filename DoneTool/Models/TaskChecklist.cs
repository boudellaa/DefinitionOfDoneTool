using System.ComponentModel.DataAnnotations;

namespace DoneTool.Models
{
    public class TaskChecklist
    {
        [Key]
        public Guid ID { get; set; }
        public int TaskID { get; set; }
        public TaskStatus Status { get; set; }
        public string? Comment { get; set; }
        public string Guard { get; set; }
        public Guid TaskChecksID { get; set; }
    }
}
