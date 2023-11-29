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
            Post post = container.posts.Where(p => p.postId == PostId).First();

            OldUrl = post.mainImageUrl;

            post.mainImageUrl = NewUrl;

            return new SimpleResult();
            }
            catch (Exception e)
            {
                return new SimpleResult { ErrorMessage = "something went wrong change mainimage[Unit test]" };
            }
        }

        public Result<bool> doesPostExist(int postId)
        {
            return new Result<bool> { Data = container.posts.Select(p => p.postId).Contains(postId) };
        }

        public Result<int> AddNewPostToDB(NewPostDto post)
        {
            Post newPost = new Post();

            newPost.title = post.Title;
            newPost.mainImageUrl = post.ImageUrl;
            newPost.postDate = DateTime.Now;
            newPost.postId = container.posts.Select(p => p.postId).Max();

            int PrimaryKeyValue = newPost.postId;

            
            return new Result<int> { Data= PrimaryKeyValue};

            //newPost.subImages = post.SubImages;
        }

        public Result<Post> GetDetailedPost(int PostId)
        {
            Post post = container.posts.Where(p => p.postId == PostId).First();

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
    }
}
