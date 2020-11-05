using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SearchBar : MonoBehaviour
{
    [SerializeField] Image glyph;
    public void HideGlyph()
    {
        glyph.DOFade(0f, 0.5f).OnComplete(() => glyph.gameObject.SetActive(false));
    }
}
