using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Cysharp.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AnimalCrossing
{
    public class Preprocess
    {
        private static readonly Regex regexPartNumber = new Regex(@"part(\d+)");

        private static readonly string urlPlaylist = @"https://www.youtube.com/playlist?list=PLc2Ya56D22xtsqtjN71_6aYQC8eahO5rp";

        public static async Task UpdateVideoList()
        {
            Console.Write("UpdateVideoList ");

            var config = Config.GetInstance();

            using (var writer = new StreamWriter(config.VideoListPath, false, Encoding.UTF8))
            {
                await foreach (var item in GetVideoItemsAsync(config))
                {
                    writer.WriteLine(item.Id + "," + item.Url + "," + item.Title);
                    Console.Write(".");
                }
            }

            Console.WriteLine(" OK");
        }

        private static async IAsyncEnumerable<VideoItem> GetVideoItemsAsync(Config config)
        {
            var command = $"{config.YtDlpPath} -j --flat-playlist {urlPlaylist}";

            await foreach (var json in ProcessX.StartAsync(command))
            {
                var obj = JObject.Parse(json);

                var url = obj["url"]!.ToString();
                var title = obj["title"]!.ToString();

                var number = int.Parse(regexPartNumber.Match(title).Groups[1].Value);
                if (title.Contains("amiibo"))
                {
                    number += 900;
                }

                yield return new VideoItem(url, title, number);
            }
        }
    }
}
