using System;
using System.Collections.Generic;
using System.Linq;
using Bichebot.Core.Utilities;
using HtmlAgilityPack;

namespace Bichebot.Domain.Modules.FOnlineStatistics
{
    public class StatisticsProvider : IStatisticsProvider
    {
        private readonly string host = "http://www.fallout-requiem.ru";
        public Result<IEnumerable<FoStatistics>> GetTotalStatistics()
        {
            try
            {
                var url = $"{host}/main.php";
                var web = new HtmlWeb();
                var doc = web.Load(url);

                var table = doc.GetElementbyId("table");

                var rows = table.ChildNodes.Where(n => n.Name == "tr");

                return Result<IEnumerable<FoStatistics>>.Ok(rows.Select(ParsePlayer).ToArray());
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public Result<CharacterStatistics> GetCharacterStatistics(string link)
        {
            try
            {
                var url = $"{host}/{link}";
                var web = new HtmlWeb();
                var doc = web.Load(url);

                var body = doc.DocumentNode.ChildNodes["html"].ChildNodes["body"];
                var name = body.ChildNodes.First(n => n.HasClass("title")).InnerText;
                var container = body.ChildNodes.First(n => n.HasClass("container"));
                var tables = container
                    .ChildNodes.Where(n => n.HasClass("block1")).Select(n => n.ChildNodes.First(t => t.HasClass("table")))
                    .ToArray();

                var kills = ParseKills(tables[0]);
                var deaths = ParseKills(tables[1]);

                return new CharacterStatistics
                {
                    Name = name,
                    Kills = kills,
                    Deaths = deaths
                };
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        
        private FoStatistics ParsePlayer(HtmlNode node)
        {
            var nodes = node.ChildNodes.Where(n => n.Name == "td").ToArray();
            var link = nodes[1].ChildNodes[0].Attributes["href"].Value;
            return new FoStatistics
            {
                Rating = int.Parse(nodes[0].InnerText),
                Player = nodes[1].ChildNodes[0].InnerText,
                Kills = int.Parse(nodes[3].InnerText),
                Death = int.Parse(nodes[5].InnerText),
                Link = link
            };
        }

        private Kill[] ParseKills(HtmlNode node)
        {
            return node.ChildNodes
                .Where(n => n.Name == "tr")
                .Select(n => n.ChildNodes[5].ChildNodes[0].InnerText)
                .Select(name => new Kill {Name = name})
                .ToArray();
        }
    }
}