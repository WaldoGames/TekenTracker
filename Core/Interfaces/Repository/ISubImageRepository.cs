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
        public bool TryGetSubimagesFromPost(int PostId, out SubimagesDto notes);
        public bool TryAddNewSubimage(int PostId, string NewUrl);
        public bool TryUpdateSubimage(int PostId, string UpdatesUrl);
        public bool TryRemoveNewSubimage(int SubimageId);
        public bool DoesSubimageExist(int SubimageId);
    }
}
