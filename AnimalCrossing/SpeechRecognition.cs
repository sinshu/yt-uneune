using System;
using System.IO;
using System.Text;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

namespace AnimalCrossing
{
    public static class SpeechRecognition
    {
        public static async Task<bool> DoSingle(string srcFile, string dstFile)
        {
            if (File.Exists(dstFile))
            {
                Console.WriteLine("SKIP: " + srcFile);
                return false;
            }

            Console.WriteLine("================================================================================");
            Console.WriteLine(" " + srcFile);
            Console.WriteLine("================================================================================");

            var config = Config.GetInstance();

            var sc = SpeechConfig.FromSubscription(config.AzureKey, config.AzureRegion);
            sc.SpeechRecognitionLanguage = "ja-JP";

            var results = new List<string>();
            var tcs = new TaskCompletionSource();
            using (var ac = AudioConfig.FromWavFileInput(srcFile))
            using (var recognizer = new SpeechRecognizer(sc, ac))
            {
                recognizer.Recognized += (sender, e) =>
                {
                    var time = new TimeSpan((long)e.Offset).ToString(@"hh\:mm\:ss");
                    var result = time + " " + e.Result.Text;
                    Console.WriteLine(result);
                    results.Add(result);
                };

                recognizer.SessionStopped += (sender, e) =>
                {
                    tcs.SetResult();
                };

                await recognizer.StartContinuousRecognitionAsync();
                await tcs.Task;
                await recognizer.StopContinuousRecognitionAsync();
            }

            Console.WriteLine();

            using (var writer = new StreamWriter(dstFile, false, Encoding.UTF8))
            {
                foreach (var result in results)
                {
                    writer.WriteLine(result);
                }
            }

            return true;
        }

        public static async Task DoMultiple(int count)
        {
            var config = Config.GetInstance();

            var i = 0;
            foreach (var line in File.ReadLines(config.VideoListPath, Encoding.UTF8))
            {
                var split = line.Split(',');
                var id = split[0];

                var srcFile = Path.Combine(config.Wav16kDirectory, id + ".wav");
                var dstFile = Path.Combine(config.RecogDirectory, id + ".txt");

                if (await DoSingle(srcFile, dstFile))
                {
                    i++;
                }

                if (i == count)
                {
                    break;
                }
            }
        }
    }
}
