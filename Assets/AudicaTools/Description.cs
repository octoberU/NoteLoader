using Newtonsoft.Json;
using System;
using System.IO;

namespace AudicaTools
{
    [Serializable]
    public class Description
    {
        public string songID;
        public string moggSong;
        public string title;
        public string artist;
        public string author;
        public string midiFile;
        public string targetDrums;
        public string sustainSongLeft;
        public string sustainSongRight;
        public string fxSong;
        public string songEndEvent;
        public string highScoreEvent;
        public float songEndPitchAdjust;
        public float prerollSeconds;
        public double previewStartSeconds;
        public bool useMidiForCues;
        public bool hidden;

        //Extra modded entries
        public string customExpert = null;
        public string customAdvanced = null;
        public string customModerate = null;
        public string customBeginner = null;

        public MemoryStream GetMemoryStream()
        {
            var jsonString = JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            return Utility.GenerateStreamFromString(jsonString);
        }

    }

}