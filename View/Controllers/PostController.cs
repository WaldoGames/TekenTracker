using Core.Classes;
using Core.Classes.DTO;
using Core.Classes.Models;
using Core.Classes.Services;
using Core.Interfaces.Repository;
using Dal.Classes;
using Dal.Classes.RepositoryImplementations;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using View.Models;

namespace View.Controllers
{
    public class PostController : LoggedinControllerBase
    {
        // GET: PostController
        PostService postService;
        TagService tagService;
        SessionController sessionController;
        IWebHostEnvironment WebHost;
        public PostController(IMemoryCache cache, IWebHostEnvironment webHost) : base(cache)
        {
            postService = new PostService(new PostRepository(),new NoteRepository(), new SubimageRepository(),new TagRepository());
            tagService = new TagService(new TagRepository());
            sessionController = new SessionController(cache);
            WebHost = webHost;
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

            postService.TryGetMainPagePosts(getOverviewMantPostsDto, out OverviewManyPostsDto posts);

            mainPageViewModel.posts = posts.Posts;
            List<Tag> c = new List<Tag>();
            if (tagService.TryGetAllTags(out List<Tag> tags)){

                c = posts.Posts.SelectMany(p => p.Tags).Distinct(new TagComparer()).OrderBy(t=>t.name).ToList();
            }
            ViewBag.Tags = c.Where(t=>t.type == Core.Classes.Enums.TagTypes.Search).ToList();

            return View(mainPageViewModel);
        }


        // GET: PostController/Details/5
        public ActionResult Details(int id)
        {
            if(postService.TryGetDetailedPost(id, out Post post))
            {
                return View(post.GetPost());
            }



            return View("error");
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
            NewPostDto dto = new NewPostDto();
            if (sessionController.GetUserFromSession(out UserDto? user)) {
                dto.Poster = user.userId;
                dto.Notes = newPost.Note;
                dto.Title = newPost.Title;
                dto.ImageUrl = UploadImage(newPost.image);
                if(newPost.subimages != null)
                {
                    dto.SubImages = new List<NewSubimageDto>();
                    foreach (IFormFile item in newPost.subimages)
                    {
                        NewSubimageDto subimage = new NewSubimageDto();
                        subimage.imageUrl = UploadImage(item);
                        dto.SubImages.Add(subimage);
                    }
                }
            }
            postService.TryPostPostToDB(dto, out int PostId);

            return RedirectToAction("EditTagsFromPost", "Post", new { id = PostId });//return tagedit window(make sure id is in link)
        }

        // POST: PostController/Create

        // GET: PostController/Edit/5
        public ActionResult EditTagsFromPost(int id)
        {
            TagEditViewModel tm = new TagEditViewModel();
            tm.PostId = id;
            //get all tags to 
            if (tagService.TryGetAllTags(out List<Tag> Alltags))
            {

                ViewBag.Tags = Alltags.Where(e=>e.type == Core.Classes.Enums.TagTypes.Search).ToList();
                ViewBag.ImprovementTags = Alltags.Where(e => e.type == Core.Classes.Enums.TagTypes.Improvement).ToList();
            }
            else
            {
                return View("error");
            }

            if (postService.TryGetTagsFromPost(id, out List<Tag> tags))
            {
                tm.Tags = tags;
            }
            else
            {
                return View("error");
            }
            tm.TagIds = tm.Tags.Select(t => t.tagId).ToList();

            return View(tm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTagsFromPost(int id, TagEditViewModel tagEditViewModel)
        {

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
            return View();
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

        private string UploadImage(IFormFile image)
        {
            string fileName = null;

            if (image != null)
            {

                string path = Path.Combine(WebHost.WebRootPath, "Images");
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
