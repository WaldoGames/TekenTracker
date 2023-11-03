using Core.Classes.DTO;
using Core.Classes.Enums;
using Core.Classes.Models;
using Core.Interfaces.Repository;
using System;
using System.Collections.Generic;
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
        public bool ChecKLoginStatus(UserDto user)
        {
            return repository.IsTokenValid(user.userName, user.Token);
        }
        public bool tmpCheckPasswords(string password)
        {
            Encryption encryption = new Encryption();

            return encryption.CompareEncryptedString(password, encryption.EncryptNewString(password));
        }
        public bool TryLogin(string username, string password, out UserDto userDto)
        {
            bool tmp = tmpCheckPasswords(password);

            Encryption encryption = new Encryption();
            userDto= new UserDto();

            if(repository.DoesUserExistInDB(username, out bool Exists) && Exists)
            {
                if(repository.TryGetUser(username, out User user)){

                    if (user != null)
                    {
                        if (encryption.CompareEncryptedString(password, user.password))
                        {
                            repository.tryAddNewAccountTokenToDB(user.userId, out CheckAccountTokenDTO accountToken);

                            userDto.email = user.email;
                            userDto.Token = accountToken.Token;
                            userDto.userName = user.userName;
                            userDto.userId = user.userId;

                            return true;
                        }
                        else
                        {
                            user = null;

                        }
                    }
                }

            }
            return false;

        }

        public UserCreationEnum CreateNewUser(NewUserDto newUser)
        {
            UserCreationEnum userCreationEnum = new UserCreationEnum();

            if (repository.DoesUserExistInDB(newUser.userName, out bool doesUserExist))
            {
                
                if (doesUserExist)
                {
                    userCreationEnum = UserCreationEnum.usernameTaken;
                }
                else
                {
                    if (repository.tryAddUserToDB(newUser))
                    {
                        userCreationEnum = UserCreationEnum.created;
                    }
                    else
                    {
                        userCreationEnum = UserCreationEnum.failed;
                    }
                }
            }



            return userCreationEnum;
        }
    }
}
