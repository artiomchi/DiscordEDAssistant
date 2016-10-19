using System;
using System.Linq;
using System.Text;

namespace FlexLabs.EDAssistant.Bot.Commands
{
    public static class Helpers
    {
        public static string FormatAsTable(string[][] data, int[] rightAlignedCols = null)
        {
            var lengths = new int[data[0].Length];

            for (int i = 0; i < lengths.Length; i++)
                lengths[i] = data.Max(r => r[i].Length);

            var sb = new StringBuilder();
            foreach (var row in data)
            {
                for (int i = 0; i < row.Length; i++)
                {
                    if (i > 0) sb.Append(" | ");
                    var value = row[i];
                    var rightAligned = rightAlignedCols?.Contains(i) == true;

                    if (rightAligned == true && value.Length < lengths[i])
                        sb.Append(new string(' ', lengths[i] - value.Length));
                    sb.Append(value);
                    if (rightAligned == false && value.Length < lengths[i])
                        sb.Append(new string(' ', lengths[i] - value.Length));
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
