using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Linq;

namespace Gsafety.PTMS.Media
{
    public class TaskExt
    {
        public static Task Delay(int millisecondsDelay)
        {
            return Task.Factory.StartNew(() => Thread.Sleep(millisecondsDelay));
        }

        public static Task Delay(int millisecondsDelay, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(() => Thread.Sleep(millisecondsDelay), cancellationToken);
        }

        public static Task<TResult> FromResult<TResult>(TResult result)
        {
            return Task.Factory.StartNew<TResult>(() => { return result; });
        }

        public static Task WhenAll(IEnumerable<Task> tasks)
        {
            return Task.Factory.ContinueWhenAll(tasks.ToArray(), t => { });
        }

        public static Task<TResult[]> WhenAll<TResult>(IEnumerable<Task<TResult>> tasks)
        {
            return Task.Factory.ContinueWhenAll<TResult[]>(tasks.ToArray(), taskList => taskList.Select(t => ((Task<TResult>)t).Result).ToArray());
        }

        public static Task<Task> WhenAny(params Task[] tasks)
        {
            return Task.Factory.ContinueWhenAny(tasks.ToArray(), task => { return task; });
        }

        public static Task Run(Action action)
        {
            return Task.Factory.StartNew(action);
        }

        public static Task Run(Action action, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(action, cancellationToken);
        }
    }
}
