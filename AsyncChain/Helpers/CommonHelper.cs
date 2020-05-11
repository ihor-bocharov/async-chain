using System;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace TestAsync.Helpers
{
    public static class CommonHelper
    {
        private static readonly object locker = new object();

        public static void LogRequestTreadInfo<T>(ILogger<T> logger, int index, string message = "")
        {
            lock (locker)
            {
                int work, completionPort;
                ThreadPool.GetAvailableThreads(out work, out completionPort);

                var threadId = Thread.CurrentThread.ManagedThreadId;
                var time = DateTime.Now.ToString("HH:mm:ss.fff").PadLeft(15);
                logger.LogInformation($"{time} Request id: {index}; ThreadId: {threadId}; Available threads: work - {work}, i/o - {completionPort}; Notes: {message}");
            }
        }
    }
}
