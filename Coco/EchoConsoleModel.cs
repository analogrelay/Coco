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
        public override Task LineCommitted(string line)
        {
            Host.InsertLineBreak();
            Host.WriteLine("ECHO: " + line);
            Host.Write("Input: ");
            return Task.FromResult<object>(null);
        }
    }
}
