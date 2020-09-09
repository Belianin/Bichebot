using System.Threading.Tasks;
using Discord.WebSocket;

namespace Bichebot.Core.Pipeline
{
    public interface IMessagePipeline
    {
        Task HandleAsync(SocketMessage message);
    }
}