using System.ComponentModel.DataAnnotations;

namespace DoneTool.Models
{
    public class Checks
    {
        [Key]
        public Guid ID { get; set; }
        public string Item { get; set; }
    }
}
