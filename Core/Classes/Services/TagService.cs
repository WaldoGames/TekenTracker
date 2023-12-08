using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Classes.DTO;
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

        public SimpleResult UpdateTagsFromPost(int postId, List<int> tagIds)
        {
            Result<List<Tag>> currentPostTags = TagRepository.GetTagsFromPost(postId);

            if (currentPostTags.IsFailed)
            {
                return new SimpleResult { ErrorMessage = "TagService->UpdateTagsFromPost: Passed from TagRepository->GetTagsFromPost" };
            }
            if (currentPostTags.Data == null)
            {
                currentPostTags.Data = new List<Tag>();
            }
            List<Tag> ToRemove = currentPostTags.Data.Where(e => !tagIds.Contains(e.TagId)).ToList();
            List<int> ToAdd = tagIds.Where(e => !currentPostTags.Data.Select(e => e.TagId).Contains(e)).ToList();

            RemoveTagsFromPost(postId, ToRemove.Select(e => e.TagId).ToList());
            AddTagsToPost(postId, ToAdd);

            return new SimpleResult();

        }
        public Result<List<Tag>> GetSearchTagsFromUser(int userId)
        {
            Result<List<Tag>> tags = TagRepository.GetTagsUsedByUser(userId);

            if (tags.IsFailed)
            {
                return new Result<List<Tag>> { ErrorMessage = "TagService->TrySearchTagsFromUser: error passed from TagRepository->GetTagsUsedByUser" };
            }
            if(tags.Data == null)
            {
                tags.Data = new List<Tag>();
            }
            tags.Data = tags.Data.Where(t => t.Type == Enums.TagTypes.Search).OrderBy(t => t.Name).ToList();
            return new Result<List<Tag>> { Data = tags.Data };

        }
        public SimpleResult AddTagsToPost(int postId, List<int> tagIds)
        {
            foreach (int tag in tagIds)
            {
                SimpleResult tmp = TagRepository.AddTagToPost(postId, tag);

                if (tmp.IsFailed)
                {
                    return new SimpleResult { ErrorMessage = "TagService->AddTagsToPost error passed from TagRepository->AddTagToPost" };
                }
            }
            return new SimpleResult();
        }
        public SimpleResult RemoveTagsFromPost(int postId, List<int> tagIds)
        {
            foreach (int tag in tagIds)
            {
                SimpleResult tmp = TagRepository.RemoveTagFromPost(postId, tag);

                if (tmp.IsFailed)
                {
                    return new SimpleResult { ErrorMessage = "TagService->RemoveTagsFromPost error passed from TagRepository->RemoveTagFromPost" };
                }
            }
            return new SimpleResult();
        }
        public Result<List<Tag>> GetAllTags()//turn in
        {
            return TagRepository.GetAllTags();
        }

        public SimpleResult CreateNewTag(string tagName, int tagType)
        {
            //TODO:? check if tag/type combination already exists


            return TagRepository.AddNewTagToDB(tagName, tagType);
        }

        public Result<List<TagAndAmount>> GetTagsForImprovementWindow(ImprovementSearchLimit searchLimit, int userId)
        {
            if(searchLimit.TimeOrAmount == TimeOrAmount.time)
            {
                return TagRepository.GetSearchTagsInLastNumberOfDays(searchLimit.Reach, userId);
            }
            else
            {
                return TagRepository.GetSearchTagsInLastNumberOfPost(searchLimit.Reach, userId);
            }
        }
    }
}
