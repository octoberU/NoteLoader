using AudicaAPI;
using Newtonsoft.Json;
using NoteLoader;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using TMPro;
using UnityEngine;
using UnityToolbag;

public class SongDownloader : MonoBehaviour
{
    private const string apiURL = "http://www.audica.wiki:5000/api/customsongs?pagesize=9";
    [SerializeField] SongItemManager songItemManager;
    [SerializeField] TMP_InputField searchField;
    [SerializeField] SongList songList;
    [SerializeField] PageControls pageControls;

    private int pageNumber = 1;
    public int PageNumber {
        get 
        {
            return pageNumber;
        }
        set 
        {
            if (value > 0 && value <= songList.total_pages)
            {
                pageNumber = value;
                pageControls.UpdatePageDisplay(pageNumber, songList.total_pages);
            }
        }
    }
        

    void Start()
    {
        StartSongSearch();
    }

    public void OnKeyboardComplete()
    {
        pageNumber = 1;
        StartSongSearch(searchField.text, page: PageNumber);
    }

    public void OnPageChanged()
    {
        if (songList.page == pageNumber) return;
        StartSongSearch(searchField.text, page: PageNumber);
    }

    public void StartSongSearch(string search = null, string difficulty = null, int page = 1, bool curated = false, bool sortByPlaycount = false)
    {
        Future<SongList> futureSongList = QueryAudicaAPI(search, difficulty, page, curated, sortByPlaycount);
        futureSongList.OnSuccess((receivedSonglist) =>
        {
            songList = receivedSonglist.value;
            songItemManager.UpdateSongItems(songList);
            pageControls.UpdatePageDisplay(pageNumber, songList.total_pages);
        });
    }

    Future<SongList> QueryAudicaAPI(string search = null, string difficulty = null, int page = 1, bool curated = false, bool sortByPlaycount = false)
    {
        var future = new Future<SongList>();

        future.Process(() =>
        {
            using (WebClient client = new WebClient())
            {
                NameValueCollection searchParams = new NameValueCollection();

                if (search != null) searchParams.Add("search", search);
                if (difficulty != null) searchParams.Add(difficulty.ToLower(), "true");
                if (curated != false) searchParams.Add("curated", "true");
                if (sortByPlaycount) searchParams.Add("sort", "leaderboards");

                searchParams.Add("page", page.ToString());

                string paramString = "";
                foreach (var param in searchParams.AllKeys)
                {
                    paramString += $"&{param}={WebUtility.UrlEncode(searchParams[param])}";
                }
                
                string data = Encoding.UTF8.GetString(client.DownloadData(apiURL + paramString));
                return JsonConvert.DeserializeObject<SongList>(data);
            }
        });
        return future;
    }

    public static void DownloadAudicaFile(string url, bool hidePopup = false)
    {
        StartAudicaFileDownload(url);
        if(!hidePopup) PopupManager.I.CreatePopup($"Downloading {NLUtility.GetFileNameFromURL(url)}");
    }

    static Future<bool> StartAudicaFileDownload(string url)
    {
        var future = new Future<bool>();

        future.Process(() =>
        {
            using (WebClient client = new WebClient())
            {
                string audicaName = NLUtility.GetFileNameFromURL(url);
                string outputPath = Path.Combine(NLUtility.GetAudicaDirectory(), audicaName);
                try
                {
                    client.DownloadFile(url, outputPath);
                    return true;
                }
                catch (System.Exception)
                {
                    return false;
                }
            }
        });

        return future;
    }
}
