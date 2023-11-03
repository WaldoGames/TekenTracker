using Core.Classes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Classes.DTO
{
    public class NewPostDto
    {
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public User Poster { get; set; }
        public List<Tag>? Tags { get; set; }

        public List<NewSubimageDto>? SubImages { get; set; }
        public List<NewNoteDto>? Notes { get; set; }
    }
}
