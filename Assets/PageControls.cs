using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PageControls : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI pageLabel;
    [SerializeField] SongDownloader songDownloader;


    public void UpdatePageDisplay(int current, int pageCount)
    {
        pageLabel.text = $"Page {current}/{pageCount}";
    }

    public void NextPage()
    {
        songDownloader.PageNumber++;
    }

    public void PreviousPage()
    {
        songDownloader.PageNumber--;
    }

}
