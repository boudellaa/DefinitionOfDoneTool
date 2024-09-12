namespace DoneTool.Models.Domain
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class CheckSkipReason
    {
        [Key]
        public Guid ID { get; set; }

        [ForeignKey("Checks")]
        public Guid CheckID { get; set; }

        [Required]
        [StringLength(255)]
        public string Reason { get; set; }
    }

}
