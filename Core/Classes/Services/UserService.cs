using Core.Classes.DTO;
using Core.Classes.Enums;
using Core.Classes.Models;
using Core.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Classes.Services
{
    public class UserService
    {
        IUserRepository repository;

        public UserService(IUserRepository repository)
        {
            this.repository = repository;
        }
        public Result<bool> ChecKLoginStatus(UserDto user)
        {
            return repository.IsTokenValid(user.UserName, user.Token);
        }
        public Result<LoginDto> TryLogin(string username, string password)
        {

            Encryption encryption = new Encryption();
            Result<User> user;
            LoginDto loginDto;

            Result<bool> DoesUserExist = repository.DoesUserExistInDB(username);

            if (DoesUserExist.IsFailed)
            {
                return new Result<LoginDto> { ErrorMessage = "UserService->Trylogin: error passed form UserRepository->DoesUserExistInDB" };
            }
            if (!DoesUserExist.Data)
            {
                return new Result<LoginDto> { Data = new LoginDto { IsLoggedIn = false } };
            }

            user = repository.GetUser(username);
            //continu here
            if (user.IsFailed)
            {
                return new Result<LoginDto> { ErrorMessage = "UserService->Trylogin: error passed form UserRepository->GetUser" };
            }
            if (user.Data == null)
            {
                return new Result<LoginDto> { ErrorMessage = "UserService->Trylogin: User could not be found after inital user check" };
            }

            if (!encryption.CompareEncryptedString(password, user.Data.Password))
            {
                return new Result<LoginDto> { Data = new LoginDto { IsLoggedIn = false } };
            }
            Result<CheckAccountTokenDTO> tokendto= repository.AddNewAccountTokenToDB(user.Data.UserId);

            if (tokendto.IsFailed){
                return new Result<LoginDto> { ErrorMessage = "UserService->Trylogin: error passed form UserRepository->AddNewAccountTokenToDB" };
            }
            if(tokendto.Data == null)
            {
                return new Result<LoginDto> { ErrorMessage = "UserService->Trylogin: token data returned null" };
            }

            loginDto = new LoginDto();

            UserDto userDto = new UserDto();
            userDto.Email = user.Data.Email;
            userDto.Token = tokendto.Data.Token;
            userDto.UserName = user.Data.UserName;
            userDto.UserId = user.Data.UserId;

            return new Result<LoginDto> { Data = new LoginDto { IsLoggedIn = true, User = userDto } };

        }
        public UserCreationEnum CreateNewUser(NewUserDto newUser)
        {
            UserCreationEnum userCreationEnum = new UserCreationEnum();

            if(string.IsNullOrEmpty(newUser.Email) || string.IsNullOrEmpty(newUser.Password) || string.IsNullOrEmpty(newUser.UserName))
            {
                userCreationEnum = UserCreationEnum.failed;
                return userCreationEnum;
            }

            Result<bool> doesUserExist = repository.DoesUserExistInDB(newUser.UserName);

            if (doesUserExist.IsFailed)
            {
                userCreationEnum = UserCreationEnum.failed;

                return userCreationEnum;
            }
                
            if (doesUserExist.Data)
            {
                userCreationEnum = UserCreationEnum.usernameTaken;

                return userCreationEnum;
            }

            if (!repository.AddUserToDB(newUser).IsFailed)
            {
                userCreationEnum = UserCreationEnum.created;

                return userCreationEnum;
            }

            userCreationEnum = UserCreationEnum.failed;

            return userCreationEnum;
        }
    }
}
