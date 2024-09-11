namespace DoneTool.Models.SkylineApiModels
{
    public class TaskResponse
    {
        public string Title { get; set; }
        public string Type { get; set; }
        public PersonReference Assignee { get; set; }
        public PersonReference Developer { get; set; }
        public PersonReference ProjectsSkylinePM { get; set; }
        public string IntegrationID { get; set; }
        public PersonReference ProjectsSkylineTam { get; set; }
        public PersonReference Creator { get; set; }
    }
}
