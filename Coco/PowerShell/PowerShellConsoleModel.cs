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
            await InvokeCommand(line, WriteOutputObject, WriteErrorObject);
            await WritePrompt();
        }

        private async Task WritePrompt()
        {
            bool first = true;
            await InvokeCommand("prompt", async item =>
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
            }, WriteErrorObject);
        }

        private Task WriteOutputObject(object obj)
        {
            return TaskEx.FromCompleted();
        }

        private Task WriteErrorObject(object obj)
        {
            ErrorRecord er = obj as ErrorRecord;
            if (er == null)
            {
                IContainsErrorRecord cer = obj as IContainsErrorRecord;
                if (cer != null)
                {
                    er = cer.ErrorRecord;
                }
                else
                {
                    Exception ex = obj as Exception;
                    if (ex != null)
                    {
                        er = new ErrorRecord(ex, "Error.Unknown", ErrorCategory.InvalidOperation, null);
                    }
                    else
                    {
                        er = new ErrorRecord(new Exception(obj.ToString()), "Error.Unknown", ErrorCategory.InvalidOperation, null);
                    }
                }
            }
            Host.WriteLine(new FormattedText(TextClassifications.Error, er.ToString()));
            return TaskEx.FromCompleted();
        }

        private async Task InvokeCommand(string command, Func<object, Task> outputStream, Func<object, Task> errorStream)
        {
            Pipeline pipe = _runspace.CreatePipeline(command);
            TaskCompletionSource<AsyncVoid> release = new TaskCompletionSource<AsyncVoid>();
            pipe.StateChanged += (sender, args) =>
            {
                if (args.PipelineStateInfo.State == PipelineState.Completed || 
                    args.PipelineStateInfo.State == PipelineState.Stopped || 
                    args.PipelineStateInfo.State == PipelineState.Failed)
                {
                    release.TrySetResult(AsyncVoid.Value);
                }
            };
            pipe.InvokeAsync();
            while (!release.Task.IsCompleted)
            {
                foreach (var outputObject in pipe.Output.NonBlockingRead())
                {
                    await outputStream(outputObject);
                }
                foreach (var errorObject in pipe.Error.NonBlockingRead())
                {
                    await errorStream(errorObject);
                }
                await Task.Yield();
            }

            // Flush anything left in the streams
            foreach (var outputObject in pipe.Output.ReadToEnd())
            {
                await outputStream(outputObject);
            }
            foreach (var errorObject in pipe.Error.ReadToEnd())
            {
                await errorStream(errorObject);
            }

            // Check for an exception
            if (pipe.PipelineStateInfo.State == PipelineState.Failed && pipe.PipelineStateInfo.Reason != null)
            {
                await errorStream(pipe.PipelineStateInfo.Reason);
            }
        }
    }
}
