using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NLButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField]
    public UnityEvent onClick;

    public Image buttonBackground;

    void Awake()
    {
        buttonBackground = GetComponent<Image>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onClick.Invoke();
        buttonBackground.DOKill(true);
        transform.DOKill(true);
        transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.1f, 2, 0.2f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonBackground.DOKill(true);
        buttonBackground.DOFade(0.8f, 0.15f).SetEase(Ease.OutQuart);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        buttonBackground.DOKill(true);
        buttonBackground.DOFade(1f, 0.5f).SetEase(Ease.InQuart);
    }
}
