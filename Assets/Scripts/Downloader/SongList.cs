using System;

namespace AudicaAPI
{
    [Serializable]
    public class SongList
    {
        public int total_pages;
        public int song_count;
        public Song[] songs;
        public int pagesize;
        public int page;
    }
}