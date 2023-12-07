using Core.Classes.DTO;
using Core.Classes.Models;

namespace View.Models
{
    public class ImprovementViewModel
    {
        public ImprovementSearchLimit SearchLimits { get; set; }
        public List<TagAndAmount> Returned { get; set; } = new List<TagAndAmount>();

        public List<TimeOrAmount> TimeOrAmountEnum = new List<TimeOrAmount>() { TimeOrAmount.time, TimeOrAmount.amount };
    }
}
