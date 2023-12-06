using Core.Classes.DTO;
using Core.Classes.Models;

namespace View.Models
{
    public class ImprovementViewModel
    {
        public List<ImprovementSearchLimit> SearchLimits { get; set; }
        public List<TagAndAmount> Returned { get; set; } = new List<TagAndAmount>();
    }
}
