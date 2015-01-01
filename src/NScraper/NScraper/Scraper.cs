using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using HtmlAgilityPack;

namespace NScraper
{
    public class Scraper
    {
        public static void StartJobs()
        {
            var jobsPath = System.IO.Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Jobs");
            if (!Directory.Exists(jobsPath))
            {
                return;
            }

            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new DirectoryCatalog(jobsPath));
            catalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetExecutingAssembly()));

            var container = new CompositionContainer(catalog);
            container.ComposeParts();

            var exports = container.GetExportedValues<IJob>();

            foreach (var job in exports)
            {
                Jobs.Add(job);
            }

            Jobs.Start();
        }

        public static string DownloadUrl(string url)
        {
            return new System.Net.WebClient().DownloadString(url);
        }

        public static HtmlDocument GetHtmlDocument(string url)
        {
            var html = Scraper.DownloadUrl(url);
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            return htmlDoc;
        }

        public static DataTable ToDataTable(HtmlNode table)
        {
            if (table != null)
            {
                var nodes = table.SelectNodes("tr");
                if(nodes == null)
                    return null;
                var dt = new DataTable("DataTable");

                var headers = nodes[0]
                    .Elements("th")
                    .Select(th => th.InnerText.Trim());
                foreach (var header in headers)
                {
                    dt.Columns.Add(header);
                }

                var rows = nodes.Skip(1).Select(tr => tr
                    .Elements("td")
                    .Select(td => td.InnerText.Trim())
                    .ToArray());
                foreach (var row in rows)
                {
                    dt.Rows.Add(row);
                }

                return dt;
            }

            return null;
        }
    }
}
