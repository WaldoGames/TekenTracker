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

        public Result<bool> DoesUserExistInDB(string UserName)
        {
            if (container.users.Select(u => u.UserName).Contains(UserName))
            {
                return new Result<bool> { Data = true };
            }
            else
            {
                return new Result<bool> { Data = false };
            }
        }

        public Result<bool> IsTokenValid(string Username, string Token)
        {
            User user = container.users.Where(u => u.UserName == Username).First();

            if (user.Token == Token && user.TokenValidUntil > DateTime.Now)
            {
                return new Result<bool> { Data = true };
            }
            return new Result<bool> { Data = false };
        }

        public Result<CheckAccountTokenDTO> AddNewAccountTokenToDB(int UserId)
        {

                CheckAccountTokenDTO AccountToken = new CheckAccountTokenDTO();

                AccountToken.Token = ("token-"+UserId);
                AccountToken.ValidUntil = DateTime.Now.AddSeconds(60);

                User user = container.users.Where(u => u.UserId==UserId).First();
                user.Token = AccountToken.Token;
                user.TokenValidUntil = AccountToken.ValidUntil;
            return new Result<CheckAccountTokenDTO> { Data = AccountToken };

        }

        public void testMoveTokenValidTimeToPast(int UserId)
        {
            User user = container.users.Where(u => u.UserId == UserId).First();

            user.TokenValidUntil = DateTime.Now.AddSeconds(-600);
        }

        public SimpleResult AddUserToDB(NewUserDto newUser)
        {
            User user = new User();
            Encryption encryption = new Encryption();

            user.UserId = container.users.Select(u => u.UserId).Max()+1;
            user.UserName = newUser.UserName;
            user.Password = encryption.EncryptNewString(newUser.Password);
            user.Email = newUser.Email;

            container.users.Add(user);

            return new SimpleResult();
        }

        public Result<User> GetUser(string Username)
        {
            User user;
            if (container.users.Select(u => u.UserName).Contains(Username))
            {
                user = container.users.Where(u => u.UserName == Username).First();
                return new Result<User> { Data = user };
            }
            else
            {
                user = null;
                return new Result<User> { ErrorMessage = "user not found[unit tests]"};
            }
        }

        public SimpleResult RemoveUserFromDB(int UserId)
        {
            container.users.Remove(container.users.Where(u=>u.UserId == UserId).First());

            if(!container.users.Select(u => u.UserId).Contains(UserId))
            {

                return new SimpleResult();
            }
            else
            {

                return new SimpleResult { ErrorMessage = "something went wrong with removing an user[unit tests]" };
            }
        }
    }
}
