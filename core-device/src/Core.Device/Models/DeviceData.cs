using ServiceStack.DataAnnotations;

namespace Core.Device.Models
{
    [Alias("Device")]
    public class DeviceData
    {
        public DeviceData()
        {
            IsActive = true;
        }

        public string Id { get; set; }
        public int? CustomerId { get; set; }
       
        public string DeviceLocale { get; set; }
       
        public string DeviceSystem { get; set; }
       
        public string SystemVersion { get; set; }
       
        public double BatteryLevel { get; set; }
       
        public string DeviceBrand { get; set; }
       
        public string BuildNumber { get; set; }
       
        public string Carrier { get; set; }
       
        public string DeviceCountry { get; set; }
       
        public long? FirstInstallTime { get; set; }
       
        public double FontScale { get; set; }
       
        public long FreeDiskStorage { get; set; }
       
        public string IpAddress { get; set; }
       
        public string MacAddress { get; set; }
       
        public string TimeZone { get; set; }
       
        public long TotalDiskCapacity { get; set; }
       
        public long TotalMemory { get; set; }
       
        public bool? IsAirPlaneMode { get; set; }
       
        public bool IsBatteryCharging { get; set; }
       
        public bool IsPinOrFingerPrint { get; set; }
       
        public string DeviceType { get; set; }
       
        public string PushToken { get; set; }
       
        public long TimeToLive { get; set; }
       
        public bool IsActive { get; set; }

    }
}
