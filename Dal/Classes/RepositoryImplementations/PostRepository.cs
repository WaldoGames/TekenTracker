using Core.Classes;
using Core.Classes.DTO;
using Core.Classes.Models;
using Core.Interfaces.Repository;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Classes.RepositoryImplementations
{
    internal class PostRepository : IPostRepository
    {
        string CS = "SERVER=127.0.0.1;UID=root;PASSWORD=;DATABASE=tekentrackerdb";

        INoteRepository noteRepository;
        ISubImageRepository subImageRepository;
        ITagRepository tagRepository;
        public bool ChangeMainImageInDB(int PostId, string NewUrl, out string OldUrl)
        {
            throw new NotImplementedException();
        }

        public bool doesPostExist(int postId)
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
                    return true;
                }
                con.Close();

            }
            return false;
        }

        public bool TryAddNewPostToDB(NewPostDto post, out int PrimaryKeyValue)
        {
            PrimaryKeyValue = -1;
            try
            {
                using (MySqlConnection con = new MySqlConnection(CS))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("INSERT INTO users(username, password, email) VALUES(@user, @password, @email); SELECT LAST_INSERT_ID();", con);
                    cmd.Parameters.AddWithValue("@title", post.Title);
                    cmd.Parameters.AddWithValue("@url", post.ImageUrl);
                    cmd.Parameters.AddWithValue("@user", post.Poster.userId);
                    cmd.Parameters.AddWithValue("@date", DateTime.Now);
                    cmd.CommandType = CommandType.Text;
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    PrimaryKeyValue = (int)cmd.LastInsertedId;
                        con.Close();
                    return true;


                }
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public bool TryGetDetailedPost(int PostId, out Post post)
        {
            post = new Post();
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
                    post.user_Id = Convert.ToInt32(rdr["user_id "]);
                    post.postId = Convert.ToInt32(rdr["post_id "]);
                    con.Close();


                    if (noteRepository.TryGetNotesFromPost(PostId, out NotesDto notes))
                    {
                        post.notes = notes.Notes;
                    }
                    if (subImageRepository.TryGetSubimagesFromPost(PostId, out SubimagesDto images))
                    {
                        post.subImages = images.images;
                    }
                    if (tagRepository.TryGetTagsFromPost(PostId, out List<Tag> tags))
                    {
                        post.tags = tags;
                    }

                    return true;
                }
                con.Close();

            }
            return false;
        }

        public bool TryGetOverviewPost(GetOverviewMantPostsDto getPostsDto, out OverviewManyPostsDto overview)
        {
            throw new NotImplementedException();
        }

        public bool TryRemovePostToDB(int PostId)
        {
            throw new NotImplementedException();
        }
    }
}
