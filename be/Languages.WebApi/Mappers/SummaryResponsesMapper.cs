using AutoMapper;
using Languages.Domain.Entities;
using Languages.WebApi.Models.Common;

namespace Languages.WebApi.Mappers;

public class SummaryResponsesMapper : Profile
{
    public SummaryResponsesMapper()
    {
        CreateMap<Language, LanguageSummaryResponse>()
            .ForMember(l => l.Id, o => o.MapFrom(l => l.Id))
            .ForMember(l => l.Name, o => o.MapFrom(l => l.Name));

        CreateMap<LanguageLevel, LanguageLevelSummaryResponse>()
            .ForMember(l => l.Id, o => o.MapFrom(l => l.Id))
            .ForMember(l => l.Name, o => o.MapFrom(l => l.Name));

        CreateMap<LanguageFamily, LanguageFamilySummaryResponse>()
            .ForMember(f => f.Id, o => o.MapFrom(f => f.Id))
            .ForMember(f => f.Name, o => o.MapFrom(f => f.Name));

        CreateMap<LanguageDictionary, LanguageDictionarySummaryResponse>()
            .ForMember(d => d.Id, o => o.MapFrom(d => d.Id))
            .ForMember(d => d.Name, o => o.MapFrom(d => d.Name));
    }
}
