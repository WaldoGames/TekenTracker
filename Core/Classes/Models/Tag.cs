using Core.Classes.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Classes.Models
{
    public class Tag
    {
        public int tagId { get; set; }
        public string name { get; set; }
        public TagTypes type { get; set; }
    }
}
