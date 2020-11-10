using System;
using System.Text;

namespace Common.Core.Cache.Client.Utils
{
    public class ConvertTools
    {
        public static string BytesToString(byte[] value)
        {
            return Encoding.UTF8.GetString(value);
        }

        public static byte[] StringToBytes(string value)
        {
            return Encoding.UTF8.GetBytes(value);
        }

        public static Int16 BytesToInt16(byte[] value)
        {
            return BitConverter.ToInt16(value);
        }

        public static byte[] Int16ToBytes(Int16 value)
        {
            return BitConverter.GetBytes(value);
        }
    }
}
