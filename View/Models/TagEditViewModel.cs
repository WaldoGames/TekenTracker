using Core.Classes.Models;
using Microsoft.AspNetCore.Mvc;

namespace View.Models
{
    public class TagEditViewModel
    {
        public int PostId { get; set; }

        public List<Tag> Tags { get; set; }
        public List<int> TagIds { get; set; }
    }
}
