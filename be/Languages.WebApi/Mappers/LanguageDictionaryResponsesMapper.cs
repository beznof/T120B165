using AutoMapper;
using Languages.Domain.Entities;
using Languages.WebApi.Models.LanguageDictionary.Responses.Create;
using Languages.WebApi.Models.LanguageDictionary.Responses.Read;

namespace Languages.WebApi.Mappers;

public class LanguageDictionaryResponsesMapper : Profile
{
    public LanguageDictionaryResponsesMapper()
    {
        CreateMap<LanguageDictionary, CreateLanguageDictionaryResponse>()
            .ForMember(d => d.Id, o => o.MapFrom(d => d.Id))
            .ForMember(d => d.Name, o => o.MapFrom(d => d.Name))
            .ForMember(d => d.BackgroundImageUrl, o => o.MapFrom(d => d.BackgroundImageUrl))
            .ForMember(d => d.Language, o => o.MapFrom(d => d.Language))
            .ForMember(d => d.LanguageLevel, o => o.MapFrom(d => d.LanguageLevel));

        CreateMap<LanguageDictionary, ReadLanguageDictionaryResponse>()
            .ForMember(d => d.Id, o => o.MapFrom(d => d.Id))
            .ForMember(d => d.Name, o => o.MapFrom(d => d.Name))
            .ForMember(d => d.BackgroundImageUrl, o => o.MapFrom(d => d.BackgroundImageUrl))
            .ForMember(d => d.Language, o => o.MapFrom(d => d.Language))
            .ForMember(d => d.LanguageLevel, o => o.MapFrom(d => d.LanguageLevel));
    }
}
