using Core.Classes;
using Core.Classes.DTO;
using Core.Classes.Models;
using Core.Interfaces.Repository;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Classes.RepositoryImplementations
{
    public class SubimageRepository: ISubImageRepository
    {
        string CS = "SERVER=127.0.0.1;UID=root;PASSWORD=;DATABASE=tekentrackerdb";

        public Result<bool> DoesSubimageExist(int SubimageId)
        {
            throw new NotImplementedException();
        }

        public SimpleResult AddNewSubimage(int PostId, string NewUrl)
        {

            try
            {
                using (MySqlConnection con = new MySqlConnection(CS))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("INSERT INTO subimage(image_url , upload_date, post_id) VALUES(@NewUrl, @date, @postId)", con);
                    cmd.Parameters.AddWithValue("@postId", PostId);
                    cmd.Parameters.AddWithValue("@newUrl", NewUrl);
                    cmd.Parameters.AddWithValue("@date", DateTime.Now);
                    cmd.CommandType = CommandType.Text;
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    con.Close();
                    return new SimpleResult();
                }
            }
            catch (Exception e)
            {
                return new SimpleResult { ErrorMessage = "SubimageRepository->AddNewSubimage: "+ e.Message };
                throw;
            }
            
        }

        public Result<SubimagesDto> GetSubimagesFromPost(int PostId)
        {
            SubimagesDto subimages = new SubimagesDto();
            subimages.images = new List<SubImage>();
            
            using (MySqlConnection con = new MySqlConnection(CS))
            {
                try
                {


                MySqlCommand cmd = new MySqlCommand("SELECT * FROM subimage WHERE post_id = @id", con);
                cmd.Parameters.AddWithValue("@id", PostId);
                cmd.CommandType = CommandType.Text;
                con.Open();

                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (subimages == null)
                    {
                        subimages = new SubimagesDto();
                        subimages.images = new List<SubImage>();
                    }


                    SubImage image = new SubImage();
                    image.imageUrl = Convert.ToString(rdr["image_url"]);
                    image.uploadDate = Convert.ToDateTime(rdr["upload_date"]);
                    image.subimageId = Convert.ToInt32(rdr["subimage_id"]);
                    subimages.images.Add(image);
                    //con.Close();

                }

                con.Close();
                    return new Result<SubimagesDto> { Data = subimages } ;
                }
                catch (Exception e)
                {
                    con.Close();
                    return new Result<SubimagesDto> { ErrorMessage = "SubimageRepositry->TryGetSubimagesFromPost: "+ e.Message }; ;
                }
            }
            
        }

        public SimpleResult RemoveNewSubimage(int SubimageId)
        {
            throw new NotImplementedException();
        }

        public SimpleResult UpdateSubimage(int PostId, string UpdatesUrl)
        {
            throw new NotImplementedException();
        }
    }
}
