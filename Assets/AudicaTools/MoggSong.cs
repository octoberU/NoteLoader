using System;
using System.IO;
using System.Text;

namespace AudicaTools
{
    public class MoggSong
    {
        public MoggVol volume;
        public string moggPath;
        public MoggVol pan;
        public string[] moggString;

        public MoggSong(Stream stream)
        {
            var ms = new MemoryStream();
            stream.CopyTo(ms);
            moggString = Encoding.UTF8.GetString(ms.ToArray()).Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in moggString)
            {
                if (line.Contains("(mogg_path")) GetMoggPathFromLine(line);
                if (line.Contains("(vol")) this.volume = GetMoggVolFromLine(line);
                if (line.Contains("(pans")) this.pan = GetMoggVolFromLine(line);
            }
        }

        public void GetMoggPathFromLine(string line)
        {
            var split = line.Split(new string[] { "\"" }, StringSplitOptions.None);
            moggPath = split[1];
        }
        public MoggVol GetMoggVolFromLine(string line)
        {
            try
            {
                var split = line.Split(new char[] { '(', ')' });
                string[] values;
                if (split[2].Contains("    ")) values = split[2].Split(new string[] { "    " }, StringSplitOptions.None);
                else if (split[2].Contains("   ")) values = split[2].Split(new string[] { "   " }, StringSplitOptions.None);
                else if (split[2].Contains("  ")) values = split[2].Split(new string[] { "  " }, StringSplitOptions.None);
                else values = split[2].Split(new string[] { " " }, StringSplitOptions.None);
                return new MoggVol(float.Parse(values[0]), float.Parse(values[1]));
            }
            catch (Exception)
            {
                Console.WriteLine("Moggsong is invalid. Using defaults instead");
                return new MoggVol(0f, 0f);
            }
        }

        public string ExportToText()
        {
            string[] exportString = moggString;
            int volIndex = 0;
            int panIndex = 0;
            for (int i = 0; i < exportString.Length; i++)
            {
                exportString[i].Replace("\n", "");
                if (exportString[i].Contains("(vols")) volIndex = i;
                if (exportString[i].Contains("(pan")) panIndex = i;
            }
            exportString[volIndex] = $"(vols ({volume.l.ToString("n2")}   {volume.r.ToString("n2")}))";
            exportString[panIndex] = $"(pans ({pan.l.ToString("n2")}   {pan.r.ToString("n2")}))";
            return string.Join(Environment.NewLine, exportString);
        }

        public void SetVolume(float value)
        {
            this.volume.l = this.volume.r = value;
            this.pan.l = -1;
            this.pan.r = 1;
        }

        public struct MoggVol
        {
            public float l;
            public float r;

            public MoggVol(float l, float r)
            {
                this.l = l;
                this.r = r;
            }
        }

        public MemoryStream GetMemoryStream()
        {
            var moggString = ExportToText();
            return Utility.GenerateStreamFromString(moggString);
        }
    }

    public class MonoMoggSong
    {
        public float volume;
        public string moggPath;
        public float pan;
        public string[] moggString;

        public MonoMoggSong(Stream stream)
        {
            var ms = new MemoryStream();
            stream.CopyTo(ms);
            moggString = Encoding.UTF8.GetString(ms.ToArray()).Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in moggString)
            {
                if (line.Contains("(mogg_path")) GetMoggPathFromLine(line);
                if (line.Contains("(vol")) this.volume = GetVolFromLine(line);
                if (line.Contains("(pans")) this.pan = GetVolFromLine(line);
            }
        }

        public void GetMoggPathFromLine(string line)
        {
            var split = line.Split(new string[] { "\"" }, StringSplitOptions.None);
            moggPath = split[1];
        }
        public float GetVolFromLine(string line)
        {
            try
            {
                var split = line.Split(new char[] { '(', ')' });
                return float.Parse(split[3]);
            }
            catch (Exception)
            {
                Console.WriteLine("Mono Moggsong is invalid. Using defaults instead");
                return 0f;
            }
        }

        public string ExportToText()
        {
            string[] exportString = moggString;
            int volIndex = 0;
            int panIndex = 0;
            for (int i = 0; i < exportString.Length; i++)
            {
                exportString[i].Replace("\n", "");
                if (exportString[i].Contains("(vols")) volIndex = i;
                if (exportString[i].Contains("(pan")) panIndex = i;
            }
            exportString[volIndex] = $"(vols ({volume.ToString("n2")}))";
            exportString[panIndex] = $"(pans ({pan.ToString("n2")}))";
            return string.Join(Environment.NewLine, exportString);
        }

        public void SetVolume(float value)
        {
            this.volume = value;
            this.pan = -1f;
            this.pan = 1f;
        }

        public struct MoggVol
        {
            public float l;
            public float r;

            public MoggVol(float l, float r)
            {
                this.l = l;
                this.r = r;
            }
        }

        public MemoryStream GetMemoryStream()
        {
            var moggString = ExportToText();
            return Utility.GenerateStreamFromString(moggString);
        }
    }
}