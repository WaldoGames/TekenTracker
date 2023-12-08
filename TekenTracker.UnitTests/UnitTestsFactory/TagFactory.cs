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
                    Name = "NULLtag",
                    TagId = 0,
                    Type = TagTypes.Search
                },
                new Tag()
                {
                    Name = "FirstTag",
                    TagId = 1,
                    Type = TagTypes.Search
                },
                new Tag()
                {
                    Name = "PineApple",
                    TagId = 2,
                    Type = TagTypes.Search
                },
                new Tag()
                {
                    Name = "Tree",
                    TagId = 3,
                    Type = TagTypes.Search
                },
                new Tag()
                {
                    Name = "No",
                    TagId = 4,
                    Type = TagTypes.Search
                },
                new Tag()
                {
                    Name = "The sun is a deadly lazer",
                    TagId = 5,
                    Type = TagTypes.Search
                },

            };
        }

        public Tag Tag(int id)
        {
            return tags[id];
        }
    }
}
