using Coco.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coco
{
    public class EchoConsoleModel : ConsoleModel
    {
        public override async Task LineCommitted(string line)
        {
            await Host.InsertLineBreak();
            await Host.WriteLine("ECHO: " + line);
            await Host.Write("Input: ");
        }
    }
}
