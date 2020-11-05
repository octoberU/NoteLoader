using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using DifficultyCalculation;
using NAudio.Midi;
using NAudio.Mixer;
using Newtonsoft.Json;
namespace AudicaTools
{
    public class Audica
    {
        public Difficulty beginner;
        public Difficulty moderate;
        public Difficulty advanced;
        public Difficulty expert;

        public MidiFile midi;
        public List<TempoData> tempoData = new List<TempoData>();

        public Description desc;

        public MoggSong moggSong;
        public MonoMoggSong moggSongSustainL;
        public MonoMoggSong moggSongSustainR;

        public Mogg song;
        public Mogg songSustainL;
        public Mogg songSustainR;

        public Audica(string filePath)
        {
            CheckPath(filePath);
            ZipArchive zip = ZipFile.OpenRead(filePath);
            this.desc = ReadJsonEntry<Description>(zip, "song.desc");
            this.expert = ReadJsonEntry<Difficulty>(zip, "expert.cues");
            this.advanced = ReadJsonEntry<Difficulty>(zip, "advanced.cues");
            this.moderate = ReadJsonEntry<Difficulty>(zip, "moderate.cues");
            this.beginner = ReadJsonEntry<Difficulty>(zip, "beginner.cues");
            
            this.moggSong = new MoggSong(zip.GetEntry(this.desc.moggSong).Open());
            if(this.desc.sustainSongLeft != "") this.moggSongSustainL = new MonoMoggSong(zip.GetEntry(this.desc.sustainSongLeft).Open());
            if(this.desc.sustainSongRight != "") this.moggSongSustainR = new MonoMoggSong(zip.GetEntry(this.desc.sustainSongRight).Open());

            if(this.moggSongSustainL != null) this.songSustainL = new Mogg(zip.GetEntry(moggSongSustainL.moggPath).Open());
            if(this.moggSongSustainR != null) this.songSustainR = new Mogg(zip.GetEntry(moggSongSustainR.moggPath).Open());

            ZipArchiveEntry songEntry = zip.GetEntry(moggSong.moggPath);
            if(songEntry != null) this.song = new Mogg(songEntry.Open());
            
            //this.midi = zip.GetEntry(desc.midiFile).Open();
            this.midi = new MidiFile(zip.GetEntry(desc.midiFile).Open(), true);
            this.tempoData = ReadTempoEvents(midi.Events);
            //this.moggSongSustainL = new MoggSong(zip.GetEntry(this.desc.sustainSongLeft).Open());
            //this.moggSongSustainR = new MoggSong(zip.GetEntry(this.desc.sustainSongRight).Open());
        }

        
        public List<TempoData> ReadTempoEvents(MidiEventCollection events)
        {
            var tempList = new List<TempoData>();
            foreach (var eventList in events)
            {
                foreach (var e in eventList)
                {
                    if (e is TempoEvent)
                    {
                        TempoEvent tempo = (e as TempoEvent);
                        tempList.Add(new TempoData((int)tempo.AbsoluteTime, (ulong)tempo.MicrosecondsPerQuarterNote));
                    }
                }
            }
            return tempList;
        }

        private static void CheckPath(string filePath)
        {
            if (!File.Exists(filePath))
                throw new ArgumentException("Audica file path doesn't exist", filePath);
            else if (!filePath.Contains(".audica"))
                throw new ArgumentException("File path doesn't lead to an .audica file", filePath);
        }

        private static T ReadJsonEntry<T>(ZipArchive zip, string entryName)
        {
            if (zip.GetEntry(entryName) == null) return default(T);
            var descStream = zip.GetEntry(entryName)
                .Open();
            using (var reader = new StreamReader(descStream))
            {
                string text = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<T>(text);
            }
        }

        public static Description GetDescOnly(string filePath)
        {
            CheckPath(filePath);
            ZipArchive zip = ZipFile.OpenRead(filePath);
            return ReadJsonEntry<Description>(zip, "song.desc");
        }

        public void Export(string filePath)
        {
            using (FileStream zipFile = File.Open(filePath, FileMode.Create))
            {
                using (var zipArchive = new ZipArchive(zipFile, ZipArchiveMode.Create, false))
                {
                    AddEntryFromStream(zipArchive, "song.desc", desc.GetMemoryStream());
                    
                    if(expert != null) AddEntryFromStream(zipArchive, "expert.cues", expert.GetMemoryStream());
                    if (advanced != null) AddEntryFromStream(zipArchive, "advanced.cues", advanced.GetMemoryStream());
                    if (moderate != null) AddEntryFromStream(zipArchive, "moderate.cues", moderate.GetMemoryStream());
                    if (beginner != null) AddEntryFromStream(zipArchive, "beginner.cues", beginner.GetMemoryStream());

                    AddEntryFromStream(zipArchive, moggSong.moggPath, song.GetMemoryStream());
                    if (this.songSustainL != null) AddEntryFromStream(zipArchive, moggSongSustainL.moggPath, songSustainL.GetMemoryStream());
                    if (this.songSustainR != null) AddEntryFromStream(zipArchive, moggSongSustainR.moggPath, songSustainR.GetMemoryStream());

                    AddEntryFromStream(zipArchive, desc.moggSong, moggSong.GetMemoryStream());

                    AddEntryFromStream(zipArchive, desc.midiFile, ExportTempoEvents());
                }

            }
        }
        private void AddEntryFromStream(ZipArchive zip, string entryName, MemoryStream ms)
        {
            var zipEntry = zip.CreateEntry(entryName, CompressionLevel.NoCompression);
            using (var zipEntryStream = zipEntry.Open())
            {
                ms.CopyTo(zipEntryStream);
            }
        }

        private void AddEntryFromStream(ZipArchive zip, string entryName, Stream stream)
        {
            var zipEntry = zip.CreateEntry(entryName);
            using (var zipEntryStream = zipEntry.Open())
            {
                stream.CopyTo(zipEntryStream);
            }
        }

        private MemoryStream ExportTempoEvents()
        {
            MidiEventCollection events = new MidiEventCollection(0, 480);
            foreach (var tempo in tempoData)
            {
                events.AddEvent(new TempoEvent((int)tempo.microsecondsPerQuarterNote, (long)tempo.tick), 0);
            }
            events.PrepareForExport();

            return Utility.ExportMidiToStream(events);
        }

        public DifficultyCalculator GetDifficultyRatings()
        {
            return new DifficultyCalculator(this);
        }
        
        public float GetRatingForDifficulty(Difficulty difficulty)
        {
            var calculatedDiff = new CalculatedDifficulty(difficulty, this.tempoData);
            return calculatedDiff.difficultyRating;
        }
    }

}