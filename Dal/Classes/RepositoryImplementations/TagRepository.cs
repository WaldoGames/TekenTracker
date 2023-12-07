using Core.Classes;
using Core.Classes.DTO;
using Core.Classes.Enums;
using Core.Classes.Models;
using Core.Interfaces.Repository;
using Google.Protobuf.Collections;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.X509;
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
    public class TagRepository : ITagRepository
    {
        string CS = "SERVER=127.0.0.1;UID=root;PASSWORD=;DATABASE=tekentrackerdb";

        public Result<bool> DoesPostHaveTag(int postId, int tagId)
        {
            try
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
                    return new Result<bool> { Data = returnVal };

                }
            }
            catch (Exception e)
            {
                return new Result<bool> { ErrorMessage = "TagRepository->DoesPostHaveTag:" + e.Message };
            }
        }

        public SimpleResult AddNewTagToDB(string tagName, int type)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(CS))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT * FROM test", con);
                    cmd.CommandText = "INSERT INTO tag(title,type) VALUES(@tagName, @type)";
                    cmd.Parameters.AddWithValue("@tagName", tagName);
                    cmd.Parameters.AddWithValue("@type", type);

                    cmd.ExecuteNonQuery();
                    cmd.CommandType = CommandType.Text;

                    con.Close();
                    return new SimpleResult();
                }
            }
            catch (Exception e)
            {
                return new SimpleResult { ErrorMessage = "TagRepository->AddNewTagToDB: " + e.Message };
                throw;
            }
        }

        public SimpleResult AddTagToPost(int postId, int tagId)
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
                    return new SimpleResult();
                }
            }
            catch (Exception e)
            {
                return new SimpleResult { ErrorMessage = "TagRepository->AddTagToPost: "+ e.Message };
                throw;
            }
        }

        public Result<List<Tag>> GetAllTags()
        {

            try
            {
                List<Tag> tags = new List<Tag>();
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
                return new Result<List<Tag>> { Data = tags };
            }
            catch (Exception e)
            {
                return new Result<List<Tag>> { ErrorMessage = "TagRepository->GetAllTags: " + e.Message };
                throw;
            }

        }

        public Result<List<Tag>> GetTags(GetTagsDto getTagsDto)
        {
            throw new NotImplementedException();
        }

        public Result<List<Tag>> GetTagsFromPost(int postId)
        {
            List<Tag>  tags = new List<Tag>();

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
                    return new Result<List<Tag>> { Data = tags };
                }
            }
            catch (Exception e)
            {
                return new Result<List<Tag>> { ErrorMessage = "TagRepository->GetTagsFromPost: " + e.Message };
            }
        }

        public Result<List<Tag>> GetTagsUsedByUser(int userId)
        {
            List<Tag>  tags = new List<Tag>();

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
                    return new Result<List<Tag>> { Data = tags };
                }
            }
            catch (Exception e)
            {
                return new Result<List<Tag>> { ErrorMessage = "TagRepository->TryGetTagsUsedByUser: " + e.Message };
            }
        }

        public SimpleResult RemoveStringFromDB(int tagId)
        {
            throw new NotImplementedException();
        }

        public SimpleResult RemoveTagFromPost(int postId, int tagId)
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
                    return new SimpleResult();
                }
            }
            catch (Exception e)
            {
                return new SimpleResult { ErrorMessage = "TagRepository->TryRemoveTagFromPost: " + e.Message };
            }
        }
        public Result<List<TagAndAmount>> GetSearchTagsInLastNumberOfPost(int Reach, int UserId)
        {
            List<TagAndAmount> TagsAndAmount = new List<TagAndAmount>();

            try
            {
                using (MySqlConnection con = new MySqlConnection(CS))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT * FROM test", con);

                    cmd.CommandText =   "SELECT" +
                                        "COUNT(`tag`.`tag_id`) as `count`," +
                                        "`tag`.`tag_id` as `tagId`,`tag`.`title` as `title`, " +
                                        "`tag`.`type` as `type` FROM((SELECT `post_id` FROM `posts` WHERE `user_id` = @userid" +
                                        "ORDER BY `post_date` DESC" +
                                        "LIMIT @limit) AS `recent_posts`" +
                                        "INNER JOIN `posttag` ON `posttag`.`post_id` = `recent_posts`.`post_id`)" +
                                        "INNER JOIN `tag` ON `posttag`.`tag_id` = `tag`.`tag_id`" +
                                        "WHERE `tag`.`type` = 2" +
                                        "GROUP BY `tag`.`tag_id` ORDER BY COUNT(`tag`.`tag_id`) DESC;";
                    cmd.Parameters.AddWithValue("@userid", UserId); 
                    cmd.Parameters.AddWithValue("@limit", Reach);
                    cmd.CommandType = CommandType.Text;

                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        //TagAndAmount
                        var tag = new Tag();
                        tag.name = Convert.ToString(rdr["title"]);
                        tag.tagId = Convert.ToInt32(rdr["tagId"]);
                        int tmp = Convert.ToInt32(rdr["type"]);
                        tag.type = (TagTypes)tmp;

                        TagAndAmount tagAndAmount = new TagAndAmount();
                        tagAndAmount.tag = tag;
                        tagAndAmount.count = Convert.ToInt32(rdr["count"]);

                        TagsAndAmount.Add(tagAndAmount);
                    }

                    con.Close();
                    return new Result<List<TagAndAmount>> { Data = TagsAndAmount };
                }
            }
            catch (Exception e)
            {
                return new Result<List<TagAndAmount>> { ErrorMessage = "TagRepository->GetSearchTagsInLastNumberOfPost: " + e.Message };
            }
        }

        public Result<List<TagAndAmount>> GetSearchTagsInLastNumberOfDays(int Reach, int UserId)
        {
            List<TagAndAmount> TagsAndAmount = new List<TagAndAmount>();

            try
            {
                using (MySqlConnection con = new MySqlConnection(CS))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT * FROM test", con);

                    cmd.CommandText =   "SELECT"+
                                        "COUNT(`tag`.`tag_id`) as `count`,"+
                                            "`tag`.`tag_id` as `tagId`," +
                                            "`tag`.`title` as `title`," +
                                            "`tag`.`type` as `type`" +
                                        "FROM((SELECT `post_id`" +
                                                 "FROM `posts`" +
                                                 "WHERE `user_id` = @userid" +
                                                    "AND `post_date` >= DATE_SUB(NOW(), INTERVAL @limit DAY)" +
                                                 "ORDER BY `post_date` DESC) AS `recent_posts`" +
                                                "INNER JOIN `posttag` ON `posttag`.`post_id` = `recent_posts`.`post_id`)" +
                                            "INNER JOIN `tag` ON `posttag`.`tag_id` = `tag`.`tag_id`" +
                                        "WHERE `tag`.`type` = 2" +
                                        "GROUP BY `tag`.`tag_id`" +
                                        "ORDER BY COUNT(`tag`.`tag_id`) DESC; ";
                    cmd.Parameters.AddWithValue("@userid", UserId);
                    cmd.Parameters.AddWithValue("@limit", Reach);
                    cmd.CommandType = CommandType.Text;

                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        //TagAndAmount
                        var tag = new Tag();
                        tag.name = Convert.ToString(rdr["title"]);
                        tag.tagId = Convert.ToInt32(rdr["tagId"]);
                        int tmp = Convert.ToInt32(rdr["type"]);
                        tag.type = (TagTypes)tmp;

                        TagAndAmount tagAndAmount = new TagAndAmount();
                        tagAndAmount.tag = tag;
                        tagAndAmount.count = Convert.ToInt32(rdr["count"]);

                        TagsAndAmount.Add(tagAndAmount);
                    }

                    con.Close();
                    return new Result<List<TagAndAmount>> { Data = TagsAndAmount };
                }
            }
            catch (Exception e)
            {
                return new Result<List<TagAndAmount>> { ErrorMessage = "TagRepository->GetSearchTagsInLastNumberOfPost: " + e.Message };
            }
        }
        /*SELECT
                COUNT(`tag`.`tag_id`) as `count`,
                `tag`.`tag_id` as `tagId`,
                `tag`.`title` as `title`,
                `tag`.`type` as `type`
            FROM
                (
                    (SELECT `post_id`
                     FROM `posts`
                     WHERE `user_id` = @userid
                     ORDER BY `post_date` DESC
                     LIMIT @limit) AS `recent_posts`
                    INNER JOIN `posttag` ON `posttag`.`post_id` = `recent_posts`.`post_id`
                )
                INNER JOIN `tag` ON `posttag`.`tag_id` = `tag`.`tag_id`
            WHERE
                `tag`.`type` = 2
            GROUP BY
                `tag`.`tag_id`
            ORDER BY
                COUNT(`tag`.`tag_id`) DESC;

*/

        //SELECT COUNT(`tag`.`tag_id`) as `count`, `tag`.`tag_id` as `tagId`, `tag`.`title` as `title`, `tag`.`type` as `type` FROM ((`posts` INNER JOIN `posttag` ON `posttag`.`post_id` = `posts`.`post_id`) INNER JOIN `tag` ON `posttag`.`tag_id` = `tag`.`tag_id`) WHERE `posts`.`user_id`=10 GROUP BY `tag`.`tag_id`
    }
}
