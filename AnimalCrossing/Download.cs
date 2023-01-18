using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Cysharp.Diagnostics;
using Zx;

namespace AnimalCrossing
{
    public static class Download
    {
        public static async Task DoSingle(string url, string dstFile)
        {
            var config = Config.GetInstance();

            if (File.Exists(config.TemporaryAudioPath))
            {
                throw new Exception("Temporary audio file already exists!");
            }

            await $"{config.YtDlpPath} -x --audio-format wav -o {config.TemporaryAudioPath} {url}";

            if (!File.Exists(config.TemporaryAudioPath))
            {
                throw new Exception("Failed to extract the audio file.");
            }

            await $"{config.SoxPath} {config.TemporaryAudioPath} -c 1 -r 16000 {dstFile}";

            if (!File.Exists(dstFile))
            {
                throw new Exception("Failed to process the audio file.");
            }

            File.Delete(config.TemporaryAudioPath);
        }

        public static async Task DoMultiple(int count, int delayInMinutes)
        {
            var config = Config.GetInstance();

            var i = 0;
            foreach (var line in File.ReadLines(config.VideoListPath, Encoding.UTF8))
            {
                var split = line.Split(',');
                var id = split[0];
                var url = split[1];
                var title = split[2];

                var dstFile = Path.Combine(config.Wav16kDirectory, id + ".wav");
                if (File.Exists(dstFile))
                {
                    Console.WriteLine("SKIP: " + dstFile);
                    continue;
                }

                Console.WriteLine("================================================================================");
                Console.WriteLine(" " + title);
                Console.WriteLine("================================================================================");

                await DoSingle(url, dstFile);

                await Task.Delay(TimeSpan.FromMinutes(delayInMinutes));

                Console.WriteLine();

                i++;
                if (i == count)
                {
                    break;
                }
            }
        }
    }
}
