using System;

namespace AudicaAPI
{
    [Serializable]
    public class Song
    {
        public string song_id;
        public string author;
        public string title;
        public string artist;
        public bool beginner;
        public bool standard;
        public bool advanced;
        public bool expert;
        public string download_url;
        public string preview_url;
        public string upload_time;
        public int leaderboard_scores;
        public string video_url;
        public string filename;
        public DateTime GetDate()
        {
            string[] day = this.upload_time.Split(new char[] { ' ', '-' });
            string[] time = this.upload_time.Split(new char[] { ' ', ':', '.' });
            return new DateTime(Int32.Parse(day[0]),
                Int32.Parse(day[1]),
                Int32.Parse(day[2]),
                Int32.Parse(time[1]),
                Int32.Parse(time[2]),
                Int32.Parse(time[3]));
        }
    }

}