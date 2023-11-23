using Core.Classes.DTO;
using Core.Classes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TekenTracker.UnitTests
{
    internal class DataContainer
    {
        public List<User> users = new List<User>()
        {
            new User{
                userId = 1,
                password = "$2a$11$2yuIcgA7BCvQyvMjl0FvpeCyfFV0d9EIaA7yPebJEfz13SL1eEvL.",
                userName = "FirstUser",
                email = "user@mail.com",
                Token = Guid.NewGuid().ToString(),
                TokenValidUntil = DateTime.Now.AddSeconds(60),
            },
                        new User{
                userId = 2,
                password = "$2a$11$2yuIcgA7BCvQyvMjl0FvpeCyfFV0d9EIaA7yPebJEfz13SL1eEvL.",
                userName = "SecondUser",
                email = "user@mail.com",
                Token = Guid.NewGuid().ToString(),
                TokenValidUntil = DateTime.Now.AddSeconds(60),
            },
        };

        public List<Note> notes = new List<Note>() {
        
            new Note{ noteId = 1, postId = 1, uploadDate=new DateTime(2023,11,10), text="note 1" },
            new Note{ noteId = 2, postId = 2, uploadDate=new DateTime(2023,11,11), text="note 2" },
        };

        public List<SubImage> subImages = new List<SubImage>
        {
            new SubImage{subimageId = 1 ,postId = 1, uploadDate = new DateTime(2023,11,10), imageUrl="url1"},
            new SubImage{subimageId = 2 ,postId = 1, uploadDate = new DateTime(2023,11,10), imageUrl="url2"},
            new SubImage{subimageId = 3 ,postId = 2, uploadDate = new DateTime(2023,11,10), imageUrl="url3"},

        };

        public List<Post> posts = new List<Post>()
            {
                new Post{ postId = 1, title = "Post1", mainImageUrl = "url1", postDate= new DateTime(2023,11,15)},
                new Post{ postId = 2, title = "Post2", mainImageUrl = "url2", postDate= new DateTime(2023,11,15)},
                new Post{ postId = 3, title = "Post3", mainImageUrl = "url3", postDate= new DateTime(2023,11,15)},
                new Post{ postId = 4, title = "Post4", mainImageUrl = "url4", postDate= new DateTime(2023,11,15)},
                new Post{ postId = 5, title = "Post5", mainImageUrl = "url5", postDate= new DateTime(2023,11,15)},
            };
    }
}
