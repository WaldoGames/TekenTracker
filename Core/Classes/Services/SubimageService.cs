using Core.Classes.DTO;
using Core.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Classes.Services
{
    internal class SubimageService
    {
        ISubImageRepository SubimageRepository;

        public SubimageService(ISubImageRepository subImageRepository)
        {
            SubimageRepository = subImageRepository;
        }
        public bool TryAddNewSubimage(NewSubimageDto subimageDto)
        {
            return SubimageRepository.TryAddNewSubimage(subimageDto.postId, subimageDto.imageUrl);
        }
        public bool TryAddManySubimagesNewPost(List<NewSubimageDto> newSubimages, int postId)
        {
            foreach (NewSubimageDto newSubimage in newSubimages)
            {
                newSubimage.postId = postId;
                if (!TryAddNewSubimage(newSubimage))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
