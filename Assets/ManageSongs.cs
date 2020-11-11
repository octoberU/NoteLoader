using NoteLoader.Cache;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityToolbag;

public class ManageSongs : MonoBehaviour
{
    [SerializeField] GameObject loadingAnimation;
    void OnEnable()
    {
        loadingAnimation.SetActive(true);
        Future<bool> prepareSongs = PrepareSongsOnSeparateThread();
        prepareSongs.OnComplete((success) =>
        {
            Debug.Log($"Processed {SongCache.songs.Count} songs");
            DisplayLocalSongs();
        });
    }

    void DisplayLocalSongs()
    {
        loadingAnimation.SetActive(false);
    }

    Future<bool> PrepareSongsOnSeparateThread()
    {
        Future<bool> future = new Future<bool>();

        future.Process(() =>
        {
            SongCache.ProcessSongs();
            return true;
        });

        return future;
    }
}
