using Core.Classes;
using Core.Classes.DTO;
using Core.Classes.Models;
using Core.Classes.Services;
using Core.Interfaces.Repository;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TekenTracker.UnitTests.UnitTestsFactory;

namespace TekenTracker.UnitTests
{
    public class PostUnitTest
    {
        private PostService postService;
        private IPostRepository postRepository;
        private INoteRepository noteRepository;
        private ISubImageRepository subImageRepository;
        private ITagRepository tagRepository;


        public PostUnitTest()
        {
            postRepository = A.Fake<IPostRepository>();
            noteRepository = A.Fake<INoteRepository>();
            subImageRepository = A.Fake<ISubImageRepository>();
            tagRepository = A.Fake<ITagRepository>();

            postService = new PostService(postRepository, noteRepository, subImageRepository, tagRepository);
        }


        [Fact]
        public void GetMainPagePosts_3_posts()
        {
            PostsFactory PF = new PostsFactory();
            TagFactory Tag = new TagFactory();

            GetOverviewMantPostsDto getOverviewMantPostsDto = new GetOverviewMantPostsDto();
            getOverviewMantPostsDto.userId = 1;
            getOverviewMantPostsDto.count = 25;
            getOverviewMantPostsDto.offset = 0;
            //PostRepository.GetOverviewPost
            A.CallTo(() => postRepository.GetOverviewPost(getOverviewMantPostsDto)).Returns(
                new Result<OverviewManyPostsDto>
                {
                    Data = new OverviewManyPostsDto
                    {
                        Posts =
                        {
                            PF.basicPost("tekening1"),
                            PF.basicPost("tekening2"),
                            PF.basicPost("tekening3"),
                        }
                    }
                }
            );
            A.CallTo(() => tagRepository.GetTagsFromPost(1)).Returns(
                new Result<List<Tag>>
                {
                    Data = new List<Tag>
                    {
                        Tag.Tag(1),
                        Tag.Tag(2)
                    }
                }
            );
            A.CallTo(() => tagRepository.GetTagsFromPost(2)).Returns(
                new Result<List<Tag>>
                {
                    Data = new List<Tag>
                    {
                        Tag.Tag(1),
                        Tag.Tag(3)
                    }
                }
            );
            A.CallTo(() => tagRepository.GetTagsFromPost(3)).Returns(
                new Result<List<Tag>>
                {
                    Data = new List<Tag>
                    {
                        Tag.Tag(1),
                        Tag.Tag(4)
                    }
                }
            );

            Result<OverviewManyPostsDto> result= postService.GetMainPagePosts(getOverviewMantPostsDto);

            Assert.Equal(3, result.Data.Posts.Count);
            Assert.True(string.IsNullOrEmpty(result.ErrorMessage));
            //TagRepository.GetTagsFromPost(post.postId)
        }

        [Fact]
        public void GetMainPagePosts_Tags_failed()
        {
            PostsFactory PF = new PostsFactory();
            TagFactory Tag = new TagFactory();

            GetOverviewMantPostsDto getOverviewMantPostsDto = new GetOverviewMantPostsDto();
            getOverviewMantPostsDto.userId = 1;
            getOverviewMantPostsDto.count = 25;
            getOverviewMantPostsDto.offset = 0;
            //PostRepository.GetOverviewPost
            A.CallTo(() => postRepository.GetOverviewPost(getOverviewMantPostsDto)).Returns(
                new Result<OverviewManyPostsDto>
                {
                    Data = new OverviewManyPostsDto
                    {
                        Posts =
                        {
                            PF.basicPost("tekening1"),
                            PF.basicPost("tekening2"),
                            PF.basicPost("tekening3"),
                        }
                    }
                }
            );
            A.CallTo(() => tagRepository.GetTagsFromPost(1)).Returns(
                new Result<List<Tag>>
                {
                    Data = new List<Tag>
                    {
                        Tag.Tag(1),
                        Tag.Tag(2)
                    }
                }
            );
            A.CallTo(() => tagRepository.GetTagsFromPost(2)).Returns(
                new Result<List<Tag>>
                {
                    Data = new List<Tag>
                    {
                        Tag.Tag(1),
                        Tag.Tag(3)
                    }
                }
            );
            A.CallTo(() => tagRepository.GetTagsFromPost(3)).Returns(
                new Result<List<Tag>>
                {
                    ErrorMessage= "Fake error[Unit test] tagrepository"
                }
            );

            Result<OverviewManyPostsDto> result = postService.GetMainPagePosts(getOverviewMantPostsDto);

            Assert.Equal(Environment.NewLine + "PostService->TryGetMainPagePosts Failed to get tags", result.ErrorMessage);
            Assert.False(string.IsNullOrEmpty(result.ErrorMessage));
            //TagRepository.GetTagsFromPost(post.postId)
        }

       /* [Fact]
        public void */

    }
}
