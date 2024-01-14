using Core.Classes;
using Core.Classes.DTO;
using Core.Classes.Models;
using Core.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TekenTracker.UnitTests.FakeDB
{
    internal class PostRepository : IPostRepository
    {

        DataContainer container = new DataContainer();
        public PostRepository()
        {
            
        }


        public SimpleResult ChangeMainImageInDB(int PostId, string NewUrl)
        {
            string OldUrl = "";
            try
            {
            Post post = container.posts.Where(p => p.PostId == PostId).First();

            OldUrl = post.MainImageUrl;

            post.MainImageUrl = NewUrl;

            return new SimpleResult();
            }
            catch (Exception e)
            {
                return new SimpleResult { ErrorMessage = "something went wrong change mainimage[Unit test]" };
            }
        }

        public Result<bool> DoesPostExist(int postId)
        {
            return new Result<bool> { Data = container.posts.Select(p => p.PostId).Contains(postId) };
        }

        public Result<int> AddNewPostToDB(NewPostDto post)
        {
            Post newPost = new Post();

            newPost.Title = post.Title;
            newPost.MainImageUrl = post.ImageUrl;
            newPost.PostDate = DateTime.Now;
            newPost.PostId = container.posts.Select(p => p.PostId).Max();

            int PrimaryKeyValue = newPost.PostId;

            
            return new Result<int> { Data= PrimaryKeyValue};

            //newPost.subImages = post.SubImages;
        }

        public Result<Post> GetDetailedPost(int PostId)
        {
            Post post = container.posts.Where(p => p.PostId == PostId).First();

            if(post == null)
            {
                return new Result<Post> { ErrorMessage = "get detailed post went wrong [unit test]" };
            }
            return new Result<Post> { Data = post };
        }

        public Result<OverviewManyPostsDto> GetOverviewPost(GetOverviewMantPostsDto getPostsDto)
        {
            throw new NotImplementedException();
        }

        public SimpleResult RemovePostToDB(int PostId)
        {
            throw new NotImplementedException();
        }

        public Result<List<string>> GetRandomImagesFromUser(int userId, int max = 9)
        {
            throw new NotImplementedException();
        }
    }
}
