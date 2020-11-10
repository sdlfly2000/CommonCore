using System;
using Common.Core.DependencyInjection;

namespace Common.Core.Cache.PipeCache.Processes
{
    [ServiceLocate(typeof(IPipeBufferComposeProcess))]
    public class PipeBufferComposeProcess : IPipeBufferComposeProcess
    {
        public byte[] ComposeGet(string key)
        {
            throw new NotImplementedException();
        }

        public byte[] ComposeSet(string key, object value)
        {
            throw new NotImplementedException();
        }

        public byte[] ComposeRemove(string key)
        {
            throw new NotImplementedException();
        }

        public byte[] ComposeGetAllKeys()
        {
            throw new NotImplementedException();
        }
    }
}
