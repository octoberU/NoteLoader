using AudicaTools;
using NoteLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.XR;

public class DebugTools : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI debugText;
    void Start()
    {
        string debugString = "";
        debugString += "Device model: " + OVRPlugin.GetSystemHeadsetType().ToString() + Environment.NewLine;

        debugString += "Version: 2" + Environment.NewLine;
        debugString += "Searching from " + NLUtility.GetNLDirectory() + Environment.NewLine;
        debugText.text = debugString;
        debugString += "Moving to " + NLUtility.GetAudicaDirectory() + Environment.NewLine;
        debugText.text = debugString;
        debugString += "Audica directory exists: " + Directory.Exists(NLUtility.GetAudicaDirectory()).ToString() + Environment.NewLine;
        debugText.text = debugString;
        debugString += "Attempting to find audica files" + Environment.NewLine;
        debugText.text = debugString;

        //try
        //{
        Stopwatch sw = new Stopwatch();
        sw.Start();
        foreach (var audicaFilePath in GetAllFiles(NLUtility.GetAudicaDirectory(), "*.audica"))
        {
            //debugString += "Found Audica File:" + audicaFile + Environment.NewLine;

            //Method 1
            //var audica = new Audica(audicaFilePath);
            //debugString += $"{audica.desc.title}-{audica.desc.artist} mapped by {audica.desc.author}";

            //var audica = Audica.GetMetadata(audicaFilePath);
            //debugString += $"{audica.desc.title} - {audica.fileInfo.FullName} \n";


        }
        print($"Loaded songs in: {sw.ElapsedMilliseconds}ms");
        debugText.text = debugString;


        NLUtility.PlayPreviewFromAudicaFile(this, FindObjectOfType<AudioSource>(), Path.Combine(NLUtility.GetAudicaDirectory(), "7Years-Continuum.audica"));

        //}
        //catch (Exception e)
        //{
        //    debugString += e.Message;
        //    debugText.text = debugString;
        //}


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

}
