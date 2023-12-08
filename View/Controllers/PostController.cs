using Core.Classes;
using Core.Classes.DTO;
using Core.Classes.Models;
using Core.Classes.Services;
using Core.Interfaces.Repository;
using Dal.Classes;
using Dal.Classes.RepositoryImplementations;
using Humanizer;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using NuGet.Protocol;
using System.IO.Compression;
using View.Models;

namespace View.Controllers
{
    public class PostController : LoggedinControllerBase
    {
        // GET: PostController
        PostService postService;
        TagService tagService;
        SessionController sessionController;
        IWebHostEnvironment webHost;
        FileChecker fileChecker;
        public PostController(IMemoryCache cache, IWebHostEnvironment webHost) : base(cache)
        {
            postService = new PostService(new PostRepository(),new NoteRepository(), new SubimageRepository(),new TagRepository());
            tagService = new TagService(new TagRepository());
            sessionController = new SessionController(cache);
            this.webHost = webHost;
            fileChecker = new FileChecker();
        }


        //TODO: alleen de tags die bij opgehalde posts horen worden toegevoegd aan de index zoek functie.
        //terwijl de plan is alleen tags te laten zien als er minimaal 1 post van de dit moet buiten de zoek functie staan.
        public ActionResult Index(MainPageViewModel mainPageViewModel)
        {
            if (!CheckLogin(out ActionResult LoginView))
            {
                return LoginView;
            }
            if(mainPageViewModel == null)
            {
                mainPageViewModel = new MainPageViewModel();
            }
            
            sessionController.GetUserFromSession(out UserDto user);
            mainPageViewModel.userId = user.userId;

            GetOverviewMantPostsDto getOverviewMantPostsDto = new GetOverviewMantPostsDto();
            getOverviewMantPostsDto.Tags = mainPageViewModel.usedTags;
            getOverviewMantPostsDto.userId = user.userId;

            Result <OverviewManyPostsDto> posts = postService.GetMainPagePosts(getOverviewMantPostsDto);

            if (posts.IsFailed)
            {
                return View("error");
            }

            mainPageViewModel.posts = posts.Data.Posts;
            Result<List<Tag>> c = tagService.GetSearchTagsFromUser(user.userId);

            if (c.IsFailed)
            {
                return View("error");
            }

            ViewBag.Tags = c.Data.Where(t=>t.type == Core.Classes.Enums.TagTypes.Search).ToList();

            return View(mainPageViewModel);
        }


        // GET: PostController/Details/5
        public ActionResult Details(int id)
        {
            Result<Post> post = postService.GetDetailedPost(id);

            if (post.IsFailed)
            {
                return View("error");  
            }

            return View(post.Data.GetPost());
        }
        
        // GET: PostController/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NewPostViewModel newPost)
        {
            //create the post (make sure to return the id)

            FileUploadPreCheckValue FileTest = fileChecker.TestFile(newPost.image);

            if(FileTest != FileUploadPreCheckValue.Accepted)
            {
                return View("error");
            }

            NewPostDto dto = new NewPostDto();
            if (sessionController.GetUserFromSession(out UserDto? user)) {
                dto.Poster = user.userId;
                dto.Notes = newPost.Note;
                dto.Title = newPost.Title;
                dto.ImageUrl = UploadImage(newPost.image);
                if(newPost.subimages != null)
                {

                    dto.SubImages = UploadManyImages(newPost.subimages);
                }
            }
            Result<int> result = postService.PostPostToDB(dto);
            if (result.IsFailed)
            {
                return View("error");
            }

            return RedirectToAction("EditTagsFromPost", "Post", new { id = result.Data });//return tagedit window(make sure id is in link)
        }

        // POST: PostController/Create

        // GET: PostController/Edit/5
        public ActionResult EditTagsFromPost(int id)
        {
            TagEditViewModel tm = new TagEditViewModel();
            tm.PostId = id;
            //get all tags to 

            Result<List<Tag>> Alltags = tagService.GetAllTags();
            if (Alltags.IsFailed)
            {
                return View("error");
            }

            ViewBag.Tags = Alltags.Data.Where(e=>e.type == Core.Classes.Enums.TagTypes.Search).ToList();
            ViewBag.ImprovementTags = Alltags.Data.Where(e => e.type == Core.Classes.Enums.TagTypes.Improvement).ToList();
 
            Result<List<Tag>> tags = postService.GetTagsFromPost(id);

            if (tags.IsFailed)
            {
                return View("error");
            }

            tm.Tags = tags.Data;

            tm.TagIds = tm.Tags.Select(t => t.tagId).ToList();

            return View(tm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTagsFromPost(int id, TagEditViewModel? tagEditViewModel)
        {

            if(tagEditViewModel == null || tagEditViewModel.TagIds == null)
            {
                return RedirectToAction("EditTagsFromPost", "Post", new { id });
            }
            tagService.UpdateTagsFromPost(id, tagEditViewModel.TagIds);
            return RedirectToAction("Index", "Post", new { });
           
        }
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PostController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PostController/Delete/5
        public ActionResult Delete(int id)
        {
            postService.DeletePost(id);

            return RedirectToAction("Index", "Post", new { });
        }

        // POST: PostController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult ChangeMainImage(int id)
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeMainImage(int id, NewMainImageModel model)
        {
            FileUploadPreCheckValue FileTest = fileChecker.TestFile(model.Image);

            if (FileTest != FileUploadPreCheckValue.Accepted)
            {
                return View("error");
            }


            string path = UploadImage(model.Image);

            SimpleResult result = postService.ChangeMainImageOfPost(id, path, model.MoveOldImageToSubimage);

            if (result.IsFailed)
            {
                return View("error");
            }

            return RedirectToAction("Details", "Post", new { id = id });//return tagedit window(make sure id is in link)

        }

        public ActionResult AddSubImage(int id)
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddSubImage(int id, NewSubImagesModel model)
        {
            List<NewSubimageDto> images = UploadManyImages(model.subimages);

            images.ForEach(i => i.postId = id);

            SimpleResult result = postService.AddManySubimageToExistingPost(id ,images);

            if (result.IsFailed)
            {
                return View("error");
            }

            return RedirectToAction("Details", "Post", new { id = id });
        }
        private List<NewSubimageDto> UploadManyImages(List<IFormFile> images)
        {
            List<NewSubimageDto> newimages = new List<NewSubimageDto>();
            foreach (IFormFile item in images)
            {
                NewSubimageDto subimage = new NewSubimageDto();
                subimage.imageUrl = UploadImage(item);
                newimages.Add(subimage);
            }
            return newimages;
        }

        private string UploadImage(IFormFile image)
        {
            string fileName = null;



            if (image != null)
            {

                string path = Path.Combine(webHost.WebRootPath, "Images");
                string Filename = Guid.NewGuid() + image.FileName;
                string FilePath = Path.Combine(path, Filename);
                fileName = Filename;
                using (var fileSteam = new FileStream(FilePath, FileMode.Create))
                {
                    image.CopyTo(fileSteam);
                }
            }
            return fileName;
        }

        
    }

}
