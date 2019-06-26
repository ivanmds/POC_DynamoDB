using Core.Device.Models;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNetCore.Mvc;

namespace Core.Device.Configurations
{
    public class DeviceModelConfiguration : IModelConfiguration
    {
        private EntityTypeConfiguration<DeviceData> ConfigureCurrent(ODataModelBuilder builder) => builder.EntitySet<DeviceData>("Device").EntityType;
        public void Apply(ODataModelBuilder builder, ApiVersion apiVersion)
        {
            switch (apiVersion.MajorVersion)
            {
                default:
                    ConfigureCurrent(builder);
                    break;
            }
        }
    }
}
