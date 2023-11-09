﻿using Core.Classes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Classes
{
    public class TagComparer : IEqualityComparer<Tag>
    {
        public bool Equals(Tag x, Tag y)
        {
            return x.tagId == y.tagId;
        }

        public int GetHashCode(Tag obj)
        {
            return obj.tagId.GetHashCode();
        }
    }
}
