namespace View.Models
{
    public class UserLogedinModel
    {
        public string username { get; set; }
        public List<string> Images { get; set; }
        public int MaxImageCount = 9;
    }
}
