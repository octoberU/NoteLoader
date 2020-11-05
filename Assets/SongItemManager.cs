using AudicaAPI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SongItemManager : MonoBehaviour
{
    [SerializeField] SongItem[] songItems;

    public void UpdateSongItems(SongList songlist)
    {
        int songItemCount = songItems.Length;
        int songCount = songlist.songs.Length;
        for (int i = 0; i < songItemCount; i++)
        {
            if (i < songCount)
            {
                songItems[i].SetData(songlist.songs[i]);
                songItems[i].ShowItem();
            }
            else songItems[i].HideItem();
        }
    }

}
