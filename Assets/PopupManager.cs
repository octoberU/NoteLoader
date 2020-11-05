using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    #region Singleton Pattern
    public static PopupManager I;
    void Awake()
    {
        if (I == null) I = this;
    } 
    #endregion

    [SerializeField] CanvasGroup popupCanvas;
    [SerializeField] TextMeshProUGUI popupLabel;

    private bool promptActive = false;
    const float popupTimer = 3.5f;

    private Queue<string> popupQueue = new Queue<string>();

    public void CreatePopup(string popupString)
    {
        if (promptActive) popupQueue.Enqueue(popupString);
        else
        {
            promptActive = true;
            popupLabel.text = popupString;
            popupCanvas.gameObject.SetActive(true);
            popupCanvas
                .DOFade(1f, 0.5f)
                .OnComplete(() =>
                {
                    popupCanvas
                    .DOFade(0.99f, popupTimer)
                    .OnComplete(() => ClosePopup());
                }); 
        }
    }

    public void ClosePopup()
    {
        popupCanvas
            .DOFade(0f, 0.5f)
            .OnComplete(() =>
            {
                popupCanvas.gameObject.SetActive(false);
                promptActive = false;
                CheckQueue();
            });

        void CheckQueue()
        {
            if (popupQueue.Count > 0)
            {
                var newPopup = popupQueue.Dequeue();
                CreatePopup(newPopup);
            }
        }
    }

    public void FeatureNotYetAvailable()
    {
        CreatePopup("Coming soon.");
    }
}
