using Core.Classes.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace View.Models
{
    public class NewTagViewModel
    {
        public string Title { get; set; }

        [Display(Name = "tag type:")]
        public TagTypes TagType { get; set; }
        public IEnumerable<TagTypes> TagsList { get; set; }

        public int ReturnPostId { get; set; }
    }
}
