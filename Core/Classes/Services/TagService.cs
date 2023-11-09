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
        public void AddTagsToPost(int PostId, List<int> tagIds)
        {
            foreach (int tag in tagIds)
            {
                TagRepository.TryAddTagToPost(PostId, tag);     
            }
        }
        public void RemoveTagsFromPost(int PostId, List<int> tagIds)
        {
            foreach (int tag in tagIds)
            {
                TagRepository.TryRemoveTagFromPost(PostId, tag);
            }
        }
        public bool TryGetAllTags(out List<Tag>? tags)//turn in
        {
            return TagRepository.TryGetAllTags(out tags);
        }
        public void CreateNewTag(string TagName, int TagType)
        {

        }
    }
}
