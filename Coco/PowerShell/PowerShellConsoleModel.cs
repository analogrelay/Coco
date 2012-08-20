using Coco.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Coco.PowerShell
{
    public class PowerShellConsoleModel : ConsoleModel
    {
        private PowerShellHost _host;
        private Runspace _runspace;
        private HostDispatcher _hostDispatcher;

        internal IConsoleHost ConsoleHost { get { return _hostDispatcher; } }

        public PowerShellConsoleModel()
        {
            // Set up Session State
            _host = new PowerShellHost(this);
            _runspace = RunspaceFactory.CreateRunspace(_host);
        }

        public override async Task Attach(IConsoleHost host)
        {
            var context = SynchronizationContext.Current;

            await Task.WhenAll(base.Attach(host), _runspace.OpenTaskAsync());

            _hostDispatcher = new HostDispatcher(Host, context);

            // Display Prompt
            await WritePrompt();
        }

        public override async Task LineCommitted(string line)
        {
            await Host.InsertLineBreak();
            await InvokeAndWriteResults(line);
            await WritePrompt();
        }

        private async Task WritePrompt()
        {
            var items = await InvokeSimpleCommand("prompt");
            bool first = true;
            foreach (var item in items)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    await Host.InsertLineBreak();
                }
                await Host.Write(item.ToString());
            }
        }

        private async Task InvokeAndWriteResults(string command)
        {
            var result = await InvokeSimpleCommand(command);
            foreach (var item in result)
            {
                await Host.WriteLine(item.ToString());
            }
        }

        private async Task<ICollection<PSObject>> InvokeSimpleCommand(string command)
        {
            Pipeline pipe = _runspace.CreatePipeline(command);
            return await pipe.InvokeTaskAsync();
        }
    }
}
