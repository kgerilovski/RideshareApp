using RideshareApp.Common.Resources;
using System.ComponentModel.DataAnnotations;

namespace RideshareApp.Entities.Helpers
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class CustomStringLengthAttribute : MaxLengthAttribute
    {
        public CustomStringLengthAttribute(MaxLengthEnum type) : base((int)type)
        {
            this.Type = type;
            this.ErrorMessage = nameof(CommonResources.MsgMinLength);
            this.ErrorMessageResourceType = typeof(CommonResources);
        }

        public MaxLengthEnum Type { get; set; }
    }
}
