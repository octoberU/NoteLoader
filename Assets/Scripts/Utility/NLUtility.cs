
using System.IO;
using UnityEngine;

namespace NoteLoader
{
    public static class NLUtility
    {
        /// <summary>
        /// Returns NoteLoader's directory without the data folder.
        /// </summary>
        public static string GetNLDirectory()
        {
            return Application.persistentDataPath;
        }

        public static string GetAudicaDirectory()
        {
            string androidPath = Directory.GetParent(Directory.GetParent(Directory.GetParent(Application.persistentDataPath).ToString()).ToString()).ToString();
            return Path.Combine(androidPath, "obb", "com.harmonixmusic.kata");
        }

        public static string GetFileNameFromURL(string url)
        {
            string[] splitUrl = url.Split('/');
            return splitUrl[splitUrl.Length - 1];
        }
    }
}
