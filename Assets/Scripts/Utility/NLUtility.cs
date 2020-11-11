
using AudicaTools;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

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
#if UNITY_EDITOR
            return @"E:\Steam\steamapps\common\Audica\Audica_Data\StreamingAssets\HmxAudioAssets\songs_og";
#endif

            string androidPath = Directory.GetParent(Directory.GetParent(Directory.GetParent(Application.persistentDataPath).ToString()).ToString()).ToString();
            return Path.Combine(androidPath, "obb", "com.harmonixmusic.kata");
        }

        public static string GetFileNameFromURL(string url)
        {
            string[] splitUrl = url.Split('/');
            return splitUrl[splitUrl.Length - 1];
        }

        public static void PlayPreviewFromAudicaFile(MonoBehaviour mono, AudioSource audioSource, string audicaPath)
        {
            var audica = new Audica(audicaPath);

            string tempPath = Path.Combine(NLUtility.GetNLDirectory(), "Temp");
            string tempAudioPath = Path.Combine(tempPath, "preview.ogg");
            if (!Directory.Exists(tempPath)) Directory.CreateDirectory(tempPath);
            audica.song.ExportToOgg(tempAudioPath);

            mono.StartCoroutine(NLUtility.PlayAudioClip(tempAudioPath, audioSource, (float)audica.desc.previewStartSeconds));
        }

        public static IEnumerator PlayAudioClip(string filePath, AudioSource audioSource, float startTime = 0f)
        {
            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(filePath, AudioType.OGGVORBIS))
            {
                yield return www.SendWebRequest();

                if (www.isNetworkError)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    AudioClip ac = DownloadHandlerAudioClip.GetContent(www);
                    audioSource.clip = ac;
                    audioSource.time = startTime;
                    audioSource.Play();
                }
            }
        }

        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString().ToLower();
            }
        }

        private static IEnumerable<string> GetAllFiles(string path, string searchPattern)
        {
            return Directory.EnumerateFiles(path, searchPattern).Union(
            Directory.EnumerateDirectories(path).SelectMany(d =>
            {
                try
                {
                    return GetAllFiles(d, searchPattern);
                }
                catch (Exception e)
                {
                    return Enumerable.Empty<string>();
                }
            }));
        }

        /// <summary>
        /// Returns an array of file paths to local audica files.
        /// </summary>
        /// <returns></returns>
        public static string[] GetLocalAudicaFiles()
        {
            return GetAllFiles(NLUtility.GetAudicaDirectory(), "*.audica").ToArray();
        }
    }
}
