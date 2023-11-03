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
        public List<PostSimpleDto> Posts { get; set; }
        public List<Tag>? UsedTags { get; set; }
    }
}
