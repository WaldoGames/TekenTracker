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
        public bool DoesUserExistInDB(string UserName, out bool DoesUserExist)
        {
            DoesUserExist = false;
            try
            {
                using (MySqlConnection con = new MySqlConnection(CS))
                {
                    MySqlCommand cmd = new MySqlCommand("SELECT * FROM users WHERE username = @username LIMIT 1", con);
                    cmd.Parameters.AddWithValue("@username", UserName);
                    cmd.CommandType = CommandType.Text;
                    con.Open();

                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        DoesUserExist = true;
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

        public bool IsTokenValid(string Username, string Token)
        {
            TryGetUser(Username, out User? user);

            if (user != null)
            {
                using (MySqlConnection con = new MySqlConnection(CS))
                {
                    MySqlCommand cmd = new MySqlCommand("SELECT remember_token,valid_until FROM users WHERE user_id = @UserId LIMIT 1", con);
                    cmd.Parameters.AddWithValue("@UserId", user.userId);
                    cmd.CommandType = CommandType.Text;
                    con.Open();

                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {

                        string DBtoken = Convert.ToString(rdr["remember_token"]);
                        DateTime dateTime = Convert.ToDateTime(rdr["valid_until"]);
                        con.Close();
                        if (Token != DBtoken || DateTime.Now > dateTime)
                        {
                            return false;
                        }


                        return true;
                    }
                    con.Close();

                }
               
            }
            return false;
        }

        public bool tryAddNewAccountTokenToDB(int UserId, out CheckAccountTokenDTO AccountToken)
        {
            AccountToken = new CheckAccountTokenDTO();
            try
            {
                using (MySqlConnection con = new MySqlConnection(CS))
                {
                    TokenGenerator tokenGenerator = new TokenGenerator();
                    AccountToken.Token = tokenGenerator.GenerateToken();
                    AccountToken.ValidUntil = DateTime.Now.AddHours(2);
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("UPDATE users SET remember_token = @newToken, valid_until = @until WHERE user_id = @UserId ", con);
                    cmd.Parameters.AddWithValue("@UserId", UserId);
                    cmd.Parameters.AddWithValue("@newToken", AccountToken.Token);
                    cmd.Parameters.AddWithValue("@until", AccountToken.ValidUntil);

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

        public bool tryAddUserToDB(NewUserDto newUser)
        {
            Encryption encryption = new Encryption();
            try
            {
                using (MySqlConnection con = new MySqlConnection(CS))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("INSERT INTO users(username, password, email) VALUES(@user, @password, @email)", con);
                    cmd.Parameters.AddWithValue("@user", newUser.userName);
                    cmd.Parameters.AddWithValue("@password", encryption.EncryptNewString(newUser.password));
                    cmd.Parameters.AddWithValue("@email", newUser.email);

                    cmd.ExecuteNonQuery();
                    cmd.CommandType = CommandType.Text;

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

        public bool TryGetUser(string Username, out User? user)
        {
            user = null;
            try
            {
                using (MySqlConnection con = new MySqlConnection(CS))
                {
                    MySqlCommand cmd = new MySqlCommand("SELECT * FROM users WHERE username = @username LIMIT 1", con);
                    cmd.Parameters.AddWithValue("@username", Username);
                    cmd.CommandType = CommandType.Text;
                    con.Open();

                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        user = new User();
                        user.userId = Convert.ToInt32(rdr["user_id"]);
                        user.userName = Convert.ToString(rdr["username"]);
                        user.password = Convert.ToString(rdr["password"]);
                        user.email = Convert.ToString(rdr["email"]);

                        TokenGenerator tokenGenerator = new TokenGenerator();
                        user.Token = tokenGenerator.GenerateToken();
                        user.TokenValidUntil = DateTime.Now.AddHours(1);
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

        public bool tryRemoveUserFromDB(int UserId)
        {
            throw new NotImplementedException();
        }
    }
}
