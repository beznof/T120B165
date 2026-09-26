using AutoMapper;
using Languages.Domain.Entities;
using Languages.WebApi.Models.DictionaryEntry.Responses.Create;
using Languages.WebApi.Models.DictionaryEntry.Responses.Read;

namespace Languages.WebApi.Mappers;

public class DictionaryEntryResponsesMapper : Profile
{
    public DictionaryEntryResponsesMapper()
    {
        CreateMap<DictionaryEntry, CreateDictionaryEntryResponse>()
            .ForMember(e => e.Id, o => o.MapFrom(e => e.ID))
            .ForMember(e => e.Text, o => o.MapFrom(e => e.Text))
            .ForMember(e => e.Translation, o => o.MapFrom(e => e.Translation))
            .ForMember(e => e.Type, o => o.MapFrom(e => e.Type))
            .ForMember(e => e.PhoneticTranscription, o => o.MapFrom(e => e.PhoneticTranscription))
            .ForMember(e => e.Dictionary, o => o.MapFrom(e => e.Dictionary))
            .ForCtorParam(nameof(CreateDictionaryEntryResponse.Synonyms), o => o.MapFrom(e =>
                e.Synonyms.OrderBy(s => s.ID).Select(s => s.Text).ToList()));

        CreateMap<DictionaryEntry, ReadDictionaryEntryResponse>()
            .ForMember(e => e.Id, o => o.MapFrom(e => e.ID))
            .ForMember(e => e.Text, o => o.MapFrom(e => e.Text))
            .ForMember(e => e.Translation, o => o.MapFrom(e => e.Translation))
            .ForMember(e => e.Type, o => o.MapFrom(e => e.Type))
            .ForMember(e => e.PhoneticTranscription, o => o.MapFrom(e => e.PhoneticTranscription))
            .ForMember(e => e.Dictionary, o => o.MapFrom(e => e.Dictionary))
            .ForCtorParam(nameof(ReadDictionaryEntryResponse.Synonyms), o => o.MapFrom(e =>
                e.Synonyms.OrderBy(s => s.ID).Select(s => s.Text).ToList()));

    }
}
