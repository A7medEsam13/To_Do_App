using AutoMapper;
using To_Do_List.ModelView;

namespace To_Do_List.Mapping
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<Models.Task, TaskViewModel>()
                .ForMember(dest => dest.Start, opt => opt.MapFrom(src => src.Start))
                .ForMember(dest => dest.End, opt => opt.MapFrom(src => src.End))
                .ReverseMap();
        }
    }
}
