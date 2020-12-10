using System;

namespace MessageDbLib.Utility
{
    public static class DbValueUtil
    {
        public static object GetValidValue(object value)
        {
            if (value == null)
            {
                return DBNull.Value;
            }
            if (value is string)
            {
                string stringValue = value as string;
                return string.IsNullOrEmpty(stringValue) ? DBNull.Value : (object)stringValue;
            }
            return value;
        }
    }
}