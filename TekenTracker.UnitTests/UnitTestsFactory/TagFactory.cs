using Core.Classes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Classes.Enums;

namespace TekenTracker.UnitTests.UnitTestsFactory
{
    internal class TagFactory
    {
        List<Tag> tags;
        public TagFactory()
        {
            tags = new List<Tag>()
            {
                new Tag()
                {
                    name = "NULLtag",
                    tagId = 0,
                    type = TagTypes.Search
                },
                new Tag()
                {
                    name = "FirstTag",
                    tagId = 1,
                    type = TagTypes.Search
                },
                new Tag()
                {
                    name = "PineApple",
                    tagId = 2,
                    type = TagTypes.Search
                },
                new Tag()
                {
                    name = "Tree",
                    tagId = 3,
                    type = TagTypes.Search
                },
                new Tag()
                {
                    name = "No",
                    tagId = 4,
                    type = TagTypes.Search
                },
                new Tag()
                {
                    name = "The sun is a deadly lazer",
                    tagId = 5,
                    type = TagTypes.Search
                },

            };
        }

        public Tag Tag(int id)
        {
            return tags[id];
        }
    }
}
