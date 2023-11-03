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
        public bool tryAddUserToDB(NewUserDto newUser);
        public bool tryRemoveUserFromDB(int UserId);
      //public bool tryGetAccountToken(int UserId, out CheckAccountTokenDTO AccountToken);
        public bool tryAddNewAccountTokenToDB(int UserId, out CheckAccountTokenDTO AccountToken);

        public bool DoesUserExistInDB(string UserName, out bool DoesUserExist);
        public bool TryGetUser(string Username, out User? user);

        public bool IsTokenValid(string Username, string Token);

    }
}   
