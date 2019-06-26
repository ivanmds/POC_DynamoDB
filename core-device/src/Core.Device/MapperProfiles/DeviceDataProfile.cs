using AutoMapper;
using Core.Device.Models;
using Core.Device.Models.Input;
using Core.Device.Models.Output;

namespace Core.Device.MapperProfiles
{
    public class DeviceDataProfile : Profile
    {
        public DeviceDataProfile()
        {
            CreateMap<DeviceInput, DeviceData>()
                .ForMember(a => a.Id, b => b.MapFrom(c => c.DeviceId))
                .ForMember(a => a.CustomerId, b => b.MapFrom(c => c.CustomerId))
                .ForMember(a => a.IpAddress, b => b.MapFrom(c => c.IpAddress))
                .ForMember(a => a.IsAirPlaneMode, b => b.MapFrom(c => c.IsAirPlaneMode))
                .ForMember(a => a.IsBatteryCharging, b => b.MapFrom(c => c.IsBatteryCharging))
                .ForMember(a => a.IsPinOrFingerPrint, b => b.MapFrom(c => c.IsPinOrFingerPrint))
                .ForMember(a => a.MacAddress, b => b.MapFrom(c => c.MacAddress))
                .ForMember(a => a.SystemVersion, b => b.MapFrom(c => c.SystemVersion))
                .ForMember(a => a.TimeZone, b => b.MapFrom(c => c.TimeZone))
                .ForMember(a => a.TotalDiskCapacity, b => b.MapFrom(c => c.TotalDiskCapacity))
                .ForMember(a => a.TotalMemory, b => b.MapFrom(c => c.TotalMemory))
                .ForMember(a => a.FreeDiskStorage, b => b.MapFrom(c => c.FreeDiskStorage))
                .ForMember(a => a.BatteryLevel, b => b.MapFrom(c => c.BatteryLevel))
                .ForMember(a => a.BuildNumber, b => b.MapFrom(c => c.BuildNumber))
                .ForMember(a => a.Carrier, b => b.MapFrom(c => c.Carrier))
                .ForMember(a => a.DeviceBrand, b => b.MapFrom(c => c.DeviceBrand))
                .ForMember(a => a.DeviceCountry, b => b.MapFrom(c => c.DeviceCountry))
                .ForMember(a => a.DeviceLocale, b => b.MapFrom(c => c.DeviceLocale))
                .ForMember(a => a.DeviceSystem, b => b.MapFrom(c => c.DeviceSystem))
                .ForMember(a => a.DeviceType, b => b.MapFrom(c => c.DeviceType))
                .ForMember(a => a.FontScale, b => b.MapFrom(c => c.FontScale))
                .ForMember(a => a.PushToken, b => b.MapFrom(c => c.PushToken));


            CreateMap<DeviceData, DeviceOutput>()
                .ForMember(a => a.DeviceId, b => b.MapFrom(c => c.Id))
                .ForMember(a => a.CustomerId, b => b.MapFrom(c => c.CustomerId));
        }
    }
}
