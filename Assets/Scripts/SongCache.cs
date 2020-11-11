using AudicaTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteLoader.Cache
{
    public static class SongCache
    {
        private static string songFolderHash;

        public static Dictionary<string, Audica.AudicaMetadata> songs;
        
        public static void ProcessSongs()
        {
            if (songFolderHash == GetLocalFolderHash()) return;
            songs = new Dictionary<string, Audica.AudicaMetadata>();
            var localPaths = NLUtility.GetLocalAudicaFiles();
            int numSongs = localPaths.Length;
            for (int i = 0; i < numSongs; i++)
            {
                var audicaMeta = Audica.GetMetadata(localPaths[i]);
                songs.Add(audicaMeta.weakHash, audicaMeta);
            }
        }

        private static string GetLocalFolderHash()
        {
            return NLUtility.CreateMD5(string.Join("&", NLUtility.GetLocalAudicaFiles()));
        }
    }
}
