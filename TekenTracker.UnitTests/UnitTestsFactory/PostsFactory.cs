﻿using Core.Classes.DTO;
using Core.Classes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TekenTracker.UnitTests.UnitTestsFactory
{
    internal class PostsFactory
    {
        int index = 0;
        public PostDto basicPost(string title)
        {
            index++;
            return new PostDto
            {
                MainImageUrl = "tmp",
                PostDate = new DateTime(2020, 4, 23),
                Title = title,
                PostId = index
            };
        }
    }
}
