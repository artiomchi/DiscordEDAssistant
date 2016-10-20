using System.Collections.Generic;

namespace FlexLabs.EDAssistant.Services.Commands
{
    public class CommandResponse
    {
        public static CommandResponse Nop => new CommandResponse();
        public static CommandResponse Text(string text) => new CommandResponse().Add(text);
        public static CommandResponse Error(string text) => new CommandResponse().Add(text);

        private List<ICommandContent> _contents = new List<ICommandContent>();
        public IReadOnlyList<ICommandContent> Contents => _contents;

        internal CommandResponse Add(ICommandContent content)
        {
            _contents.Add(content);
            return this;
        }
        internal CommandResponse Add(string text) => Add(new CommandTextContent(text));
        internal CommandResponse AddTable(string[][] cells, int[] rightAlignedCells = null) => Add(new CommandTableContent(cells, rightAlignedCells));
    }
}
