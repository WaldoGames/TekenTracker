using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Classes.DTO
{
    public class GetTagsDto
    {
        public int Count { get; set; }
        public int Offset { get; set; }
        public string? SearchTerm { get; set; }
    }
}
