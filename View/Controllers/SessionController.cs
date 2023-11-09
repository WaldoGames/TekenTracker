using Core.Classes.DTO;
using Core.Interfaces.Repository;
using Dal.Classes.RepositoryImplementations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Text;

namespace View.Controllers
{
    public class SessionController : Controller
    {
        IMemoryCache _cache;

        string userCacheKey = "UserCache";



        public SessionController(IMemoryCache cache)
        {
            _cache = cache;

        }
        public void AddUserToSession(UserDto user)
        {
            _cache.Set(userCacheKey, user);

            MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions();
            cacheOptions.AbsoluteExpiration = DateTimeOffset.UtcNow.AddHours(2);
        }
        public bool GetUserFromSession(out UserDto? user)
        {
            if(_cache.TryGetValue(userCacheKey, out user))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
