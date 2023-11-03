using Core.Classes.DTO;
using Core.Classes.Models;
using Core.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Core.Classes.Services
{
    internal class PostService
    {
        IPostRepository postRepository;
        INoteRepository noteRepository;
        ISubImageRepository SubImageRepository;
        ITagRepository TagRepository;
        TagService TagService = new TagService();
        NoteServices NoteServices = new NoteServices();
        SubimageService SubimageService = new SubimageService();

        public bool TryGetMainPagePosts(GetOverviewMantPostsDto paramaters, out OverviewManyPostsDto posts)
        {
            posts = null;

            try
            {
                if (postRepository.TryGetOverviewPost(paramaters, out posts))
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex) { }
            {

                return false;
            }         
        }
        public bool TryGetDetailedPost(int postId, out Post post)
        {
            post = null;
            try
            {
                if (postRepository.TryGetDetailedPost(postId, out post))
                {
                    if (noteRepository.TryGetNotesFromPost(postId, out NotesDto notes))
                        post.notes = notes.Notes;
                    if (SubImageRepository.TryGetSubimagesFromPost(postId, out SubimagesDto subimages))
                        post.subImages = subimages.images;
                    if(TagRepository.TryGetTagsFromPost(postId, out List<Tag> tags))
                        post.tags = tags;
                       
                    return true;
                }
                return false;
            }
            catch (Exception ex) { }
            {
                return false;
            }
        }
        public bool TryPostPostToDB(NewPostDto newPost)
        {
            try
            {
                postRepository.TryAddNewPostToDB(newPost, out int PostId);
                if(newPost.Tags != null)
                {
                    TagService.AddTagsToPost(PostId, newPost.Tags.Select(np => np.tagId).ToList());
                }   
                if(newPost.Notes != null)
                {
                    NoteServices.TryAddManyNotesNewPost(newPost.Notes, PostId);
                }
                if(newPost.SubImages != null)
                {
                    SubimageService.TryAddManySubimagesNewPost(newPost.SubImages, PostId);
                }                
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            
        }
        public bool TryDeletePost(int postId)
        {
            if (postRepository.doesPostExist(postId))
            {
                //remove subimages belonging to post

                //remove notes belonging to post
                return postRepository.TryRemovePostToDB(postId);       
            }
            return false;
        }    
        
    }
}
