using System;
using System.Linq;
using System.Text;

namespace FlexLabs.DiscordEDAssistant.Bot.Commands
{
    public static class Helpers
    {
        public static string FormatAsTable(string[][] data)
        {
            var lengths = new int[data[0].Length];

            foreach (var col in lengths)
                lengths[col] = data.Max(r => r[col].Length);

            var sb = new StringBuilder();
            foreach (var row in data)
            {
                for (int i = 0; i <= row.Length; i++)
                {
                    if (i > 0) sb.Append(" | ");
                    var value = row[i];
                    sb.Append(value);
                    if (value.Length < lengths[i] && i < row.Length)
                        sb.Append(new String(' ', lengths[i] - value.Length));
                }
            }

            return sb.ToString();
        }
    }
}
