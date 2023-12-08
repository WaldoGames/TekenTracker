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
        public void TryLogin_should_be_true()
        {
            UserService userService = new UserService(new UserRepository());
            
            Assert.True(userService.TryLogin("FirstUser", "helloWorldPassWord").Data.IsLoggedIn);

        }

        [Fact] 
        public void TryLoginWithFakeUser_should_be_false()
        {

            UserService userService = new UserService(new UserRepository());
            
            Assert.False(userService.TryLogin("TotalyRealUser", "helloWorldPassWord").Data.IsLoggedIn);
        }

        [Fact]
        public void TryLoginWithWrongPassword_should_be_false()
        {
            UserService userService = new UserService(new UserRepository());

            Assert.False(userService.TryLogin("FirstUser", "p2ssw0r3").Data.IsLoggedIn);

        }

        [Fact]

        public void TryCreateNewUser_should_be_true()
        {
            NewUserDto newUser = new NewUserDto();
            newUser.Email = "email";
            newUser.Password = "password"; 
            newUser.UserName = "NewUser";

            UserService userService = new UserService(new UserRepository());

            UserCreationEnum uc = userService.CreateNewUser(newUser);

            Assert.Equal(UserCreationEnum.created, uc);
        }

        [Fact]
        public void TryCreateNewUserWithExistingName_should_be_usernametaken()
        {
            NewUserDto newUser = new NewUserDto();
            newUser.Email = "email";
            newUser.Password = "password";
            newUser.UserName = "FirstUser";

            UserService userService = new UserService(new UserRepository());

            UserCreationEnum uc = userService.CreateNewUser(newUser);

            Assert.Equal(UserCreationEnum.usernameTaken, uc);
        }
        [Fact]
        public void TryCreateNewUserWithNoInput_should_be_failed()
        {
            NewUserDto newUser = new NewUserDto();

            UserService userService = new UserService(new UserRepository());

            UserCreationEnum uc = userService.CreateNewUser(newUser);

            Assert.Equal(UserCreationEnum.failed, uc);
        }
        [Fact]
        public void CheckLoginStatus_should_be_true()
        {
            UserService userService = new UserService(new UserRepository());

            Result<LoginDto> result= userService.TryLogin("FirstUser", "helloWorldPassWord");

            Assert.True(userService.ChecKLoginStatus(result.Data.User).Data);
        }
        [Fact]
        public void CheckLoginStatusOld_should_be_false()
        {
            UserRepository userRepository = new UserRepository();

            UserService userService = new UserService(userRepository);

            Result<LoginDto> LoginDto = userService.TryLogin("FirstUser", "helloWorldPassWord");

            userRepository.testMoveTokenValidTimeToPast(LoginDto.Data.User.UserId);

            Assert.False(userService.ChecKLoginStatus(LoginDto.Data.User).Data);
        }

    }
}