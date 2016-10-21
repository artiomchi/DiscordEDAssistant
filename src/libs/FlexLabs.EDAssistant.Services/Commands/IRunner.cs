using System;
using System.Threading.Tasks;

namespace FlexLabs.EDAssistant.Services.Commands
{
    public interface IRunner : IDisposable
    {
        string Prefix { get; }
        string Template { get; }
        string Title { get; }
        Task<CommandResponse> RunAsync(string[] arguments, object channelData);
    }
}
