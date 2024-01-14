namespace View.Models
{
    public class NewMainImageModel
    {
        public IFormFile Image {  get; set; }

        public bool MoveOldImageToSubimage { get; set; }

        public string ErrorMessage { get; set; }
    }
}
