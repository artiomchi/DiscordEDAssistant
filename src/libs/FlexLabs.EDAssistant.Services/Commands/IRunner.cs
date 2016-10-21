using System;
using System.Threading.Tasks;

namespace FlexLabs.EDAssistant.Services.Commands
{
    internal interface IRunner : IDisposable
    {
        string Prefix { get; }
        string Template { get; }
        string Title { get; }
        Task<CommandResponse> RunAsync(string[] arguments);
    }
}
