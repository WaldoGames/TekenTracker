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
    public class PostService
    {
        IPostRepository PostRepository;
        INoteRepository NoteRepository;
        ISubImageRepository SubImageRepository;
        ITagRepository TagRepository;
        TagService TagService;
        NoteServices NoteServices;
        SubimageService SubimageService;

        public PostService(IPostRepository postRepository, INoteRepository noteRepository, ISubImageRepository subImageRepository, ITagRepository tagRepository)
        {
            this.PostRepository = postRepository;
            this.NoteRepository = noteRepository;
            this.SubImageRepository = subImageRepository;
            this.TagRepository = tagRepository;

            TagService = new TagService(TagRepository);
            NoteServices = new NoteServices(NoteRepository);
            SubimageService = new SubimageService(SubImageRepository);
            
        }

        public bool TryGetMainPagePosts(GetOverviewMantPostsDto paramaters, out OverviewManyPostsDto posts)
        {
            posts = null;

            try
            {
                if (PostRepository.TryGetOverviewPost(paramaters, out posts))
                {
                    foreach (var post in posts.Posts)
                    {
                        if (TagRepository.TryGetTagsFromPost(post.postId,out List<Tag> tags))
                        {
                            post.Tags = tags;
                        }
                    }
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
                if (PostRepository.TryGetDetailedPost(postId, out post))
                {
                    if (NoteRepository.TryGetNotesFromPost(postId, out NotesDto notes))
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
        public bool TryPostPostToDB(NewPostDto newPost, out int PostId)
        {
            try
            {
                PostRepository.TryAddNewPostToDB(newPost, out PostId);
 
                if(newPost.Notes != null)
                {
                    NewNoteDto newNoteDto = new NewNoteDto();
                    newNoteDto.Text = newPost.Notes;
                    newNoteDto.PostId = PostId;
                    NoteServices.TryAddNewNote(newNoteDto);
                }
                if(newPost.SubImages != null)
                {
                    SubimageService.TryAddManySubimagesNewPost(newPost.SubImages, PostId);
                }                
                return true;
            }
            catch (Exception)
            {
                PostId = -1;
                return false;
            }
            
        }

        public bool TryDeletePost(int postId)
        {
            if (PostRepository.doesPostExist(postId))
            {
                //remove subimages belonging to post

                //remove notes belonging to post
                return PostRepository.TryRemovePostToDB(postId);       
            }
            return false;
        }  
        public bool TryGetTagsFromPost(int postId, out List<Tag> tags)
        {
            try
            {
                return TagRepository.TryGetTagsFromPost(postId, out tags);
            }
            catch (Exception)
            {
                throw;
            }
        }
        
    }
}
