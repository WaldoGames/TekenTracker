using Core.Classes.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Classes.Models
{
    public class Post
    {
        public int postId { get; set; }
        public string title { get; set; }

        public string mainImageUrl { get; set; }

        public DateTime postDate { get; set; }

        public int user_Id { get; set; }

        public List<Note>? notes { get; set; }
        public List<SubImage>? subImages { get; set; }

        public List<Tag> tags { get; set; }

        public PostDto GetPost()
        {
            PostDto dto = new PostDto();
            dto.postId = postId;
            dto.title = title;
            dto.mainImageUrl = mainImageUrl;
            dto.postDate = postDate;
            dto.notes = notes;
            dto.subImages = subImages;

            return dto;
        }
    }
}
