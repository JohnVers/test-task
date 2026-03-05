using System.Globalization;
using System.Text.RegularExpressions;
using AutoMapper;
using task.Data.Entities;
using task.Dto;

namespace task.Map;

class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<TerminalDto, Office>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => int.Parse(src.Id ?? "0")))
            .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.AddressCode == null ? null : src.AddressCode.StreetCode))
            .ForMember(dest => dest.CityCode, opt => opt.Ignore())
            .ForMember(dest => dest.Uuid, opt => opt.MapFrom(x => Guid.NewGuid().ToString()))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.IsPVZ ? OfficeType.PVZ : src.Storage ? OfficeType.WAREHOUSE : (OfficeType?)null))
            .ForMember(dest => dest.CountryCode, opt => opt.MapFrom(src => "RU"))
            .ForMember(dest => dest.Coordinates, opt => opt.MapFrom(src => new Coordinates
            {
                Latitude = double.Parse(src.Latitude ?? "0", CultureInfo.InvariantCulture),
                Longitude = double.Parse(src.Longitude ?? "0", CultureInfo.InvariantCulture)
            }))
            .ForMember(dest => dest.AddressRegion, opt => opt.MapFrom(src => ParseAddressRegion(src.FullAddress)))
            .ForMember(dest => dest.AddressCity, opt => opt.Ignore())
            .ForMember(dest => dest.AddressStreet, opt => opt.MapFrom(src => ParseAddressStreet(src.Address)))
            .ForMember(dest => dest.AddressHouseNumber, opt => opt.MapFrom(src => ParseHouseNumber(src.Address)))
            .ForMember(dest => dest.AddressApartment, opt => opt.MapFrom(src => ParseApartment(src.FullAddress)))
            .ForMember(dest => dest.WorkTime,
                    opt => opt.MapFrom(src =>
                        src.Worktables != null &&
                        src.Worktables.Worktable != null &&
                        src.Worktables.Worktable.Any()
                            ? src.Worktables.Worktable.First().Timetable
                            : (src.CalcSchedule != null ? src.CalcSchedule.Derival : "")))
            .ForMember(dest => dest.Phones, opt => opt.MapFrom(src => src.Phones));

        CreateMap<PhoneDto, Phone>()
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Number ?? ""))
            .ForMember(dest => dest.Additional, opt => opt.MapFrom(src => $"{src.Type} {src.Comment}".Trim()));
    }

    private static string? ParseAddressRegion(string? fullAddress) 
        => fullAddress?.Split(", ").ElementAtOrDefault(1);
    
    private static string? ParseAddressStreet(string? address) 
        => address?.Split(", ").FirstOrDefault();
    
    private static string? ParseHouseNumber(string? address) 
        => address?.Split(", ").LastOrDefault();
    
    private static int? ParseApartment(string? fullAddress)
    {
        if (string.IsNullOrEmpty(fullAddress)) 
            return null;
        
        var match = Regex.Match(fullAddress, @"(офис|пом\.)\s*(\d+)");
        
        return match.Success && int.TryParse(match.Groups[2].Value, out int apt) ? apt : null;
    }
}
