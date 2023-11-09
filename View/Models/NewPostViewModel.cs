using Core.Classes.DTO;
using Core.Classes.Models;

namespace View.Models
{
    public class NewPostViewModel
    {
        public string Title { get; set; }
        public IFormFile? image { get; set; }
        public List<int>? SelectedTags { get; set; }

        public List<IFormFile>? subimages { get; set; }
        public string Note {  get; set; }
    }
}
