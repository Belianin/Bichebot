using Bichebot.Utilities;

namespace Bichebot.Banking
{
    public interface IBankCore<in TKey>
    {
        int GetBalance(TKey id);

        int SetBalance(TKey id, int value);

        int Add(TKey id, int value);

        bool TryTransact(TKey from, TKey to, int value);
    }
}