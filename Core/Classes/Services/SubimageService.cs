using Core.Classes.DTO;
using Core.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Classes.Services
{
    public class SubimageService
    {
        ISubImageRepository SubimageRepository;

        public SubimageService(ISubImageRepository subImageRepository)
        {
            SubimageRepository = subImageRepository;
        }
        public SimpleResult AddNewSubimage(NewSubimageDto subimageDto)
        {
            return SubimageRepository.AddNewSubimage(subimageDto.postId, subimageDto.imageUrl);
        }
        public SimpleResult AddManySubimagesNewPost(List<NewSubimageDto> newSubimages, int postId)
        {
            foreach (NewSubimageDto newSubimage in newSubimages)
            {
                newSubimage.postId = postId;
                if (AddNewSubimage(newSubimage).IsFailed)
                {
                    return new SimpleResult { ErrorMessage = "SubimageServices->TryAddManySubimagesNewPost: Error passed from AddNewSubimage" };
                }
            }
            return new SimpleResult { };
        }
    }
}
