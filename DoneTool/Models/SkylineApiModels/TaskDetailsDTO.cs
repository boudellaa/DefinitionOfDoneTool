namespace DoneTool.Models.SkylineApiModels
{
    public class TaskDetailsDTO
    {
        public string Title { get; set; }
        public string Type { get; set; }
        public string AssigneeName { get; set; }
        public string DeveloperName { get; set; }
        public string IntegrationID { get; set; }
        public string TamName { get; set; }
        public string CreatorName { get; set; }
        public string ProductOwnerName { get; set; }
        public List<string> CodeOwnerNames { get; set; }
    }
}
