using Core.Classes;
using Core.Classes.DTO;
using Core.Classes.Enums;
using Core.Classes.Services;
using Core.Interfaces.Repository;
using TekenTracker.UnitTests.FakeDB;

namespace TekenTracker.UnitTests
{
    public class UserAndUserRelatedFuntionsUnitTest
    {
        [Fact]
        public void encryption_directTest()
        {
            string pass = "helloWorldPassWord";

            Encryption enc = new Encryption();

            string encPass = enc.EncryptNewString(pass);

            Assert.True(enc.CompareEncryptedString(pass, encPass));
        }
        [Fact]
        public void encryption_PreGeneratedTest()
        {
            string pass = "helloWorldPassWord";
            string encPass = "$2a$11$2yuIcgA7BCvQyvMjl0FvpeCyfFV0d9EIaA7yPebJEfz13SL1eEvL.";

            Encryption enc = new Encryption();



            Assert.True(enc.CompareEncryptedString(pass, encPass));
        }

        [Fact]
        public void encryption_WrongPasswordTest()
        {
            string pass = "helloWorldNewPassword123";
            string encPass = "$2a$11$2yuIcgA7BCvQyvMjl0FvpeCyfFV0d9EIaA7yPebJEfz13SL1eEvL.";

            Encryption enc = new Encryption();
            Assert.False(enc.CompareEncryptedString(pass, encPass));
        }

        [Fact]
        public void TryLogin()
        {
            UserService userService = new UserService(new UserRepository());
            
            Assert.True(userService.TryLogin("FirstUser", "helloWorldPassWord").Data.IsLoggedIn);

        }

        [Fact] 
        public void TryLoginWithFakeUser()
        {

            UserService userService = new UserService(new UserRepository());
            
            Assert.False(userService.TryLogin("TotalyRealUser", "helloWorldPassWord").Data.IsLoggedIn);
        }

        [Fact]
        public void TryLoginWithWrongPassword()
        {
            UserService userService = new UserService(new UserRepository());

            Assert.False(userService.TryLogin("FirstUser", "p2ssw0r3").Data.IsLoggedIn);

        }

        [Fact]

        public void TryCreateNewUser()
        {
            NewUserDto newUser = new NewUserDto();
            newUser.email = "email";
            newUser.password = "password"; 
            newUser.userName = "NewUser";

            UserService userService = new UserService(new UserRepository());

            UserCreationEnum uc = userService.CreateNewUser(newUser);

            Assert.Equal(UserCreationEnum.created, uc);
        }

        [Fact]
        public void TryCreateNewUserWithExistingName()
        {
            NewUserDto newUser = new NewUserDto();
            newUser.email = "email";
            newUser.password = "password";
            newUser.userName = "FirstUser";

            UserService userService = new UserService(new UserRepository());

            UserCreationEnum uc = userService.CreateNewUser(newUser);

            Assert.Equal(UserCreationEnum.usernameTaken, uc);
        }
        [Fact]
        public void TryCreateNewUserWithNoInput()
        {
            NewUserDto newUser = new NewUserDto();

            UserService userService = new UserService(new UserRepository());

            UserCreationEnum uc = userService.CreateNewUser(newUser);

            Assert.Equal(UserCreationEnum.failed, uc);
        }
        [Fact]
        public void CheckLoginStatus()
        {
            UserService userService = new UserService(new UserRepository());

            Result<LoginDto> result= userService.TryLogin("FirstUser", "helloWorldPassWord");

            Assert.True(userService.ChecKLoginStatus(result.Data.User).Data);
        }
        [Fact]
        public void CheckLoginStatusOld()
        {
            UserRepository userRepository = new UserRepository();

            UserService userService = new UserService(userRepository);

            Result<LoginDto> LoginDto = userService.TryLogin("FirstUser", "helloWorldPassWord");

            userRepository.testMoveTokenValidTimeToPast(LoginDto.Data.User.userId);

            Assert.False(userService.ChecKLoginStatus(LoginDto.Data.User).Data);
        }

    }
}