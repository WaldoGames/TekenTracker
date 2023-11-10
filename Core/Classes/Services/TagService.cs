using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Classes.Models;
using Core.Interfaces.Repository;

namespace Core.Classes.Services
{
    public class TagService
    {
        ITagRepository TagRepository;

        public TagService(ITagRepository tagRepository)
        {
            TagRepository = tagRepository;
        }

        public void UpdateTagsFromPost(int PostId, List<int> tagIds)
        {
            if (TagRepository.TryGetTagsFromPost(PostId, out List<Tag> currentTags))
            {
                List<Tag> ToRemove = currentTags.Where(e => !tagIds.Contains(e.tagId)).ToList();
                List<int> ToAdd = tagIds.Where(e => !currentTags.Select(e=>e.tagId).Contains(e)).ToList();

                RemoveTagsFromPost(PostId,ToRemove.Select(e => e.tagId).ToList());
                AddTagsToPost(PostId, ToAdd);
            }
             
        }
        public bool TryGetSearchTagsFromUser(int userId, out List<Tag>? tags)
        {
            if (TagRepository.TryGetTagsUsedByUser(userId, out List<Tag> UsedTags))
            {
                tags = UsedTags.Where(t => t.type == Enums.TagTypes.Search).OrderBy(t=>t.name).ToList();
                return true;
            }
            tags = null;
            return false;
        }
        public void AddTagsToPost(int postId, List<int> tagIds)
        {
            foreach (int tag in tagIds)
            {
                TagRepository.TryAddTagToPost(postId, tag);     
            }
        }
        public void RemoveTagsFromPost(int postId, List<int> tagIds)
        {
            foreach (int tag in tagIds)
            {
                TagRepository.TryRemoveTagFromPost(postId, tag);
            }
        }
        public bool TryGetAllTags(out List<Tag>? tags)//turn in
        {
            return TagRepository.TryGetAllTags(out tags);
        }

        public void CreateNewTag(string tagName, int tagType)
        {

        }
    }
}
