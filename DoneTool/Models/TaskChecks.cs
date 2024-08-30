using System.ComponentModel.DataAnnotations;

namespace DoneTool.Models
{
    public class TaskChecks
    {
        [Key]
        public Guid ID { get; set; }
        public int Step { get; set; }
        public string TaskType { get; set; }
        public Guid CheckID { get; set; }
    }
}
