using Core.Classes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Classes.DTO
{
    public class GetOverviewMantPostsDto
    {
        List<Tag>? Tags;

        public int userId { get; set; }
        public int count { get; set; }
        public int offset { get; set; }
    }
}
