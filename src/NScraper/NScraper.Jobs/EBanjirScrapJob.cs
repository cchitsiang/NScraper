using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using NScraper.Data;

namespace NScraper.Jobs
{
    [Export(typeof(IJob))]
    public class EBanjirScrapeJob : IJob
    {
        public Lazy<Timer> Timer { get; private set; }

        private const string URL = "http://ebanjir.kelantan.gov.my/p_llrpt01.php";

        public EBanjirScrapeJob()
        {
            Timer = new Lazy<Timer>(() => new Timer(Scrape, null, TimeSpan.Zero, TimeSpan.FromSeconds(10)));
        }

        public void Scrape(object message)
        {
            Console.WriteLine("Scraping {0}", URL);
            var htmlDoc = Scraper.GetHtmlDocument(URL);
            var page = htmlDoc.DocumentNode;
            var table = page.Descendants().FirstOrDefault(x => x.Name == "table" && x.Attributes.Contains("class")
                                                      && x.Attributes["class"].Value.Split().Contains("Grid_theme"));
            var dt = Scraper.ToDataTable(table);

            if (dt == null)
                return; //TODO More handling if rows cannot be found

            foreach (DataRow row in dt.Rows)
            {
                var item = ParseRow(row);
                if(IsDuplicate(item))
                    continue;

                Add(item);
            }
        }

        public void Add(Item item)
        {
            using (var context = new NScaperContext())
            {
                item.CreatedAt = DateTime.UtcNow;
                context.Items.Add(item);
                context.SaveChanges();
            }
        }

        public bool IsDuplicate(Item item)
        {
            using (var context = new NScaperContext())
            {
                var record = context.Items.FirstOrDefault(x => x.Title == item.Title);

                return record != null;
            }
        }

        private Item ParseRow(DataRow row)
        {
            var jajahan = row["Jajahan"];
            var jalan = row["Jalan"];
            var kilometer = row["Kilometer"];
            var tempat = row["Tempat"];
            var tutupPada = row["Tutup Pada"];
            var nota = row["Nota"];
            var laluanAltenatif = row["Laluan Altenatif"];
            var bukaPada = row["Buka Pada"];

            var item = new Item();
            item.Title = string.Format("{0} - {1} tutup pada {2}", jajahan, tempat, tutupPada);

            var contentBuilder = new StringBuilder();
            contentBuilder.AppendLine("Jajahan: " + jajahan);
            contentBuilder.AppendLine("Jalan: " + jalan);
            contentBuilder.AppendLine("Kilometer: " + kilometer);
            contentBuilder.AppendLine("Tempat: " + tempat);
            contentBuilder.AppendLine("Tutup Pada: " + tutupPada);
            contentBuilder.AppendLine("Nota: " + nota);
            contentBuilder.AppendLine("Laluan Altenatif: " + laluanAltenatif);
            contentBuilder.AppendLine("Buka Pada: " + bukaPada);
            item.Content = contentBuilder.ToString();

            return item;
        }
    }
}
