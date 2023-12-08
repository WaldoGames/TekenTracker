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
                UserId = 1,
                Password = "$2a$11$2yuIcgA7BCvQyvMjl0FvpeCyfFV0d9EIaA7yPebJEfz13SL1eEvL.",
                UserName = "FirstUser",
                Email = "user@mail.com",
                Token = Guid.NewGuid().ToString(),
                TokenValidUntil = DateTime.Now.AddSeconds(60),
            },
                        new User{
                UserId = 2,
                Password = "$2a$11$2yuIcgA7BCvQyvMjl0FvpeCyfFV0d9EIaA7yPebJEfz13SL1eEvL.",
                UserName = "SecondUser",
                Email = "user@mail.com",
                Token = Guid.NewGuid().ToString(),
                TokenValidUntil = DateTime.Now.AddSeconds(60),
            },
        };

        public List<Note> notes = new List<Note>() {
        
            new Note{ NoteId = 1, PostId = 1, UploadDate=new DateTime(2023,11,10), Text="note 1" },
            new Note{ NoteId = 2, PostId = 2, UploadDate=new DateTime(2023,11,11), Text="note 2" },
        };

        public List<SubImage> subImages = new List<SubImage>
        {
            new SubImage{SubimageId = 1 ,PostId = 1, UploadDate = new DateTime(2023,11,10), ImageUrl="url1"},
            new SubImage{SubimageId = 2 ,PostId = 1, UploadDate = new DateTime(2023,11,10), ImageUrl="url2"},
            new SubImage{SubimageId = 3 ,PostId = 2, UploadDate = new DateTime(2023,11,10), ImageUrl="url3"},

        };

        public List<Post> posts = new List<Post>()
            {
                new Post{ PostId = 1, Title = "Post1", MainImageUrl = "url1", PostDate= new DateTime(2023,11,15)},
                new Post{ PostId = 2, Title = "Post2", MainImageUrl = "url2", PostDate= new DateTime(2023,11,15)},
                new Post{ PostId = 3, Title = "Post3", MainImageUrl = "url3", PostDate= new DateTime(2023,11,15)},
                new Post{ PostId = 4, Title = "Post4", MainImageUrl = "url4", PostDate= new DateTime(2023,11,15)},
                new Post{ PostId = 5, Title = "Post5", MainImageUrl = "url5", PostDate= new DateTime(2023,11,15)},
            };
    }
}
