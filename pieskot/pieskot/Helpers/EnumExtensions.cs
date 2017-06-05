using System;
using System.ComponentModel;

namespace NaSpacerDo.Helpers
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            var attribute = value.GetAttributeOfType<DescriptionAttribute>();

            return attribute?.Description ?? string.Empty;
        }

        private static T GetAttributeOfType<T>(this Enum value) where T : System.Attribute
        {
            var type = value.GetType();
            var memInfo = type.GetMember(value.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return (attributes.Length > 0) ? (T)attributes[0] : null;
        }
    }
}