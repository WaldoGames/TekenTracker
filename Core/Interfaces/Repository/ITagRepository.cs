using Core.Classes;
using Core.Classes.DTO;
using Core.Classes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repository
{
    public interface ITagRepository
    {
        public Result<List<Tag>> GetAllTags();
        public SimpleResult AddNewTagToDB(string tagName);
        public SimpleResult RemoveStringFromDB(int tagId);
        public Result<List<Tag>> GetTags(GetTagsDto getTagsDto);
        public Result<List<Tag>> GetTagsFromPost(int postId);

        public Result<List<Tag>> GetTagsUsedByUser(int userId);
        public Result<bool> DoesPostHaveTag(int postId, int tagId);

        public SimpleResult AddTagToPost(int postId, int tagId);
        public SimpleResult RemoveTagFromPost(int postId, int tagId);

    }
}
