using AudicaAPI;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SongItem : MonoBehaviour, IPointerClickHandler
{
    #region References
    [SerializeField] TextMeshProUGUI titleLabel;
    [SerializeField] TextMeshProUGUI artistLabel;
    [SerializeField] TextMeshProUGUI mapperLabel;

    [SerializeField] Transform expertIcon;
    [SerializeField] Transform advancedIcon;
    [SerializeField] Transform standardIcon;
    [SerializeField] Transform beginnerIcon;

    [SerializeField] PreviewSong previewSong;

    [SerializeField] Image background;
    #endregion

    private string downloadUrl;

    public void SetData(Song song)
    {
        titleLabel.text = song.title;
        artistLabel.text = song.artist;
        mapperLabel.text = "mapped by " + song.author;
        LayoutDifficultyIcons(song.beginner, song.standard, song.advanced, song.expert);
        downloadUrl = song.download_url;
        previewSong.previewUrl = song.preview_url; 
    }

    public void HideItem()
    {
        gameObject.SetActive(false);
    }

    public void ShowItem()
    {
        gameObject.SetActive(true);
        background.DOFade(1f, 0.2f);
    }


    public void LayoutDifficultyIcons(bool beginner, bool standard, bool advanced, bool expert)
    {
        //This is an absolute disaster, I couldn't think of a better method on the spot. Someone please rewrite this.
        expertIcon.gameObject.SetActive(expert);
        advancedIcon.gameObject.SetActive(advanced);
        standardIcon.gameObject.SetActive(standard);
        beginnerIcon.gameObject.SetActive(beginner);
        Vector3 defaultPosition = new Vector3(172f, -41.5f, 0f);
        Vector3 offset = new Vector3(-20, -0, 0f);
        int count = 0;
        if (expert)
        {
            expertIcon.localPosition = defaultPosition + (offset * count);
            count++;
        }
        else if (advanced)
        {
            advancedIcon.localPosition = defaultPosition + (offset * count);
            count++;
        }
        else if (standard)
        {
            standardIcon.localPosition = defaultPosition + (offset * count);
            count++;
        }
        else if (beginner)
        {
            beginnerIcon.localPosition = defaultPosition + (offset * count);
            count++;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SongDownloader.DownloadAudicaFile(downloadUrl);
        transform.DOShakeScale(2f, 0.02f, 0, 2, true);
        background.DOFade(0.7f, 0.5f);
    }
}
