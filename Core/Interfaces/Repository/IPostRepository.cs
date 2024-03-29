﻿using Core.Classes;
using Core.Classes.DTO;
using Core.Classes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repository
{
    public interface IPostRepository
    {
        public Result<bool> DoesPostExist(int postId);
        public Result<int> AddNewPostToDB(NewPostDto post);
        public SimpleResult RemovePostToDB(int postId);

        public Result<Post> GetDetailedPost(int postId);
        public Result<OverviewManyPostsDto> GetOverviewPost(GetOverviewMantPostsDto getPostsDto);
        public SimpleResult ChangeMainImageInDB(int postId ,string newUrl);

        public Result<List<string>> GetRandomImagesFromUser(int userId, int max = 9);
    }
}
