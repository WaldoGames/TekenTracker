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
        public Result<SubimagesDto> GetSubimagesFromPost(int postId);
        public SimpleResult AddNewSubimage(int postId, string newUrl);
        public SimpleResult UpdateSubimage(int postId, string updatesUrl);
        public SimpleResult RemoveNewSubimage(int subimageId);
        public Result<bool> DoesSubimageExist(int subimageId);
    }
}
