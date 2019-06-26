namespace Core.Device.Models
{
    public static class ErrorCode
    {
        public static readonly ErrorType IsNullOrWhiteSpaceDeviceIdStatus = new ErrorType("01", "Null or Empty DeviceId");
        public static readonly ErrorType IsNullOrWhiteSpacePushTokenStatus = new ErrorType("02", "Null or Empty PushToken");
        public static readonly ErrorType IsNullOrWhiteSpaceMacAddressStatus = new ErrorType("03", "Null or Empty MacAddress");
    }

    public class ErrorType
    {
        public ErrorType(string code, string message)
        {
            Code = code;
            Message = message;
        }

        public string Code { get; set; }
        public string Message { get; set; }
    }
}
