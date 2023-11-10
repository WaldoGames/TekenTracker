using Core.Classes.DTO;
using Core.Classes.Enums;
using Core.Classes.Models;
using Core.Interfaces.Repository;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Dal.Classes.RepositoryImplementations
{
    public class TagRepository : ITagRepository
    {
        string CS = "SERVER=127.0.0.1;UID=root;PASSWORD=;DATABASE=tekentrackerdb";

        public bool DoesPostHaveTag(int postId, int tagId)
        {

            bool returnVal = false;

            using (MySqlConnection con = new MySqlConnection(CS))
            {
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM posttag WHERE(post_id = @postid && tag_id = @tagId)", con);
                cmd.Parameters.AddWithValue("@postid", postId);
                cmd.Parameters.AddWithValue("@tagId", tagId);
                cmd.CommandType = CommandType.Text;
                con.Open();

                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    returnVal = true;
                }
                con.Close();
                return returnVal;

            }
        }

        public bool TryAddNewTagToDB(string tagName)
        {
            throw new NotImplementedException();
        }

        public bool TryAddTagToPost(int postId, int tagId)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(CS))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT * FROM test", con);
                    cmd.CommandText = "INSERT INTO posttag(post_id,tag_id) VALUES(@postId, @tagId)";
                    cmd.Parameters.AddWithValue("@postId", postId);
                    cmd.Parameters.AddWithValue("@tagId", tagId);

                    cmd.ExecuteNonQuery();
                    cmd.CommandType = CommandType.Text;

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

        public bool TryGetAllTags(out List<Tag>? tags)
        {
            tags = null;
            try
            {
                tags = new List<Tag>();
                using (MySqlConnection con = new MySqlConnection(CS))
                {
                    MySqlCommand cmd = new MySqlCommand("SELECT * FROM tag", con);
                    cmd.CommandType = CommandType.Text;
                    con.Open();

                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        var tag = new Tag();
                        tag.name = Convert.ToString(rdr["title"]);
                        tag.tagId = Convert.ToInt32(rdr["tag_id"]);
                        int tmp = Convert.ToInt32(rdr["type"]);
                        tag.type = (TagTypes)tmp;
                        tags.Add(tag);
                    }
                    con.Close();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }

        }

        public bool TryGetTags(GetTagsDto getTagsDto, out List<Tag> tags)
        {
            throw new NotImplementedException();
        }

        public bool TryGetTagsFromPost(int postId, out List<Tag> tags)
        {
            tags = new List<Tag>();

            try
            {
                using (MySqlConnection con = new MySqlConnection(CS))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT * FROM test", con);

                    cmd.CommandText = "SELECT tag.tag_id as tagId, tag.title as title, tag.type as type FROM ((posts INNER JOIN posttag ON posttag.post_id = posts.post_id) INNER JOIN tag ON posttag.tag_id = tag.tag_id) WHERE posts.post_id = @Postid";
                    cmd.Parameters.AddWithValue("@Postid", postId);
                    cmd.CommandType = CommandType.Text;

                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        var tag = new Tag();
                        tag.name = Convert.ToString(rdr["title"]);
                        tag.tagId = Convert.ToInt32(rdr["tagId"]);
                        int tmp = Convert.ToInt32(rdr["type"]);
                        tag.type = (TagTypes)tmp;
                        tags.Add(tag);
                    }

                    con.Close();
                    return true;
                }
            }
            catch (Exception)
            {
                //return false;
                throw;
            }
        }

        public bool TryGetTagsUsedByUser(int userId, out List<Tag> tags)
        {
            tags = new List<Tag>();

            try
            {
                using (MySqlConnection con = new MySqlConnection(CS))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT * FROM test", con);

                    cmd.CommandText = "SELECT DISTINCT tag.tag_id as tagId, tag.title as title, tag.type as type FROM ((posts INNER JOIN posttag ON posttag.post_id = posts.post_id) INNER JOIN tag ON posttag.tag_id = tag.tag_id) WHERE posts.user_id = @userid";
                    cmd.Parameters.AddWithValue("@userid", userId);
                    cmd.CommandType = CommandType.Text;

                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        var tag = new Tag();
                        tag.name = Convert.ToString(rdr["title"]);
                        tag.tagId = Convert.ToInt32(rdr["tagId"]);
                        int tmp = Convert.ToInt32(rdr["type"]);
                        tag.type = (TagTypes)tmp;
                        tags.Add(tag);
                    }

                    con.Close();
                    return true;
                }
            }
            catch (Exception)
            {
                //return false;
                throw;
            }
        }

        public bool TryRemoveStringFromDB(int tagId)
        {
            throw new NotImplementedException();
        }

        public bool TryRemoveTagFromPost(int postId, int tagId)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(CS))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT * FROM test", con);
                    cmd.CommandText = "DELETE FROM posttag WHERE(post_id = @postId && tag_id = @tagId)";
                    cmd.Parameters.AddWithValue("@postId",postId );
                    cmd.Parameters.AddWithValue("@tagId", tagId);

                    cmd.ExecuteNonQuery();
                    cmd.CommandType = CommandType.Text;

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
    }
}
