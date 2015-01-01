using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NScraper.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Factory.StartNew(StartScraping);
            System.Console.ReadLine();
        }

        static void StartScraping()
        {
            Scraper.StartJobs();
        }
    }


}
