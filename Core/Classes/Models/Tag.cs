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
        public int TagId { get; set; }
        public string Name { get; set; }
        public TagTypes Type { get; set; }
    }
}
