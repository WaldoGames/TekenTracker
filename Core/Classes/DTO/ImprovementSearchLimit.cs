using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Classes.DTO
{
        public class ImprovementSearchLimit
        {
            public TimeOrAmount TimeOrAmount { get; set; }
            public int Reach { get; set; }//days if time, post count if amount
        }
        public enum TimeOrAmount
        {
            time, amount
        }

    
}
