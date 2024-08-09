using AutoMapper;

namespace DorfInfoBot.API.MappingProfiles
{
    public class BroadcastProfile : Profile
    {
        public BroadcastProfile()
        {
            CreateMap<Entities.Broadcast, Models.BroadcastDto>().ReverseMap();
            CreateMap<Entities.Broadcast, Models.BroadcastCreationDto>().ReverseMap();
            CreateMap<Entities.Broadcast, Models.BroadcastUpdateDto>().ReverseMap();
        }
    }
}