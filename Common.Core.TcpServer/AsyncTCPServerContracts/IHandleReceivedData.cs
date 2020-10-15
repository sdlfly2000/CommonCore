namespace Common.Core.TcpServer.AsyncTCPServerContracts
{
    public interface IHandleReceivedData
    {
        void Process(byte[] data);
    }
}
