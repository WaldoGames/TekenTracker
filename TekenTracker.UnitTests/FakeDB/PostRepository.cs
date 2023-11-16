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


        public bool ChangeMainImageInDB(int PostId, string NewUrl, out string OldUrl)
        {
            OldUrl = "";
            try
            {
            Post post = container.posts.Where(p => p.postId == PostId).First();

            OldUrl = post.mainImageUrl;

            post.mainImageUrl = NewUrl;

            return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool doesPostExist(int postId)
        {
            return container.posts.Select(p => p.postId).Contains(postId);
        }

        public bool TryAddNewPostToDB(NewPostDto post, out int PrimaryKeyValue)
        {
            Post newPost = new Post();

            newPost.title = post.Title;
            newPost.mainImageUrl = post.ImageUrl;
            newPost.postDate = DateTime.Now;
            newPost.postId = container.posts.Select(p => p.postId).Max();

            PrimaryKeyValue = newPost.postId;

            return true;

            //newPost.subImages = post.SubImages;
        }

        public bool TryGetDetailedPost(int PostId, out Post post)
        {
            post = container.posts.Where(p => p.postId == PostId).First();

            return true;
        }

        public bool TryGetOverviewPost(GetOverviewMantPostsDto getPostsDto, out OverviewManyPostsDto overview)
        {
            throw new NotImplementedException();
        }

        public bool TryRemovePostToDB(int PostId)
        {
            throw new NotImplementedException();
        }
    }
}
