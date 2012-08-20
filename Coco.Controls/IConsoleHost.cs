using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coco.Controls
{
    public interface IConsoleHost
    {
        Task Write(FormattedText text);
        Task InsertLineBreak();
        Task Clear();
    }

    public static class ConsoleHostExtensions
    {
        public static Task Write(this IConsoleHost self, string text)
        {
            return self.Write(new FormattedText(
                new FormattedTextRun(TextClassifications.PlainText, text)));
        }

        public static async Task WriteLine(this IConsoleHost self, string line)
        {
            await self.Write(line);
            await self.InsertLineBreak();
        }

        public static async Task WriteLine(this IConsoleHost self, FormattedText line)
        {
            await self.Write(line);
            await self.InsertLineBreak();
        }
    }
}
