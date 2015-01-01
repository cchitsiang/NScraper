using System;
using System.Threading;

namespace NScraper
{
    public interface IJob
    {
        Lazy<Timer> Timer { get; }

        void Scrape(object message);
    }
}
