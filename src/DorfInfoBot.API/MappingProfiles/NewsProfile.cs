using AutoMapper;

namespace DorfInfoBot.API.MappingProfiles
{
    public class NewsProfile : Profile
    {
        public NewsProfile()
        {
            CreateMap<Entities.News, Models.NewsDto>().ReverseMap();
            CreateMap<Entities.News, Models.NewsWithoutAttachmentDto>().ReverseMap();
            CreateMap<Entities.News, Models.NewsCreationDto>().ReverseMap();
            CreateMap<Entities.News, Models.NewsUpdateDto>().ReverseMap();
        }
    }
}