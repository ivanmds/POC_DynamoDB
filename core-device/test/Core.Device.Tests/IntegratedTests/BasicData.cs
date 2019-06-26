using Core.Device.Configurations;
using Core.Device.Models;
using Core.Device.Models.Input;
using System;
using System.Collections.Generic;

namespace Core.Device.Tests.IntegratedTests
{
    public class BasicData
    {
        public static void PopulateTestData(DeviceContext appDb)
        {
            appDb.RegisterTables();

            foreach (var item in BasicDatas)
                appDb.PutItem(item);
        }

        private static List<DeviceData> BasicDatas => new List<DeviceData>
        {
            Device1
        };

        public static DeviceData Device1 = new DeviceData
        {
            CustomerId = 1,
            Id = "1",
            BatteryLevel = 0.55,
            BuildNumber = "1.60",
            Carrier = "CLARO",
            DeviceBrand = "APPLE",
            DeviceCountry = "BRAZIL",
            DeviceLocale= "pt-BR",
            DeviceSystem = "IOS",
            DeviceType = "CELLPHONE",
            FirstInstallTime = DateTime.Now.Ticks,
            FontScale = 10,
            FreeDiskStorage = 250000,
            IpAddress = "127.0.0.1",
            IsAirPlaneMode = true,
            IsBatteryCharging = true,
            IsPinOrFingerPrint = false,
            MacAddress = "00F2-A426-FBC30",
            PushToken = "e6rGiqUotyo:APA91bFjVT75ppph8gpNAyAcfOwwoUAojfSYIPcduiudl2cChW-0hSP1x78zQ01jd5CLIfPL64Lrdy-zmg6EQvP7NdU3cRDWOBN-_Ui749LcUSFuOXjwUicaphkX6wyyr7RG6mB7U_u6",
            SystemVersion ="1.56",
            TimeToLive = DateTime.Now.AddDays(1).Ticks,
            TimeZone = "Africa/Tunis",
            TotalDiskCapacity = 50000000,
            TotalMemory = 525000
        };

        public static DeviceInput DataInput = new DeviceInput
        {
            CustomerId = 1,
            DeviceId = "1",
            BatteryLevel = 0.55,
            BuildNumber = "1.60",
            Carrier = "CLARO",
            DeviceBrand = "APPLE",
            DeviceCountry = "BRAZIL",
            DeviceLocale = "pt-BR",
            DeviceSystem = "IOS",
            DeviceType = "CELLPHONE",
            FirstInstallTime = DateTime.Now.Ticks,
            FontScale = 10,
            FreeDiskStorage = 250000,
            IpAddress = "127.0.0.1",
            IsAirPlaneMode = true,
            IsBatteryCharging = true,
            IsPinOrFingerPrint = false,
            MacAddress = "00F2-A426-FBC30",
            PushToken = "e6rGiqUotyo:APA91bFjVT75ppph8gpNAyAcfOwwoUAojfSYIPcduiudl2cChW-0hSP1x78zQ01jd5CLIfPL64Lrdy-zmg6EQvP7NdU3cRDWOBN-_Ui749LcUSFuOXjwUicaphkX6wyyr7RG6mB7U_u6",
            SystemVersion = "1.56",
            TimeZone = "Africa/Tunis",
            TotalDiskCapacity = 50000000,
            TotalMemory = 525000
        };
    }
}
