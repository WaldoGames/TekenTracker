using Core.Classes.DTO;
using Core.Classes.Models;

namespace View.Models
{
    public class MainPageViewModel
    {
        public List<PostDto> posts {  get; set; }
        
        public List<int> usedTags { get; set; }

        public int userId { get; set; }
    }
}
