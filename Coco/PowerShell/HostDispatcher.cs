using Coco.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Coco.PowerShell
{
    class HostDispatcher : IConsoleHost
    {
        private IConsoleHost _host;
        private SynchronizationContext _ownerContext;

        public HostDispatcher(IConsoleHost host, SynchronizationContext ownerContext)
        {
            _host = host;
            _ownerContext = ownerContext;
        }

        public Task Write(FormattedText text)
        {
            return _ownerContext.PostAsync(async () => await _host.Write(text));
        }

        public Task InsertLineBreak()
        {
            return _ownerContext.PostAsync(async () => await _host.InsertLineBreak());
        }

        public Task Clear()
        {
            return _ownerContext.PostAsync(async () => await _host.Clear());
        }
    }
}
