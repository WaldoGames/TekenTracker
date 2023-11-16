using Core.Classes;
using Core.Classes.DTO;
using Core.Classes.Models;
using Core.Classes.Services;
using Core.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TekenTracker.UnitTests.FakeDB
{
    internal class UserRepository : IUserRepository
    {

        DataContainer container = new DataContainer();

        public bool DoesUserExistInDB(string UserName, out bool DoesUserExist)
        {
            if (container.users.Select(u => u.userName).Contains(UserName))
            {
                DoesUserExist = true;
            }
            else
            {
                DoesUserExist = false;
            }

            return true;
        }

        public bool IsTokenValid(string Username, string Token)
        {
            User user = container.users.Where(u => u.userName == Username).First();

            if (user.Token == Token && user.TokenValidUntil > DateTime.Now)
            {
                return true;    
            }
            return false;
        }

        public bool tryAddNewAccountTokenToDB(int UserId, out CheckAccountTokenDTO AccountToken)
        {

                AccountToken = new CheckAccountTokenDTO();

                AccountToken.Token = ("token-"+UserId);
                AccountToken.ValidUntil = DateTime.Now.AddSeconds(60);

                User user = container.users.Where(u => u.userId==UserId).First();
                user.Token = AccountToken.Token;
                user.TokenValidUntil = AccountToken.ValidUntil;
                return true;

        }

        public void testMoveTokenValidTimeToPast(int UserId)
        {
            User user = container.users.Where(u => u.userId == UserId).First();

            user.TokenValidUntil = DateTime.Now.AddSeconds(-600);
        }

        public bool tryAddUserToDB(NewUserDto newUser)
        {
            User user = new User();
            Encryption encryption = new Encryption();

            user.userId = container.users.Select(u => u.userId).Max()+1;
            user.userName = newUser.userName;
            user.password = encryption.EncryptNewString(newUser.password);
            user.email = newUser.email;

            container.users.Add(user);

            return true;
        }

        public bool TryGetUser(string Username, out User? user)
        {
            if (container.users.Select(u => u.userName).Contains(Username))
            {
                user = container.users.Where(u => u.userName == Username).First();
                return true;
            }
            else
            {
                user = null;
                return false;
            }
        }

        public bool tryRemoveUserFromDB(int UserId)
        {
            container.users.Remove(container.users.Where(u=>u.userId == UserId).First());

            return !container.users.Select(u => u.userId).Contains(UserId);
        }
    }
}
