using Core.Classes;
using Core.Classes.DTO;
using Core.Classes.Models;
using Core.Interfaces.Repository;
using Google.Protobuf.Collections;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Dal.Classes.RepositoryImplementations
{
    public class PostRepository : IPostRepository
    {
        string CS = "SERVER=127.0.0.1;UID=root;PASSWORD=;DATABASE=tekentrackerdb";


        public SimpleResult ChangeMainImageInDB(int PostId, string NewUrl)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(CS))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("UPDATE posts SET main_image_url=@newUrl WHERE post_id = @postId", con);
                    cmd.Parameters.AddWithValue("@newUrl", NewUrl);
                    cmd.Parameters.AddWithValue("@postId", PostId);
                    cmd.CommandType = CommandType.Text;
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    con.Close();
                    return new SimpleResult();
                }
            }
            catch (Exception e)
            {
                return new SimpleResult { ErrorMessage = "PostRepositroy->ChangeMainImageInDB" + e.Message };
                throw;
            }
        }

        public Result<bool> doesPostExist(int postId)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(CS))
                {
                    MySqlCommand cmd = new MySqlCommand("SELECT * FROM posts WHERE post_id = @id LIMIT 1", con);
                    cmd.Parameters.AddWithValue("@id", postId);
                    cmd.CommandType = CommandType.Text;
                    con.Open();

                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        con.Close();
                        return new Result<bool> { Data = true };
                        
                    
                    }
                    con.Close();

                }
            }
            catch (Exception e)
            {
                return new Result<bool> { ErrorMessage = "PostRepository->doesPostExist: " + e.Message};

            }
            return new Result<bool> { Data = false };
        }

        public Result<int> AddNewPostToDB(NewPostDto post)
        {
            int PrimaryKeyValue = -1;
            try
            {
                using (MySqlConnection con = new MySqlConnection(CS))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("INSERT INTO posts(title, main_image_url, post_date, user_id) VALUES(@title, @url, @date, @user); SELECT LAST_INSERT_ID();", con);
                    cmd.Parameters.AddWithValue("@title", post.Title);
                    cmd.Parameters.AddWithValue("@url", post.ImageUrl);
                    cmd.Parameters.AddWithValue("@user", post.Poster);
                    cmd.Parameters.AddWithValue("@date", DateTime.Now);
                    cmd.CommandType = CommandType.Text;
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    PrimaryKeyValue = (int)cmd.LastInsertedId;
                        con.Close();
                    return new Result<int> { Data =PrimaryKeyValue };


                }
            }
            catch (Exception e)
            {
                return new Result<int> { ErrorMessage = "PostRepository->AddNewPostToDB: " + e.Message };
                throw;
            }
        }

        public Result<Post> GetDetailedPost(int PostId)
        {
            try
            {
                Post post = new Post();
                using (MySqlConnection con = new MySqlConnection(CS))
                {
                    MySqlCommand cmd = new MySqlCommand("SELECT * FROM posts WHERE post_id = @id LIMIT 1", con);
                    cmd.Parameters.AddWithValue("@id", PostId);
                    cmd.CommandType = CommandType.Text;
                    con.Open();

                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        post.title= Convert.ToString(rdr["title"]);
                        post.postDate = Convert.ToDateTime(rdr["post_date"]);
                        post.mainImageUrl = Convert.ToString(rdr["main_image_url"]);
                        post.user_Id = Convert.ToInt32(rdr["user_id"]);
                        post.postId = Convert.ToInt32(rdr["post_id"]);
                        con.Close();

                        return new Result<Post> { Data = post };
                    }
                    con.Close();

                }
            }
            catch (Exception e)
            {
                return new Result<Post> { ErrorMessage = "PostRepository->GetDetailedPost: " + e.Message };
            }
            return new Result<Post> { ErrorMessage = "PostRepository->GetDetailedPost: detailed post not found" };
        }

        public Result<OverviewManyPostsDto> GetOverviewPost(GetOverviewMantPostsDto getPostsDto)
        {
            if (getPostsDto.Tags == null || getPostsDto.Tags.Count == 0)
            {
                return TryGetOverviewPostWithoutTags(getPostsDto);
            }
            else
            {
                return TryGetOverviewPostWithTags(getPostsDto);
            }
        }

        public Result<OverviewManyPostsDto> TryGetOverviewPostWithTags(GetOverviewMantPostsDto getPostsDto)
        {

            OverviewManyPostsDto overview = new OverviewManyPostsDto();
            overview.Posts = new List<PostDto>();
            overview.UsedTags = getPostsDto.Tags;
            //SELECT DISTINCT `posts`.`post_id` FROM `posts` INNER JOIN `posttag` on `posttag`.`post_id` = `posts`.`post_id` INNER JOIN `tag` on `tag`.`tag_id` = `posttag`.`tag_id` WHERE(`tag`.`tag_id` in (1, 3) and posts.user_id = 5) GROUP by(`posts`.`post_id`) HAVING COUNT(DISTINCT `tag`.`tag_id`) = 2;
            using (MySqlConnection con = new MySqlConnection(CS))
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand("SELECT * FROM test", con);
                    string FinalCommand = "SELECT DISTINCT posts.title, posts.post_date, posts.main_image_url, posts.post_id FROM posts INNER JOIN posttag on posttag.post_id = posts.post_id INNER JOIN tag on tag.tag_id = posttag.tag_id WHERE(tag.tag_id in (";
                    bool first = true;
                    foreach (int id in getPostsDto.Tags)
                    {

                            if (first)
                            {
                                first = false;
                            }
                            else
                            {
                                FinalCommand += ", ";
                            }
                            FinalCommand += "@Tag_" + id;
                        cmd.Parameters.AddWithValue("@Tag_" + id, id);

                    }
                    FinalCommand += ") and posts.user_id = @userId) GROUP by(`posts`.`post_id`) HAVING COUNT(DISTINCT `tag`.`tag_id`) = @tagCount;";
                    cmd.Parameters.AddWithValue("@userId", getPostsDto.userId);
                    cmd.Parameters.AddWithValue("@tagCount", getPostsDto.Tags.Count());
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = FinalCommand;
                    con.Open();
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        PostDto post = new PostDto();
                        post.title = Convert.ToString(rdr["title"]);
                        post.postDate = Convert.ToDateTime(rdr["post_date"]);
                        post.mainImageUrl = Convert.ToString(rdr["main_image_url"]);
                        post.postId = Convert.ToInt32(rdr["post_id"]);
                        overview.Posts.Add(post);
                    }
                    con.Close();
                        return new Result<OverviewManyPostsDto> { Data= overview};
                }
                catch (Exception e)
                {
                    con.Close();
                    return new Result<OverviewManyPostsDto> { ErrorMessage = "PostRepository->TryGetOverviewPostWithTags: " + e.Message };

                }
            }
        }

        public Result<OverviewManyPostsDto> TryGetOverviewPostWithoutTags(GetOverviewMantPostsDto getPostsDto)
        {
            OverviewManyPostsDto overview = new OverviewManyPostsDto();
            overview.Posts = new List<PostDto>();
            overview.UsedTags = getPostsDto.Tags;
            //SELECT DISTINCT `posts`.`post_id` FROM `posts` INNER JOIN `posttag` on `posttag`.`post_id` = `posts`.`post_id` INNER JOIN `tag` on `tag`.`tag_id` = `posttag`.`tag_id` WHERE(`tag`.`tag_id` in (1, 3) and posts.user_id = 5) GROUP by(`posts`.`post_id`) HAVING COUNT(DISTINCT `tag`.`tag_id`) = 2;
            using (MySqlConnection con = new MySqlConnection(CS))
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand("SELECT * FROM test", con);
                    string FinalCommand = "SELECT DISTINCT posts.title, posts.post_date, posts.main_image_url, posts.post_id FROM posts WHERE(posts.user_id = @userId) GROUP by(`posts`.`post_id`)";
                    cmd.Parameters.AddWithValue("@userId", getPostsDto.userId);

                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = FinalCommand;
                    con.Open();
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        PostDto post = new PostDto();
                        post.title = Convert.ToString(rdr["title"]);
                        post.postDate = Convert.ToDateTime(rdr["post_date"]);
                        post.mainImageUrl = Convert.ToString(rdr["main_image_url"]);
                        post.postId = Convert.ToInt32(rdr["post_id"]);
                        overview.Posts.Add(post);
                    }
                    con.Close();
                    return new Result<OverviewManyPostsDto> { Data = overview };
                }
                catch (Exception e)
                {
                    con.Close();
                    return new Result<OverviewManyPostsDto> { ErrorMessage = "PostRepository->TryGetOverviewPostWithoutTags: " + e.Message };
                }
            }
        }
    
        public SimpleResult RemovePostToDB(int PostId)
    {
        throw new NotImplementedException();
    }
    }
}
