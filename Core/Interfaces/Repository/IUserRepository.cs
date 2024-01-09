using Core.Classes;
using Core.Classes.DTO;
using Core.Classes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repository
{   
    public interface IUserRepository
    {
        public SimpleResult AddUserToDB(NewUserDto newUser);
        public SimpleResult RemoveUserFromDB(int userId);
      //public bool tryGetAccountToken(int UserId, out CheckAccountTokenDTO AccountToken);
        public Result<CheckAccountTokenDTO> AddNewAccountTokenToDB(int userId);

        public Result<bool> DoesUserExistInDB(string username);
        public Result<User> GetUser(string username);

        public Result<bool> IsTokenValid(string username, string token);

    }
}   
