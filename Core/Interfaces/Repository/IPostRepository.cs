using Core.Classes.DTO;
using Core.Classes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repository
{
    public interface IPostRepository
    {
        public bool doesPostExist(int postId);
        public bool TryAddNewPostToDB(NewPostDto post, out int PrimaryKeyValue);
        public bool TryRemovePostToDB(int PostId);

        public bool TryGetDetailedPost(int PostId, out Post post);
        public bool TryGetOverviewPost(GetOverviewMantPostsDto getPostsDto ,out OverviewManyPostsDto overview);
        public bool ChangeMainImageInDB(int PostId ,string NewUrl, out string OldUrl);
    }
}
