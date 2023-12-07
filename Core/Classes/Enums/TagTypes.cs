using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Classes.Enums
{
    public enum TagTypes
    {
        [Display(Name = "First Value - desc..")]
        Search = 1,
        [Display(Name = "Second Value - desc...")]
        Improvement = 2
    }
}
