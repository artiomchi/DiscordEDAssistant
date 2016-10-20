using System.Linq;
using System.Text;

namespace FlexLabs.EDAssistant.Services.Commands
{
    public class CommandTableContent : ICommandContent
    {
        public CommandTableContent(string[][] cells, int[] rightAlignedCells = null)
        {
            Cells = cells;
            RightAlignedCells = rightAlignedCells;
        }

        public string[][] Cells { get; }
        public int[] RightAlignedCells { get; }

        public string Format(string channel)
        {
            var lengths = new int[Cells[0].Length];

            for (int i = 0; i < lengths.Length; i++)
                lengths[i] = Cells.Max(r => r[i].Length);

            var sb = new StringBuilder();
            foreach (var row in Cells)
            {
                for (int i = 0; i < row.Length; i++)
                {
                    if (i > 0) sb.Append(" | ");
                    var value = row[i];
                    var rightAligned = RightAlignedCells?.Contains(i) == true;

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
