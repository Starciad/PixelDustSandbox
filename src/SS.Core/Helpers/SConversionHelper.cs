using System;

namespace StardustSandbox.Core.Helpers
{
    public static class SConversionHelper
    {
        public static T ConvertTo<T>(object value) where T : struct
        {
            return value is T typedValue ? typedValue : (T)Convert.ChangeType(value, typeof(T));
        }
    }
}
