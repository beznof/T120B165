using AutoMapper;
using Languages.Domain.Entities;
using Languages.WebApi.Models.LanguageLevel.Responses.Create;
using Languages.WebApi.Models.LanguageLevel.Responses.Read;

namespace Languages.WebApi.Mappers;

public class LanguageLevelResponsesMapper : Profile
{
    public LanguageLevelResponsesMapper()
    {
        CreateMap<LanguageLevel, CreateLanguageLevelResponse>()
            .ForMember(l => l.Id, o => o.MapFrom(l => l.Id))
            .ForMember(l => l.Name, o => o.MapFrom(l => l.Name))
            .ForMember(l => l.LanguageID, o => o.MapFrom(l => l.LanguageId));

        CreateMap<LanguageLevel, ReadLanguageLevelResponse>()
            .ForMember(l => l.Id, o => o.MapFrom(l => l.Id))
            .ForMember(l => l.Name, o => o.MapFrom(l => l.Name))
            .ForMember(l => l.LanguageID, o => o.MapFrom(l => l.LanguageId));
    }
}
