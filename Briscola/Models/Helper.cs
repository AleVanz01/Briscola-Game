using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Briscola.Models
{
    public static class Helper
    {
        public static void RemoveFirst<T>(this List<T> lista) => lista.RemoveAt(0);

        public static async void RunTemporized(Action action, TimeSpan period, CancellationToken cancellationToken, IErrorHandler handler = null)
        {
            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    await Task.Delay(period, cancellationToken).ConfigureAwait(true);

                    if (!cancellationToken.IsCancellationRequested)
                    {
                        action();
                    }
                }
            }
            catch (Exception ex)
            {
                handler?.HandleError(ex);
            }
        }

        public static void RunTemporized(Action action, TimeSpan period) =>
                    RunTemporized(action, period, CancellationToken.None);
    }

    public interface IErrorHandler
    {
        void HandleError(Exception ex);
    }
}
