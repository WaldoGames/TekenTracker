using Core.Classes.DTO;
using Core.Classes.Models;

namespace View.Models
{
    public class NewPostViewModel
    {
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string UserNamePoster { get; set; }
        public List<int>? SelectedTags { get; set; }

        public List<NewSubimageDto>? SubImages { get; set; }
        public List<NewNoteDto>? Notes { get; set; }
    }
}
