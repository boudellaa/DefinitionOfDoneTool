using DoneTool.Models.SkylineApiModels;

namespace DoneTool.Models.ViewModels
{
    public class PageModel
    {
        public string TaskTitle { get; set; }
        public string DeveloperName { get; set; }
        public List<TaskViewModel> Checks { get; set; }
        public List<string> Guards { get; set; }
        public string TamName { get; set; }
        public string CreatorName { get; set; }
        public string ProductOwnerName { get; set; }
        public List<string> CodeOwnerNames { get; set;}
    }
}
