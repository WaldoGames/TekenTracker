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


    public class UserRepository : IUserRepository
    {
        string CS = "SERVER=127.0.0.1;UID=root;PASSWORD=;DATABASE=tekentrackerdb";
        public Result<bool> DoesUserExistInDB(string userName)
        {
            bool doesUserExist = false;
            try
            {
                using (MySqlConnection con = new MySqlConnection(CS))
                {
                    MySqlCommand cmd = new MySqlCommand("SELECT * FROM users WHERE username = @username LIMIT 1", con);
                    cmd.Parameters.AddWithValue("@username", userName);
                    cmd.CommandType = CommandType.Text;
                    con.Open();

                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        doesUserExist = true;
                    }
                    con.Close();

                }
                return new Result<bool> { Data = doesUserExist };
            }
            catch (Exception e)
            {
                return new Result<bool> { ErrorMessage = "UserRepository->DoesUserExistInDB: " + e.Message };
            }
        }

        public Result<bool> IsTokenValid(string username, string token)
        {
            try
            {
            Result<User> user = GetUser(username);

                if (user.Data != null && user.IsFailed == false)
                {
                    using (MySqlConnection con = new MySqlConnection(CS))
                    {
                        MySqlCommand cmd = new MySqlCommand("SELECT remember_token,valid_until FROM users WHERE user_id = @UserId LIMIT 1", con);
                        cmd.Parameters.AddWithValue("@UserId", user.Data.UserId);
                        cmd.CommandType = CommandType.Text;
                        con.Open();

                        MySqlDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {

                            string DBtoken = Convert.ToString(rdr["remember_token"]);
                            DateTime dateTime = Convert.ToDateTime(rdr["valid_until"]);
                            con.Close();
                            if (token != DBtoken || DateTime.Now > dateTime)
                            {
                                return new Result<bool> { Data = false };
                            }


                            return new Result<bool> { Data = true };
                        }
                        con.Close();

                    }
               
                }
            }
            catch (Exception e)
            {
                return new Result<bool> { ErrorMessage = "UserRepository->IsTokenValid: " + e.Message };
            }
            return new Result<bool> { ErrorMessage = "UserRepository->IsTokenValid: " + "unknown error(likly the user could not be found)" };
        }

        public Result<CheckAccountTokenDTO> AddNewAccountTokenToDB(int userId)
        {
            CheckAccountTokenDTO accountToken = new CheckAccountTokenDTO();
            try
            {
                using (MySqlConnection con = new MySqlConnection(CS))
                {
                    TokenGenerator tokenGenerator = new TokenGenerator();
                    accountToken.Token = tokenGenerator.GenerateToken();
                    accountToken.ValidUntil = DateTime.Now.AddHours(2);
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("UPDATE users SET remember_token = @newToken, valid_until = @until WHERE user_id = @UserId ", con);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@newToken", accountToken.Token);
                    cmd.Parameters.AddWithValue("@until", accountToken.ValidUntil);

                    cmd.ExecuteNonQuery();
                    cmd.CommandType = CommandType.Text;

                    con.Close();
                    return new Result<CheckAccountTokenDTO> { Data = accountToken };
                }
            }
            catch (Exception e)
            {
                return new Result<CheckAccountTokenDTO> { ErrorMessage = "UserRepository->AddNewAccountTokenToDB: "+ e.Message };
            }
        }

        public SimpleResult AddUserToDB(NewUserDto newUser)
        {
            Encryption encryption = new Encryption();
            try
            {
                using (MySqlConnection con = new MySqlConnection(CS))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("INSERT INTO users(username, password, email) VALUES(@user, @password, @email)", con);
                    cmd.Parameters.AddWithValue("@user", newUser.UserName);
                    cmd.Parameters.AddWithValue("@password", encryption.EncryptNewString(newUser.Password));
                    cmd.Parameters.AddWithValue("@email", newUser.Email);

                    cmd.ExecuteNonQuery();
                    cmd.CommandType = CommandType.Text;

                    con.Close();
                    return new SimpleResult();
                }
            }
            catch (Exception e)
            {
                return new SimpleResult { ErrorMessage = "UserRepository->AddUserToDB: " + e.Message };
                throw;
            }
        }

        public Result<User> GetUser(string username)
        {
            try
            {
                User user = new User();
                using (MySqlConnection con = new MySqlConnection(CS))
                {
                   
                    MySqlCommand cmd = new MySqlCommand("SELECT * FROM users WHERE username = @username LIMIT 1", con);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.CommandType = CommandType.Text;
                    con.Open();

                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {

                        user.UserId = Convert.ToInt32(rdr["user_id"]);
                        user.UserName = Convert.ToString(rdr["username"]);
                        user.Password = Convert.ToString(rdr["password"]);
                        user.Email = Convert.ToString(rdr["email"]);

                        TokenGenerator tokenGenerator = new TokenGenerator();
                        user.Token = tokenGenerator.GenerateToken();
                        user.TokenValidUntil = DateTime.Now.AddHours(1);
                    }
                    con.Close();
                }
                return new Result<User> { Data = user };
            }
            catch (Exception e)
            {
                return new Result<User> { ErrorMessage = "UserRepository->GetUser: " + e.Message };
                throw;
            }

        }
        public SimpleResult RemoveUserFromDB(int userId)
        {
            throw new NotImplementedException();
        }


    }
}
