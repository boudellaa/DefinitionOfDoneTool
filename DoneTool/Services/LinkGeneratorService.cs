namespace DoneTool.Services
{
    public class LinkGeneratorService
    {
        public string GenerateStepLink(string step)
        {
            return step switch
            {
                // default step
                "Reproduce and Isolate the Issue" => "https://www.google.com/",
                _ => "https://skylinebe.sharepoint.com/sites/DeployandAccelerate/EcsProductsNotes/Shared%20Documents/Forms/AllItems.aspx?viewpath=%2Fsites%2FDeployandAccelerate%2FEcsProductsNotes%2FShared%20Documents%2FForms%2FAllItems%2Easpx&id=%2Fsites%2FDeployandAccelerate%2FEcsProductsNotes%2FShared%20Documents%2FNotebooks&viewid=7ec3d9b3%2D4c67%2D4ac9%2Db72b%2D1a46fb69115c"
            };
        }
    }
}
