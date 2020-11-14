using System;
using System.Collections.Generic;
using System.IO.Pipes;
using Common.Core.Cache.Client.Utils;
using Common.Core.Cache.PipeCache.Processes;
using Common.Core.DependencyInjection;
using Newtonsoft.Json;

namespace Common.Core.Cache.PipeCache
{
    [ServiceLocate(typeof(IPipeCacheClient))]
    public class PipeCacheClient : IPipeCacheClient, IDisposable
    {
        private NamedPipeClientStream _pipeClient;
        private static byte[] _bufferReceived = new byte[Int16.MaxValue];

        private readonly IPipeBufferComposeProcess _composeProcess;

        public PipeCacheClient(IPipeBufferComposeProcess composeProcess)
        {
            _composeProcess = composeProcess;
        }

        public void SetupPipeClient(NamedPipeClientStream pipe)
        {
            _pipeClient = pipe;
        }

        public T Get<T>(string key) where T: class
        {
            var bufferSent = _composeProcess.ComposeGet(key);
            var numRecv = SendAndReceive(bufferSent, _bufferReceived);

            return JsonConvert.DeserializeObject<T>(ConvertTools.BytesToString(_bufferReceived.AsSpan().Slice(0,numRecv).ToArray()));
        }

        public void Set<T>(string key, T value) where T : class
        {
            var bufferSent = _composeProcess.ComposeSet(key, value);
            _ = SendAndReceive(bufferSent, _bufferReceived);
        }

        public void Remove(string key)
        {
            var bufferSent = _composeProcess.ComposeRemove(key);
            _ = SendAndReceive(bufferSent, _bufferReceived);
        }

        public IList<string> GetAllKeys()
        {
            var bufferSent = _composeProcess.ComposeGetAllKeys();
            var numRecv = SendAndReceive(bufferSent, _bufferReceived);

            return JsonConvert.DeserializeObject<IList<string>>(ConvertTools.BytesToString(_bufferReceived.AsSpan().Slice(0, numRecv).ToArray()));
        }

        public void Dispose()
        {
            _pipeClient?.Dispose();
        }        

        #region Private Methods

        private int SendAndReceive(byte[] bufferSent, byte[] bufferRecv)
        {
            if (!_pipeClient.IsConnected)
            {
                _pipeClient.Connect();
            }

            _pipeClient.Write(bufferSent.AsSpan());
            var numRecv = _pipeClient.Read(bufferRecv.AsSpan());

            return numRecv;
        }

        #endregion

    }
}
