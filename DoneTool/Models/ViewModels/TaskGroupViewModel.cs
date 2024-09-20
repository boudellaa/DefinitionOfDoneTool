namespace DoneTool.Models.ViewModels
{
    public class TaskGroupViewModel
    {
        public TaskViewModel Original { get; set; }
        public List<TaskViewModel> Duplicates { get; set; }
    }
}
