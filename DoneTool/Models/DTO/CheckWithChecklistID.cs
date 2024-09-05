namespace DoneTool.Models.DTO
{
    using DoneTool.Models.Domain;

    public class CheckWithChecklistID
    {
        public Checks Check { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the task checklist.
        /// </summary>
        public Guid TaskChecklistID { get; set; }
    }
}
