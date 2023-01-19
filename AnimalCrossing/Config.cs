using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AnimalCrossing
{
    public class Config
    {
        private static readonly string rootPath = @"D:\yt-uneune-storage";

        private static readonly string ytDlpPath = Path.Combine(rootPath, "bin", "yt-dlp", "yt-dlp.exe");
        private static readonly string soxPath = Path.Combine(rootPath, "bin", "sox", "sox.exe");
        private static readonly string videoListPath = Path.Combine(rootPath, "videolist.csv");
        private static readonly string temporaryAudioPath = Path.Combine(rootPath, "tmp.wav");
        private static readonly string wav16kDirectory = Path.Combine(rootPath, "wav16k");
        private static readonly string recogDirectory = Path.Combine(rootPath, "recog");

        private readonly string azureKey;
        private readonly string azureRegion;
        private readonly string azureEndPoint;

        private Config(string path)
        {
            var dic = File
                .ReadLines(path, Encoding.UTF8)
                .Select(line => line.Split('='))
                .ToDictionary(pair => pair[0], pair => pair[1]);

            azureKey = dic["AZURE_KEY"];
            azureRegion = dic["AZURE_REGION"];
            azureEndPoint = dic["AZURE_ENDPOINT"];
        }

        public static Config GetInstance()
        {
            return new Config(Path.Combine(rootPath, "settings.cfg"));
        }

        public string YtDlpPath => ytDlpPath;
        public string SoxPath => soxPath;
        public string VideoListPath => videoListPath;
        public string TemporaryAudioPath => temporaryAudioPath;
        public string Wav16kDirectory = wav16kDirectory;
        public string RecogDirectory = recogDirectory;

        public string AzureKey => azureKey;
        public string AzureRegion => azureRegion;
        public string AzureEndPoint => azureEndPoint;
    }
}
