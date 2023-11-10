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
        public bool TryGetAllTags(out List<Tag> tags);
        public bool TryAddNewTagToDB(string tagName);
        public bool TryRemoveStringFromDB(int tagId);
        public bool TryGetTags(GetTagsDto getTagsDto, out List<Tag> tags);
        public bool TryGetTagsFromPost(int postId, out List<Tag> tags);

        public bool TryGetTagsUsedByUser(int userId, out List<Tag> tags);
        public bool DoesPostHaveTag(int postId, int tagId);

        public bool TryAddTagToPost(int postId, int tagId);
        public bool TryRemoveTagFromPost(int postId, int tagId);

    }
}
