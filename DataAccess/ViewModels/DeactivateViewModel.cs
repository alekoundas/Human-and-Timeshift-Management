namespace DataAccess.ViewModels
{
    public class DeactivateViewModel
    {
        public object Entity { get; set; }
        public bool IsSuccessful { get; set; }
        public string ResponseTitle { get; set; }
        public string ResponseBody { get; set; }
    }
}
