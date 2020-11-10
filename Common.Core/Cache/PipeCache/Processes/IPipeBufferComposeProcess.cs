namespace Common.Core.Cache.PipeCache.Processes
{
    public interface IPipeBufferComposeProcess
    {
        byte[] ComposeGet(string key);

        byte[] ComposeSet(string key, object value);

        byte[] ComposeRemove(string key);

        byte[] ComposeGetAllKeys();
    }
}
