using System;
using System.Collections.Generic;

namespace AudicaTools
{
    public class TempoData
    {
        public int tick;
        public UInt64 microsecondsPerQuarterNote;

        public TempoData(int tick, ulong microsecondsPerQuarterNote)
        {
            this.tick = tick;
            this.microsecondsPerQuarterNote = microsecondsPerQuarterNote;
        }

        public static double GetBPMFromMicrosecondsPerQuaterNote(UInt64 microsecondsPerQuarterNote)
        {
            return (double)60000000 / microsecondsPerQuarterNote;
        }

        public static UInt64 MicrosecondsPerQuarterNoteFromBPM(double bpm)
        {
            if (bpm == 0) { return 0; }
            return (UInt64)Math.Round((double)60000000 / bpm);
        }

        public static float TickToMilliseconds(int tick, List<TempoData> tempoDataList)
        {
            UInt64 microsecond = 0;
            TempoData prevTempoData = tempoDataList[0];
            for (int i = 1; i < tempoDataList.Count && tempoDataList[i].tick < tick; i++)
            {
                TempoData nextTempoData = tempoDataList[i];
                microsecond += prevTempoData.microsecondsPerQuarterNote * (UInt64)(nextTempoData.tick - prevTempoData.tick) / 480;
                prevTempoData = nextTempoData;
            }
            microsecond += prevTempoData.microsecondsPerQuarterNote * (UInt64)(tick - prevTempoData.tick) / 480;
            return (float)microsecond/1000;
        }
    }
}
