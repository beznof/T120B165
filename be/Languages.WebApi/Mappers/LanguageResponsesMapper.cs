using AutoMapper;
using Languages.Domain.Entities;
using Languages.WebApi.Models.Language.Responses.Create;
using Languages.WebApi.Models.Language.Responses.Read;

namespace Languages.WebApi.Mappers;

public class LanguageResponsesMapper : Profile
{
    public LanguageResponsesMapper()
    {
        CreateMap<Language, CreateLanguageResponse>()
            .ForMember(l => l.Id, o => o.MapFrom(l => l.Id))
            .ForMember(l => l.Name, o => o.MapFrom(l => l.Name))
            .ForMember(l => l.BackgroundImageUrl, o => o.MapFrom(l => l.BackgroundImageUrl))
            .ForMember(l => l.LanguageFamily, o => o.MapFrom(l => l.LanguageFamily));

        CreateMap<Language, ReadLanguageResponse>()
            .ForMember(l => l.Id, o => o.MapFrom(l => l.Id))
            .ForMember(l => l.Name, o => o.MapFrom(l => l.Name))
            .ForMember(l => l.BackgroundImageUrl, o => o.MapFrom(l => l.BackgroundImageUrl))
            .ForMember(l => l.LanguageFamily, o => o.MapFrom(l => l.LanguageFamily));
    }
}
