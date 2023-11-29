using Core.Classes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Classes.DTO
{
    public class OverviewManyPostsDto
    {
        public List<PostDto> Posts { get; set; }=new List<PostDto>();
        public List<int>? UsedTags { get; set; }
    }
}
