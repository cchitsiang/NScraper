using System;
using System.Collections.Generic;

namespace NScraper
{
    public static class Jobs
    {
        private static readonly IList<IJob> _jobs = new List<IJob>();

        public static void Add(IJob job)
        {
            _jobs.Add(job);
        }

        public static IEnumerable<IJob> Get()
        {
            return _jobs;
        }

        public static void Start()
        {
            foreach (var job in _jobs)
            {
                var start = job.Timer.Value;
            }
        }
    }
}
