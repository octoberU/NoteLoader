using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using DG.Tweening;
using UnityEngine.UI;

public class PreviewSong : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public AudioSource AudioHandler;
    private AudioClip previewSong;
    public string previewUrl;
    [SerializeField] Image icon;

    [SerializeField] Color activeColor;
    [SerializeField] Color inActiveColor;

    public void OnEnable()
    {
        icon.color = inActiveColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioHandler.DOKill(true);
        AudioHandler.Stop();
        StartCoroutine(SongCoroutine(previewUrl));
        AudioHandler.DOFade(1f, 1f);

        icon.DOColor(activeColor, 0.8f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        AudioHandler.DOKill(true);
        AudioHandler.Stop();
        StopCoroutine(SongCoroutine(previewUrl));
        AudioHandler.DOFade(0f, 0.5f);

        icon.DOColor(inActiveColor, 0.8f);
    }

    IEnumerator SongCoroutine(string path)
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.OGGVORBIS))
        {
            DownloadHandlerAudioClip dHA = new DownloadHandlerAudioClip(string.Empty, AudioType.OGGVORBIS);
            dHA.streamAudio = true;
            www.downloadHandler = dHA;
            www.SendWebRequest();
            while (www.downloadProgress < 0.01)
            {
                Debug.Log(www.downloadProgress);
                yield return new WaitForSeconds(.1f);
            }
            if (www.isNetworkError)
            {
                Debug.Log("error");
            }
            else
            {
                AudioHandler.clip = dHA.audioClip;
                AudioHandler.Play();
            }
        }
    }
}
