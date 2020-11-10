using System;
using System.Collections.Generic;

namespace Common.Core.Cache.PipeCache
{
    using System.IO.Pipes;

    using Common.Core.Cache.PipeCache.Processes;

    public class PipeCacheClient<T> : IPipeCacheClient<T>, IDisposable where T: class
    {
        private string PipeServer = "";

        private string PipeName = "";

        private NamedPipeClientStream _pipeClient;

        private readonly IPipeBufferComposeProcess _composeProcess;

        public PipeCacheClient(IPipeBufferComposeProcess composeProcess)
        {
            _composeProcess = composeProcess;
        }

        public void SetupPipeClient(string pipeServer, string pipeName)
        {
            PipeServer = pipeServer;
            PipeName = pipeName;
            _pipeClient = new NamedPipeClientStream(PipeServer, PipeName, PipeDirection.InOut);
            _pipeClient.Connect();
        }

        public T Get(string key)
        {
            var buffer = _composeProcess.ComposeGet(key);
            if (_pipeClient.IsConnected)
            {
                _pipeClient.Write(buffer.AsSpan());
                _pipeClient.Read();
            }

        }

        public T Set(string key, T value)
        {
            throw new NotImplementedException();
        }

        public void Remove(string key)
        {
            throw new NotImplementedException();
        }

        public IList<string> GetAllKeys()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _pipeClient?.Dispose();
        }
    }
}
