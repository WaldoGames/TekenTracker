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
        public int PostId { get; set; }
        public string Title { get; set; }

        public string MainImageUrl { get; set; }

        public DateTime PostDate { get; set; }

        public int User_Id { get; set; }

        public List<Note>? Notes { get; set; }
        public List<SubImage>? SubImages { get; set; }

        public List<Tag> Tags { get; set; }

        public PostDto GetPost()
        {
            PostDto dto = new PostDto();
            dto.PostId = PostId;
            dto.Title = Title;
            dto.MainImageUrl = MainImageUrl;
            dto.PostDate = PostDate;
            dto.Notes = Notes;
            dto.SubImages = SubImages;

            return dto;
        }
    }
}
