using Bichebot.Utilities;

namespace Bichebot.Banking
{
    public interface IBankCore<in TKey>
    {
        int GetBalance(TKey id);

        Result<int> SetBalance(TKey id, int value);

        Result<int> Add(TKey id, int value);
        
        Result TryTransact(TKey from, TKey to, int value);
    }
    
    public interface IBankCore : IBankCore<ulong>
    {
        int GetBalance(ulong id);

        Result<int> SetBalance(ulong id, int value);

        Result<int> Add(ulong id, int value);
        
        Result TryTransact(ulong from, ulong to, int value);
    }
}