namespace DoneTool.Models.SkylineApiModels
{
    public class DriverResponse
    {
        public PersonReference ProductOwner { get; set; }
        public List<PersonReference> CodeOwner { get; set; }
        public PersonReference Creator { get; set; }
    }

}
