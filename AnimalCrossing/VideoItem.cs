using System;

namespace AnimalCrossing
{
    public class VideoItem
    {
        private string url;
        private string title;
        private int number;

        private string id;

        internal VideoItem(string url, string title, int number)
        {
            this.url = url;
            this.title = title;
            this.number = number;

            id = "animal" + number.ToString("000");
        }

        public override string ToString()
        {
            return $"({url}, {title})";
        }

        public string Url => url;
        public string Title => title;
        public int Number => number;
        public string Id => id;
    }
}
