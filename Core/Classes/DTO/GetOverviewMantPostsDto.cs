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
        public List<int>? Tags;
        public bool IsOneTagEnough = false;

        public int UserId { get; set; }
        public int Count { get; set; } = 0;
        public int Offset { get; set; } = 0;
    }
}
