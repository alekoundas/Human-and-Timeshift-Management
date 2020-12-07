namespace DataAccess.Models.Identity
{
    public class ApplicationUserTag
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public int ApplicationTagId { get; set; }
        public ApplicationTag ApplicationTag { get; set; }
    }
}
