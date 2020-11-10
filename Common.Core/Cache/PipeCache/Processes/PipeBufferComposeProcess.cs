using System.Collections.Generic;
using Common.Core.Cache.Client.Contracts;
using Common.Core.Cache.Client.Utils;
using Common.Core.DependencyInjection;
using Newtonsoft.Json;

namespace Common.Core.Cache.PipeCache.Processes
{
    [ServiceLocate(typeof(IPipeBufferComposeProcess))]
    public class PipeBufferComposeProcess : IPipeBufferComposeProcess
    {
        public byte[] ComposeGet(string key)
        {
            var buffer = new List<byte>();
            var keyLen = (short)key.Length;
            var totalLen = (short)(2 + 2 + 2 + keyLen);
            var commandCode = (short)CommandType.Get;

            buffer.AddRange(ConvertTools.Int16ToBytes(totalLen));
            buffer.AddRange(ConvertTools.Int16ToBytes(commandCode));
            buffer.AddRange(ConvertTools.Int16ToBytes(keyLen));
            buffer.AddRange(ConvertTools.StringToBytes(key));

            return buffer.ToArray();
        }

        public byte[] ComposeSet(string key, object value)
        {
            var buffer = new List<byte>();
            var keyLen = (short)key.Length;            
            var commandCode = (short)CommandType.Set;
            var valueSent = JsonConvert.SerializeObject(value);
            var valueSentLen = (short)valueSent.Length;
            var totalLen = (short)(2 + 2 + 2 + keyLen + 2 + valueSentLen);

            buffer.AddRange(ConvertTools.Int16ToBytes(totalLen));
            buffer.AddRange(ConvertTools.Int16ToBytes(commandCode));
            buffer.AddRange(ConvertTools.Int16ToBytes(keyLen));
            buffer.AddRange(ConvertTools.StringToBytes(key));
            buffer.AddRange(ConvertTools.Int16ToBytes(valueSentLen));
            buffer.AddRange(ConvertTools.StringToBytes(valueSent));

            return buffer.ToArray();
        }

        public byte[] ComposeRemove(string key)
        {
            var buffer = new List<byte>();
            var keyLen = (short)key.Length;
            var totalLen = (short)(2 + 2 + 2 + keyLen);
            var commandCode = (short)CommandType.Remove;

            buffer.AddRange(ConvertTools.Int16ToBytes(totalLen));
            buffer.AddRange(ConvertTools.Int16ToBytes(commandCode));
            buffer.AddRange(ConvertTools.Int16ToBytes(keyLen));
            buffer.AddRange(ConvertTools.StringToBytes(key));

            return buffer.ToArray();
        }

        public byte[] ComposeGetAllKeys()
        {
            var buffer = new List<byte>();
            var totalLen = (short)(2 + 2 );
            var commandCode = (short)CommandType.GetAllKeys;

            buffer.AddRange(ConvertTools.Int16ToBytes(totalLen));
            buffer.AddRange(ConvertTools.Int16ToBytes(commandCode));

            return buffer.ToArray();
        }
    }
}
