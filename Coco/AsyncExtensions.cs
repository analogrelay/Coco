using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Coco
{
    public static class AsyncExtensions
    {
        public static Task OpenTaskAsync(this Runspace self)
        {
            TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
            self.StateChanged += (sender, args) =>
            {
                if (args.RunspaceStateInfo.State == RunspaceState.Opened)
                {
                    tcs.TrySetResult(null);
                }
                else if (args.RunspaceStateInfo.State == RunspaceState.Broken)
                {
                    tcs.TrySetException(args.RunspaceStateInfo.Reason);
                }
            };
            self.OpenAsync();
            return tcs.Task;
        }

        public static Task<ICollection<PSObject>> InvokeTaskAsync(this Pipeline self)
        {
            TaskCompletionSource<ICollection<PSObject>> tcs = new TaskCompletionSource<ICollection<PSObject>>();
            self.StateChanged += (sender, args) =>
            {
                if (args.PipelineStateInfo.State == PipelineState.Completed)
                {
                    tcs.TrySetResult(self.Output.ReadToEnd());
                }
                else if (args.PipelineStateInfo.State == PipelineState.Failed)
                {
                    tcs.TrySetException(args.PipelineStateInfo.Reason);
                }
                else if (args.PipelineStateInfo.State == PipelineState.Stopped)
                {
                    tcs.TrySetCanceled();
                }
            };
            self.InvokeAsync();
            return tcs.Task;
        }

        public static Task<PSDataCollection<PSObject>> InvokeAsync(this System.Management.Automation.PowerShell self)
        {
            return Task.Factory.FromAsync(self.BeginInvoke(), ar => self.EndInvoke(ar));
        }

        public static Task PostAsync(this SynchronizationContext self, Action action)
        {
            return self.PostAsync(() => { action(); return TaskEx.FromCompleted(); });
        }

        public static Task PostAsync(this SynchronizationContext self, Func<Task> action)
        {
            TaskCompletionSource<AsyncVoid> tcs = new TaskCompletionSource<AsyncVoid>();
            self.Post(async _ =>
            {
                await action();
                tcs.TrySetResult(AsyncVoid.Value);
            }, null);
            return tcs.Task;
        }

        public static Task<T> PostAsync<T>(this SynchronizationContext self, Func<T> func)
        {
            return self.PostAsync(() => Task.FromResult(func()));
        }

        public static Task<T> PostAsync<T>(this SynchronizationContext self, Func<Task<T>> func)
        {
            TaskCompletionSource<T> tcs = new TaskCompletionSource<T>();
            self.Post(async _ =>
            {
                tcs.TrySetResult(await func());
            }, null);
            return tcs.Task;
        }
    }
}
