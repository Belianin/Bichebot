using System.Threading.Tasks;
using Discord.WebSocket;

namespace Bichebot.Core.Pipeline
{
    public interface IMessageHandler
    {
        Task<bool> HandleAsync(SocketMessage message);
    }
}