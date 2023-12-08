using Core.Classes.DTO;
using Core.Classes.Models;
using Core.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Core.Classes.Services
{
    public class PostService
    {
        IPostRepository PostRepository;
        INoteRepository NoteRepository;
        ISubImageRepository SubImageRepository;
        ITagRepository TagRepository;
        TagService TagService;
        NoteService NoteServices;
        SubimageService SubimageService;

        public PostService(IPostRepository postRepository, INoteRepository noteRepository, ISubImageRepository subImageRepository, ITagRepository tagRepository)
        {
            this.PostRepository = postRepository;
            this.NoteRepository = noteRepository;
            this.SubImageRepository = subImageRepository;
            this.TagRepository = tagRepository;

            TagService = new TagService(TagRepository);
            NoteServices = new NoteService(NoteRepository);
            SubimageService = new SubimageService(SubImageRepository);
            
        }

        public Result<OverviewManyPostsDto> GetMainPagePosts(GetOverviewMantPostsDto paramaters)
        {
            Result<OverviewManyPostsDto> posts = PostRepository.GetOverviewPost(paramaters);

            if (posts.IsFailed)
            {
                return new Result<OverviewManyPostsDto> { ErrorMessage = "PostService->TryGetMainPagePosts Post could not be found" };
            }
            if(posts.Data == null)
            {
                posts.Data = new OverviewManyPostsDto();
            }
            foreach (var post in posts.Data.Posts)
            {
                Result<List<Tag>> tags = TagRepository.GetTagsFromPost(post.PostId);

                if (tags.IsFailed)
                {
                    return new Result<OverviewManyPostsDto> { ErrorMessage = "PostService->TryGetMainPagePosts Failed to get tags" };
                }
                if (tags.Data == null)
                {
                    tags.Data = new List<Tag>();
                }
                post.Tags = tags.Data;
            }
            return posts;
        }  
        public Result<Post> GetDetailedPost(int postId)
        {
            Result<Post> post = PostRepository.GetDetailedPost(postId);

            if (post.IsFailed)
            {
                return new Result<Post> { ErrorMessage = "PostService->TryGetDetailedPost passed from PostRepository->GetDetailedPost" };
            }
            if (post.Data == null)
            {
                return new Result<Post> { ErrorMessage = "PostService->TryGetDetailedPost Post not found" };
            }
            Result<NotesDto> notes = NoteRepository.GetNotesFromPost(postId);

            if (notes.IsFailed)
            {
                return new Result<Post> { ErrorMessage = "PostService->TryGetDetailedPost Failed to get notes" };
            }
            post.Data.Notes = notes.Data.Notes;

            Result<SubimagesDto> subimages = SubImageRepository.GetSubimagesFromPost(postId);

            if (subimages.IsFailed)
            {
                return new Result<Post> { ErrorMessage = "PostService->TryGetDetailedPost Failed to get Subimages" };
            }
            post.Data.SubImages = subimages.Data.Images;

            Result<List<Tag>> tags = TagRepository.GetTagsFromPost(postId);

            if (tags.IsFailed)
            {
                return new Result<Post> { ErrorMessage = "PostService->TryGetDetailedPost Failed to get tags" };
            }
            post.Data.Tags = tags.Data;

            return post;
        }
        public Result<int> PostPostToDB(NewPostDto newPost)
        {
            Result<int> newPostId = PostRepository.AddNewPostToDB(newPost);

            if (newPostId.IsFailed)
            {
                return new Result<int> { ErrorMessage = "PostService->PostPostToDB failed to post post" };
            }

            if(newPost.Notes != null)
            {
                NewNoteDto newNoteDto = new NewNoteDto();
                newNoteDto.Text = newPost.Notes;
                newNoteDto.PostId = newPostId.Data;
                SimpleResult result = NoteServices.AddNewNote(newNoteDto);
                if (result.IsFailed)
                {
                    return new Result<int> { ErrorMessage = "PostService->TryPostPostToDB failed to add notes to post" };

                }
            }
            if (newPost.SubImages != null)
            {
                SimpleResult result = SubimageService.AddManySubimagesNewPost(newPost.SubImages, newPostId.Data);
                if (result.IsFailed)
                {
                    return new Result<int> { ErrorMessage = "PostService->TryPostPostToDB failed to add subimages to post" };
                }
            }

            return new Result<int> { Data = newPostId.Data };
            
        }
        public SimpleResult DeletePost(int postId)
        {
            Result<bool> result= PostRepository.doesPostExist(postId);

            if (result.IsFailed)
            {
                return new SimpleResult { ErrorMessage = "PostService->DeletePost could not confirm exisitance of post" };
            }

            return PostRepository.RemovePostToDB(postId);
        }  
        public Result<List<Tag>> GetTagsFromPost(int postId)
        {
            return TagRepository.GetTagsFromPost(postId);

        }

        public SimpleResult ChangeMainImageOfPost(int postId, string path, bool keepOldImageAsSubimage = true)
        {
            Result<Post> post = GetDetailedPost(postId);

            if (post.IsFailed)
            {
                return new SimpleResult { ErrorMessage = "PostService->CopyMainImageToGallery failed to get post" };
            }

            if (keepOldImageAsSubimage)
            {
                SimpleResult copyResult = CopyMainImageToGallery(postId);
                if (copyResult.IsFailed)
                {
                    return copyResult;
                }
            }
            return PostRepository.ChangeMainImageInDB(postId, path);


        }
        private SimpleResult CopyMainImageToGallery(int postId)
        {
            Result<Post> post = GetDetailedPost(postId);

            if (post.IsFailed)
            {
                return new SimpleResult { ErrorMessage = "PostService->CopyMainImageToGallery failed to get post" };
            }
            NewSubimageDto newSubimage = new NewSubimageDto { ImageUrl = post.Data.MainImageUrl, PostId = postId };

            return SubimageService.AddNewSubimage(newSubimage);
        }
        public SimpleResult AddManySubimageToExistingPost(int postId, List<NewSubimageDto> images)
        {
            foreach (NewSubimageDto image in images)
            {
                SimpleResult tmpResult = AddSubimageToExistingPost(image);
                if (tmpResult.IsFailed)
                {
                    return new SimpleResult { ErrorMessage = "PostService->AddManySubimageToExistingPost one or more images failed to post" };
                }
            }
            return new SimpleResult();
        }
        public SimpleResult AddSubimageToExistingPost(NewSubimageDto newSubimage)
        {
            return SubimageService.AddNewSubimage(newSubimage);
        }
        
    }
}
