using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coco.Controls
{
    public interface IConsoleHost
    {
        Task Write(string text);
        Task InsertLineBreak();
        Task Clear();
    }

    public static class ConsoleHostExtensions
    {
        public static async Task WriteLine(this IConsoleHost self, string line)
        {
            await self.Write(line);
            await self.InsertLineBreak();
        }
    }
}
