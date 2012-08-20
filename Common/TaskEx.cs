using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Threading.Tasks
{
    internal struct AsyncVoid {
        public static readonly AsyncVoid Value = new AsyncVoid();
    }

    internal static class TaskEx
    {
        private static readonly Task CompletedVoidTask = Task.FromResult<AsyncVoid>(AsyncVoid.Value);

        public static Task FromCompleted()
        {
            return CompletedVoidTask;
        }
    }
}
