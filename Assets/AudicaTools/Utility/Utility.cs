using NAudio.Midi;
using NAudio.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AudicaTools
{
    internal static partial class Utility
    {
        public static MemoryStream GenerateStreamFromString(string value)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(value ?? ""));
        }

        private static uint SwapUInt32(uint i)
        {
            return ((i & 0xFF000000) >> 24) | ((i & 0x00FF0000) >> 8) | ((i & 0x0000FF00) << 8) | ((i & 0x000000FF) << 24);
        }

        private static ushort SwapUInt16(ushort i)
        {
            return (ushort)(((i & 0xFF00) >> 8) | ((i & 0x00FF) << 8));
        }

        public static MemoryStream ExportMidiToStream(MidiEventCollection events)
        {
            //Taken from https://github.com/naudio/NAudio/blob/master/NAudio/Midi/MidiFile.cs#L256 and edited to support MemoryStream export(thanks Mettra)
            if (events.MidiFileType == 0 && events.Tracks > 1)
            {
                throw new ArgumentException("Can't export more than one track to a type 0 file");
            }


            using (MemoryStream stream = new MemoryStream())
            {
                using (var writer = new BinaryWriter(stream))
                {

                    writer.Write(Encoding.UTF8.GetBytes("MThd"));
                    writer.Write(SwapUInt32(6)); // chunk size
                    writer.Write(SwapUInt16((ushort)events.MidiFileType));
                    writer.Write(SwapUInt16((ushort)events.Tracks));
                    writer.Write(SwapUInt16((ushort)events.DeltaTicksPerQuarterNote));

                    for (int track = 0; track < events.Tracks; track++)
                    {
                        IList<MidiEvent> eventList = events[track];

                        writer.Write(Encoding.UTF8.GetBytes("MTrk"));
                        long trackSizePosition = writer.BaseStream.Position;
                        writer.Write(SwapUInt32(0));

                        long absoluteTime = events.StartAbsoluteTime;

                        // use a stable sort to preserve ordering of MIDI events whose 
                        // absolute times are the same
                        MergeSort.Sort(eventList, new MidiEventComparer());
                        if (eventList.Count > 0)
                        {
                            System.Diagnostics.Debug.Assert(MidiEvent.IsEndTrack(eventList[eventList.Count - 1]), "Exporting a track with a missing end track");
                        }
                        foreach (var midiEvent in eventList)
                        {
                            midiEvent.Export(ref absoluteTime, writer);
                        }

                        uint trackChunkLength = (uint)(writer.BaseStream.Position - trackSizePosition) - 4;
                        writer.BaseStream.Position = trackSizePosition;
                        writer.Write(SwapUInt32(trackChunkLength));
                        writer.BaseStream.Position += trackChunkLength;
                    }
                }
                stream.Flush();
                byte[] bytes = stream.GetBuffer();
                return new MemoryStream(bytes);
            }

        }



    }
}
