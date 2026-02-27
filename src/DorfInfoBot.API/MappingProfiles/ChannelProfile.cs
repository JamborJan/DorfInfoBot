using AutoMapper;

namespace DorfInfoBot.API.MappingProfiles
{
    public class ChannelProfile : Profile
    {
        public ChannelProfile()
        {
            CreateMap<Entities.Channel, Models.ChannelDto>().ReverseMap();
            CreateMap<Entities.Channel, Models.ChannelCreationDto>().ReverseMap();
            CreateMap<Entities.Channel, Models.ChannelUpdateDto>().ReverseMap();
        }
    }
}