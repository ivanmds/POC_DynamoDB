using ProtoBuf;

namespace Core.Device.Models.Input
{
    [ProtoContract()]
    public class DeviceInput
    {
        [ProtoMember(1)]
        public string DeviceId { get; set; }
        [ProtoMember(2)]
        public int? CustomerId { get; set; }
        [ProtoMember(3)]
        public string DeviceLocale { get; set; }
        [ProtoMember(4)]
        public string DeviceSystem { get; set; }
        [ProtoMember(5)]
        public string SystemVersion { get; set; }
        [ProtoMember(6)]
        public double BatteryLevel { get; set; }
        [ProtoMember(7)]
        public string DeviceBrand { get; set; }
        [ProtoMember(8)]
        public string BuildNumber { get; set; }
        [ProtoMember(9)]
        public string Carrier { get; set; }
        [ProtoMember(10)]
        public string DeviceCountry { get; set; }
        [ProtoMember(11)]
        public long? FirstInstallTime { get; set; }
        [ProtoMember(12)]
        public double FontScale { get; set; }
        [ProtoMember(13)]
        public long FreeDiskStorage { get; set; }
        [ProtoMember(14)]
        public string IpAddress { get; set; }
        [ProtoMember(15)]
        public string MacAddress { get; set; }
        [ProtoMember(16)]
        public string TimeZone { get; set; }
        [ProtoMember(17)]
        public long TotalDiskCapacity { get; set; }
        [ProtoMember(18)]
        public long TotalMemory { get; set; }
        [ProtoMember(19)]
        public bool? IsAirPlaneMode { get; set; }
        [ProtoMember(20)]
        public bool IsBatteryCharging { get; set; }
        [ProtoMember(21)]
        public bool IsPinOrFingerPrint { get; set; }
        [ProtoMember(22)]
        public string DeviceType { get; set; }
        [ProtoMember(23)]
        public string PushToken { get; set; }

    }
}
