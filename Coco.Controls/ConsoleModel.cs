using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Coco.Controls
{
    public interface IConsoleModel
    {
        Task Attach(IConsoleHost host);
        Task Detach(IConsoleHost host);
        Task LineCommitted(string line);
    }

    public abstract class ConsoleModel : IConsoleModel
    {
        public static readonly IConsoleModel Null = new NullConsoleModel();

        protected IConsoleHost Host { get; private set; }
        
        public abstract Task LineCommitted(string line);
        
        public virtual Task Attach(IConsoleHost host)
        {
            Host = host;
            return TaskEx.FromCompleted();
        }

        public virtual Task Detach(IConsoleHost host)
        {
            if (ReferenceEquals(Host, host))
            {
                Host = null;
            }
            return Task.FromResult<object>(null);
        }

        private class NullConsoleModel : ConsoleModel
        {
            public override Task LineCommitted(string line) { return Task.FromResult<object>(null); }
        }
    }
}
