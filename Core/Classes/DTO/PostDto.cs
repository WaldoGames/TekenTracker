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
        public int postId { get; set; }
        public string title { get; set; }
        public string mainImageUrl { get; set; }
        public DateTime postDate { get; set; }
        public List<Note>? notes { get; set; }
        public List<SubImage>? subImages { get; set; }

        public List<Tag>? Tags { get; set; }
        public List<Tag>? RemovedTags { get; set; }
    }
}
