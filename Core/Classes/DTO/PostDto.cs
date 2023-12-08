using Core.Classes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Classes.DTO
{
    public class PostDto
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string MainImageUrl { get; set; }
        public DateTime PostDate { get; set; }
        public List<Note>? Notes { get; set; }
        public List<SubImage>? SubImages { get; set; }

        public List<Tag>? Tags { get; set; } = new List<Tag>();
        public List<Tag>? RemovedTags { get; set; }
    }
}
