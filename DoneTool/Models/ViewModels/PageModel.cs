using DoneTool.Models.DTO;

namespace DoneTool.Models.ViewModels
{
    public class PageModel
    {
        public string TaskTitle { get; set; }
        public string DeveloperName { get; set; }
        public List<TaskViewModel> Checks { get; set; }
        public List<SkylineUser> Guards { get; set; }
    }
}
