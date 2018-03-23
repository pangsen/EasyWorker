using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using Newtonsoft.Json;
using NUnit.Framework;
using Worker.Implementation;
using Worker.Interface;

namespace Worker.Test
{

    public class SiteProfile : Message
    {
        public string SiteAddress { get; set; }
    }

    public class ArticleProfile : Message
    {
        public string NugetLink { get; set; }
        public string ProjectLink { get; set; }
        public string Title { get; set; }
    }

    public class SiteHandler : IHander<SiteProfile>
    {
        public void Handle(SiteProfile message, CancellationToken cancellationToken)
        {
            var document = new HtmlDocument();
            using (var client = new HttpClient())
            {
                var html = client.GetStringAsync(message.SiteAddress).Result;
                document.LoadHtml(html);
            }
            var articles = document.DocumentNode.QuerySelectorAll(".list-packages .package a.package-title")?.Select(
                a =>
                    new ArticleProfile
                    {
                        NugetLink = "https://www.nuget.org" + a.Attributes["href"]?.Value,
                        Title = a.InnerText
                    });
            Console.WriteLine(message.SiteAddress);
            WorkerProvider.GetWorker().Publish(articles);
        }
    }
    public class ArticleHandler : IHander<ArticleProfile>
    {
        public void Handle(ArticleProfile message, CancellationToken cancellationToken)
        {
            var document = new HtmlDocument();
            using (var client = new HttpClient())
            {
                var html = client.GetStringAsync(message.NugetLink).Result;
                document.LoadHtml(html);
            }
            message.ProjectLink = document.DocumentNode.QuerySelector("a[data-track='outbound-project-url']")?.Attributes["href"]?.Value;
            Console.WriteLine($"{message.Title}:{message.ProjectLink}");
        }
    }
    public class GetUngetPackagesTest
    {
        [Test]
        public void GetUngetPackages()
        {
            var worker = WorkerProvider.GetWorker();
            worker.AddHandler(new SiteHandler());
            worker.AddHandler(new ArticleHandler());
            worker.Start();
            foreach (var i in Enumerable.Range(1, 2))
            {
                worker.Publish(new SiteProfile
                {
                    SiteAddress = "https://www.nuget.org/packages?page=" + i
                });
            }

            worker.WaitUntilNoMessage();
            worker.Stop();
        }
    }
}