using AutoMapper;
using Languages.Domain.Entities;
using Languages.WebApi.Models.LanguageFamily.Responses.Create;
using Languages.WebApi.Models.LanguageFamily.Responses.Read;

namespace Languages.WebApi.Mappers;

public class LanguageFamilyResponsesMapper : Profile
{
    public LanguageFamilyResponsesMapper()
    {
        CreateMap<LanguageFamily, CreateLanguageFamilyResponse>()
            .ForMember(f => f.Id, o => o.MapFrom(f => f.Id))
            .ForMember(f => f.Name, o => o.MapFrom(f => f.Name));

        CreateMap<LanguageFamily, ReadLanguageFamilyResponse>()
            .ForMember(f => f.Id, o => o.MapFrom(f => f.Id))
            .ForMember(f => f.Name, o => o.MapFrom(f => f.Name));
    }
}
