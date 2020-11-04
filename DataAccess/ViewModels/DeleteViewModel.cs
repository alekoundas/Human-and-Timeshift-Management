namespace DataAccess.ViewModels
{
    public class DeleteViewModel
    {
        public object Entity { get; set; }
        public bool IsSuccessful { get; set; }
        public string ResponseTitle { get; set; }
        public string ResponseBody { get; set; }
    }
}
