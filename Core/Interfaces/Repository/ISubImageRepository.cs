using Core.Classes;
using Core.Classes.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repository
{
    public interface ISubImageRepository
    {
        public Result<SubimagesDto> GetSubimagesFromPost(int PostId);
        public SimpleResult AddNewSubimage(int PostId, string NewUrl);
        public SimpleResult UpdateSubimage(int PostId, string UpdatesUrl);
        public SimpleResult RemoveNewSubimage(int SubimageId);
        public Result<bool> DoesSubimageExist(int SubimageId);
    }
}
