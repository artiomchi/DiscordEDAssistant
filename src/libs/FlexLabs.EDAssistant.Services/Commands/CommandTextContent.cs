namespace FlexLabs.EDAssistant.Services.Commands
{
    public class CommandTextContent : ICommandContent
    {
        public CommandTextContent(string text)
        {
            Text = text;
        }

        public string Text { get; }

        public string Format(string channel) => Text;
    }
}
