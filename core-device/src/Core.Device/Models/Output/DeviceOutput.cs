using ProtoBuf;

namespace Core.Device.Models.Output
{
    [ProtoContract()]
    public class DeviceOutput
    {
        [ProtoMember(1)]
        public string DeviceId { get; set; }
        [ProtoMember(2)]
        public int? CustomerId { get; set; }
    }
}
