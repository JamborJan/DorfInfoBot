using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace DorfInfoBot.API.MappingProfiles
{
    public class AttachmentProfile : Profile
    {
        public AttachmentProfile()
        {
            CreateMap<Models.AttachmentDto, Entities.Attachment>().ReverseMap();
            CreateMap<Models.AttachmentCreationDto, Entities.Attachment>().ReverseMap();
            CreateMap<Models.AttachmentUpdateDto, Entities.Attachment>().ReverseMap();
        }
    }
}